using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.Data;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

namespace _5_8AOI
{
    class DataOperator
    {
        //保存当前地图对象
        public IMap m_map;
        //用于传入当前地图对象,构造函数
        public DataOperator(IMap map)
        {
            m_map = map;
        }
        //获取图层
        public ILayer GetLayerByName(string sLayerName)
        {
            //判断图层名或地图对象是否为空，若为空，函数返回空
            if (sLayerName == "" || m_map == null)
            {
                return null;
            }
            //对地图对象中的所有图层进行遍历，若某一图层的名称与指定图层名相同，则返回该图层
            for (int i = 0; i < m_map.LayerCount; i++)
            {
                if (m_map.get_Layer(i).Name == sLayerName)
                {
                    return m_map.get_Layer(i);
                }
            }
            //若地图对象中的所有图层名均与指定图层名不匹配，函数返回空
            return null;
        }
        public DataTable GetContinentsNames()
        {
            ILayer layer = GetLayerByName("Continents");
            IFeatureLayer featureLayer = layer as IFeatureLayer;    //      接口查询(Query Interface)
            if (featureLayer == null)
            {
                return null;
            }
            IFeature feature;
            IFeatureCursor featureCursor = featureLayer.Search(null, false);
            feature = featureCursor.NextFeature();
            if (feature == null)
            {
                return null;
            }
            DataTable dataTable = new DataTable();  //用于函数返回
            DataColumn dataColumn = new DataColumn();
            dataColumn.ColumnName = "序号";
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataTable.Columns.Add(dataColumn);
            dataColumn = new DataColumn();
            dataColumn.ColumnName = "名称";
            dataColumn.DataType = System.Type.GetType("System.String");
            dataTable.Columns.Add(dataColumn);
            DataRow dataRow;
            while (feature != null)
            {
                dataRow = dataTable.NewRow();
                dataRow[0] = feature.get_Value(0);
                dataRow[1] = feature.get_Value(2);
                dataTable.Rows.Add(dataRow);
                feature = featureCursor.NextFeature();
            }
            return dataTable;
        }
        public IFeatureClass CreateShapefile(String sParentDirectory,   //上级路径
            String sWorkspaceName,      //包含Shape文件的文件夹名 
            string sFileName)           //Shape文件名
        { 
            //判定路径和文件夹是否已存在,存在则删除
            if (Directory.Exists(sParentDirectory + sWorkspaceName))
            {
                Directory.Delete(sParentDirectory + sWorkspaceName, true);
            }
            //创建工作空间工厂对象，再创建Shape文件工作空间
            IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactoryClass();
            IWorkspaceName workspaceName = workspaceFactory.Create(sParentDirectory,sWorkspaceName,null,0);     //创建一个指定的工作空间
            IName name = workspaceName as IName;
            //打开新建的工作空间
            IWorkspace workspace = name.Open() as IWorkspace;
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
            //创建并编辑该要素类所需的字段集
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = fields as IFieldsEdit;
            //创建并编辑“序号”字段
            IFieldEdit fieldEdit = new FieldClass();
            fieldEdit.Name_2 = "OID";
            fieldEdit.AliasName_2 = "序号";   //别名
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            IField field = fieldEdit as IField;
            fieldsEdit.AddField(field);     //直接打完方法名
            //创建并编辑“名称”字段
            fieldEdit = new FieldClass();
            fieldEdit.Name_2 = "Name";
            fieldEdit.AliasName_2 = "名称";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            fieldsEdit.AddField((IField)fieldEdit);
            //创建地理定义，设置其空间参考和几何类型，为创建“形状”字段做准备
            IGeometryDefEdit geoDefEdit = new GeometryDefClass();
            ISpatialReference spatialReference = m_map.SpatialReference;
            geoDefEdit.SpatialReference_2 = spatialReference;
            geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
            //创建并编辑“形状”字段
            fieldEdit = new FieldClass();
            String sShapeFileName = "Shape";
            fieldEdit.Name_2 = sShapeFileName;
            fieldEdit.AliasName_2 = "形状";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            fieldEdit.GeometryDef_2 = geoDefEdit;
            fieldsEdit.AddField((IField)fieldEdit);
            //创建要素类
            IFeatureClass featureClass = featureWorkspace.CreateFeatureClass(sFileName,fields,null,null,esriFeatureType.esriFTSimple,"Shape","");
            //判断是否创建成功
            if (featureClass == null)
            {
                return null;
            }
            //返回创建好的要素类
            return featureClass;
        }
        //将指定的要素类以图层的形式添加到类保存的地图对象中，并同时指定图层的名称
        public bool AddFeatureClassToMap(IFeatureClass featureClass, String sLayerName)
        { 
            //判断要素类、图层名和地图对象是否为空
            if (featureClass == null || sLayerName == "" || m_map == null)
            {
                return false;
            }
            //创建要素图层对象，将要素类以层的形式进行操作
            IFeatureLayer featureLayer = new FeatureLayerClass();
            featureLayer.FeatureClass = featureClass;
            featureLayer.Name = sLayerName;
            //将要素类转换为一般图层，并判断是否成功
            ILayer layer = featureLayer as ILayer;
            if (layer == null)
            {
                return false; 
            }
            //将创建好的图层添加至地图对象，并将地图对象转化为活动视图
            m_map.AddLayer(layer);
            IActiveView activeView = m_map as IActiveView;
            if (activeView == null)
            {
                return false;
            }
            //活动视图进行刷新，新添加的图层将被展示在控件中
            activeView.Refresh();
            return true;
        }
        public bool AddFeatureToLayer(
            string sLayerName,      //指定图层的名称
            string sFeatureName,    //将被添加的要素的名称
            IPoint point            //将被添加的要素的坐标信息
            )
        { 
            //判断图层名、要素名、要素坐标和地图对象是否为空。
            if (sLayerName == "" || sFeatureName == "" || point == null || m_map == null)
            {
                return false;
            }
            //对地图对象中的图层进行遍历。当某图层的名称与指定名称相同时，则跳出循环
            ILayer layer = null;
            for (int i = 0; i < m_map.LayerCount; i++)
            {
                layer = m_map.get_Layer(i);
                if (layer.Name == sLayerName)
                {
                    break;
                }
                layer = null;
            }
            //判断图层是否获取成功
            if (layer == null)
            {
                return false;
            }
            //访问获取到的图层，并进一步获取其要素类
            IFeatureLayer featureLayer = layer as IFeatureLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            //访问要素类新创建的要素，并判断是否成功
            IFeature feature = featureClass.CreateFeature();
            if (feature == null)
            {
                return false;
            }
            //对新创建的要素进行编辑，将其坐标、属性值进行设置，最后保存该要素
            feature.Shape = point;
            int index = feature.Fields.FindField("Name");
            feature.set_Value(index,sFeatureName);
            feature.Store();
            if (feature == null)
            {
                return false;
            }
            //将地图对象转化为活动视图，并判断是否成功
            IActiveView activeView = m_map as IActiveView;
            if (activeView == null)
            {
                return false;
            }
            //活动视图进行刷新，新添加的要素将被展示在控件中。
            activeView.Refresh();
            return true;
        }
    }
}
