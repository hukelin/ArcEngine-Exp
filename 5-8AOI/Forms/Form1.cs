using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.GlobeCore;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesRaster;

namespace _5_8AOI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region 打开*.mxd文档
        private void miOpenMxd_Click(object sender, EventArgs e)
        {
            OpenFileDialog pOpenFileDialog = new OpenFileDialog();
            pOpenFileDialog.CheckFileExists = true;
            pOpenFileDialog.Title = "打开地图文档";
            pOpenFileDialog.Filter = "ArcMap Documents(*.mxd)|*.mxd;|ArcMAP 模板(*.mxt)|(*.mxt);|发布地图文件(*.pmf)|(*.pmf)|所有地图格式(*.mxd;*.mxt;*.pmf)|(*.mxd;*.mxt;*.pmf)";
            pOpenFileDialog.Multiselect = false;        //不允许多个文件同时选择
            pOpenFileDialog.RestoreDirectory = true;    //存储打开的文件路径 
            if (pOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string pFileName = pOpenFileDialog.FileName;
                if (pFileName == "")
                {
                    return;
                }
                if (mainMapControl.CheckMxFile(pFileName))       //检查地图文档的有效性
                {
                    mainMapControl.LoadMxFile(pFileName);
                }
                else
                {
                    MessageBox.Show(pFileName + "是无效的地图文档！", "信息提示");
                    return;
                }
            }
        }
        #endregion

        #region 打开*.shp文件
        private void miOpenShapefile_Click(object sender, EventArgs e)
        {
            OpenFileDialog pOpenFileDialog = new OpenFileDialog();
            pOpenFileDialog.CheckFileExists = true;
            pOpenFileDialog.Title = "打开Shape文件";
            pOpenFileDialog.Filter = "Shape文件(*.shp)|*.shp";
            pOpenFileDialog.ShowDialog();
            //获取文件路径
            IWorkspaceFactory pWorkspaceFactory;
            IFeatureWorkspace pFeatureWorkspace;
            IFeatureLayer pFeatureLayer;
            string pFullPath = pOpenFileDialog.FileName;
            if (pFullPath == "")
                return;
            int pIndex = pFullPath.LastIndexOf("\\");
            string pFilePath = pFullPath.Substring(0, pIndex);       //文件路径
            string pFileName = pFullPath.Substring(pIndex + 1);     //文件
            //实例化ShapefileWorkspaceFactory工作空间，打开Shapefile文件
            pWorkspaceFactory = new ShapefileWorkspaceFactory();
            pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(pFilePath, 0);
            //创建并实例化要素集
            IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(pFileName);
            pFeatureLayer = new FeatureLayer();
            pFeatureLayer.FeatureClass = pFeatureClass;
            pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
            ClearAllData();
            mainMapControl.Map.AddLayer(pFeatureLayer);
            mainMapControl.ActiveView.Refresh();
            //同步鹰眼
            SynchronizeEagleEye();
        }
        #endregion

        #region 加载工作空间中的数据
        private void AddAllDataset(IWorkspace pWorkspace, AxMapControl mapControl)
        {
            IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTAny);
            pEnumDataset.Reset();   //重置数据集序列，使指针位于第一个数据集之前
            IDataset pDataset = pEnumDataset.Next();    //数据集中的数据一个一个读取
            //判断数据集是否有数据
            while (pDataset != null)
            {
                if (pDataset is IFeatureDataset) //要素数据集
                {
                    IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                    IFeatureDataset pFeatureDataset = pFeatureWorkspace.OpenFeatureDataset(pDataset.Name);
                    IEnumDataset pEnumDataset1 = pFeatureDataset.Subsets;
                    pEnumDataset1.Reset();
                    IGroupLayer pGroupLayer = new GroupLayerClass();
                    pGroupLayer.Name = pFeatureDataset.Name;
                    IDataset pDataset1 = pEnumDataset1.Next();
                    while (pDataset1 != null)
                    {
                        if (pDataset1 is IFeatureClass) //要素类
                        {
                            IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                            pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(pDataset1.Name);
                            if (pFeatureLayer.FeatureClass != null)
                            {
                                pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                                pGroupLayer.Add(pFeatureLayer);
                                mapControl.Map.AddLayer(pFeatureLayer);
                            }
                        }
                        pDataset1 = pEnumDataset1.Next();
                    }
                }
                else if (pDataset is IFeatureClass)  //要素类
                {
                    IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                    IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                    pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(pDataset.Name);
                    pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                    mapControl.Map.AddLayer(pFeatureLayer);
                }
                else if (pDataset is IRasterDataset)    //栅格数据集
                {
                    IRasterWorkspaceEx pRasterworkspace = pWorkspace as IRasterWorkspaceEx;
                    IRasterDataset pRasterDataset = pRasterworkspace.OpenRasterDataset(pDataset.Name);
                    //影像金字塔判断与创建
                    IRasterPyramid3 pRasPyrmid = pRasterDataset as IRasterPyramid3;
                    if (pRasPyrmid != null)
                    {
                        if (!(pRasPyrmid.Present))  //Indicates whether pyramid layers exist
                        {
                            pRasPyrmid.Create();    //创建金字塔
                        }
                    }
                    IRasterLayer pRasterLayer = new RasterLayerClass();
                    pRasterLayer.CreateFromDataset(pRasterDataset);
                    ILayer pLayer = pRasterLayer as ILayer;
                    mapControl.AddLayer(pLayer, 0);
                }
                pDataset = pEnumDataset.Next();
            }
            mapControl.ActiveView.Refresh();
            //同步鹰眼
            SynchronizeEagleEye();
        }
        #endregion

        #region 加载个人地理数据库
        private void miOpenAccess_Click(object sender, EventArgs e)
        {
            OpenFileDialog pOenFileDialog = new OpenFileDialog();
            pOenFileDialog.Filter = "Personal Geodatabase(*.mdb)|*.mdb";
            pOenFileDialog.Title = "打开PersonGeodatabase文件";
            pOenFileDialog.ShowDialog();    //弹出模式窗体
            string pFullPath = pOenFileDialog.FileName;
            if (pFullPath == "") return;
            IWorkspaceFactory pAccessWorkspaceFactory = new AccessWorkspaceFactory();
            IWorkspace pWorkspace = pAccessWorkspaceFactory.OpenFromFile(pFullPath, 0);
            //加载工作空间里的数据
            ClearAllData();     //删除所有已加载的数据
            AddAllDataset(pWorkspace, mainMapControl);
        }
        #endregion

        #region 打开Raster数据
        private void miOpenRaster_Click(object sender, EventArgs e)
        {
            OpenFileDialog pOpenFileDialog = new OpenFileDialog();      //初始化一个OpenFileDialog类的实例
            pOpenFileDialog.CheckFileExists = true;         //CheckFileExists属性获取或设置一个值，如果该值指示用户指定不存在的文件，对话框是否显示警告
            pOpenFileDialog.Title = "打开Raster文件";        //Title属性获取或设置文件对话框标题
            pOpenFileDialog.Filter = "栅格文件(*.*)|*.bmp;*.tif;*.jpg;*.img|(*.bmp)|*.bmp|(*.tif)|*.tif|(*.jpg)|*.jpg|(*.img)|*.img";   //Filter属性获取或设置当前文件名筛选器字符串，该字符串决定对话框的“另存为文件名”或“文件名”框中出现的选择内容
            pOpenFileDialog.ShowDialog();   //ShowDialog()方法用默认的所有者运行通用对话框
            string pRasterFilename = pOpenFileDialog.FileName;  //string定义一个字符串变量pRasterFilename，FileName属性获取或设置一个包含在文件对话框中选定的文件名的字符串
            if (pRasterFilename == "") return;
            string pPath = System.IO.Path.GetDirectoryName(pRasterFilename);    //返回指定路径字符串的目录信息
            string pFileName = System.IO.Path.GetFileName(pRasterFilename);     //返回指定路径字符串的文件名和扩展名
            IWorkspaceFactory pWorkspaceFactory = new RasterWorkspaceFactory();     //实例化栅格文件工作空间     
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(pPath, 0);       //Workspace类可以通过WorkspaceFactory类的OpenFormFile方法来创建
            IRasterWorkspace pRasterWorkspace = pWorkspace as IRasterWorkspace;     //将pWorkspace工作空间转化到IRasterWorkspace接口上来
            IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset(pFileName);      //用IRasterWorkspace接口的OpenRasterDataset方法获得栅格文件的数据集，并实例化栅格文件对象
            //影像金字塔判断与创建(注：影像金字塔是由原始影像按一定规则生成的由细到粗不同分辨率的影像集。金字塔的底部是图像的高分辨率表示，也就是原始图像，而顶部是低分辨率的近似。最底层的分辨率最高，并且数据量最大，随着层数的增加，其分辨率逐渐降低，数据量也按比例减少。)
            IRasterPyramid3 pRasPyrmid;     //声明pRasPyrmid变量类型（Pyramid：金字塔）
            pRasPyrmid = pRasterDataset as IRasterPyramid3;     //将pRasterDataset文件对象转化到IRasterPyramid3接口上来
            //对栅格数据集是否具有金子塔进行判断
            if (pRasPyrmid != null)
            {
                if (!(pRasPyrmid.Present))   //栅格文件不具有金字塔则创建金字塔
                {
                    pRasPyrmid.Create();    //创建金字塔
                }
            }
            IRaster pRaster;
            pRaster = pRasterDataset.CreateDefaultRaster();
            IRasterLayer pRasterLayer;
            pRasterLayer = new RasterLayerClass();
            pRasterLayer.CreateFromRaster(pRaster);     //为一个栅格对象创建一个图层
            ILayer pLayer = pRasterLayer as ILayer;
            mainMapControl.AddLayer(pLayer, 0);
            //同步鹰眼
            SynchronizeEagleEye();
        }
        #endregion

        #region 删除所有已加载的数据
        private void ClearAllData()
        {
            if (mainMapControl.Map != null && mainMapControl.Map.LayerCount > 0)
            {
                //新建mainMapControl中Map
                IMap dataMap = new MapClass();
                dataMap.Name = "Map";
                mainMapControl.DocumentFilename = string.Empty;
                mainMapControl.Map = dataMap;

                //新建EagleEyeMapControl中Map
                IMap eagleEyeMap = new MapClass();
                eagleEyeMap.Name = "eagleEyeMap";
                EagleEyeMapControl.DocumentFilename = string.Empty;
                EagleEyeMapControl.Map = eagleEyeMap;
            }
        }
        #endregion

        #region 鹰眼的实现及同步
        //鹰眼同步
        private bool bCanDrag;              //鹰眼地图上的矩形框可移动的标志
        private IPoint pMoveRectPoint;      //记录在移动鹰眼地图上的矩形框时鼠标的位置
        private IEnvelope pEnv;             //记录数据视图的Extent
        private IRgbColor GetRgbColor(int intR, int intG, int intB)
        {
            IRgbColor pRgbColor = null;
            if (intR < 0 || intR > 255 || intG < 0 || intG > 255 || intB < 0 || intB > 255)
            {
                return pRgbColor;
            }
            pRgbColor = new RgbColorClass();
            pRgbColor.Red = intR;
            pRgbColor.Green = intG;
            pRgbColor.Blue = intB;
            return pRgbColor;
        }

        private void mainMapControl_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            SynchronizeEagleEye();
        }

        private void SynchronizeEagleEye()
        {
            if (EagleEyeMapControl.LayerCount > 0)
            {
                EagleEyeMapControl.ClearLayers();
            }
            //设置鹰眼和主地图的坐标系统一致
            EagleEyeMapControl.SpatialReference = mainMapControl.SpatialReference;
            for (int i = mainMapControl.LayerCount - 1; i >= 0; i--)
            {
                //使鹰眼视图与数据视图的图层上下顺序保持一致
                ILayer pLayer = mainMapControl.get_Layer(i);
                if (pLayer is IGroupLayer || pLayer is ICompositeLayer)
                {
                    ICompositeLayer pCompositeLayer = (ICompositeLayer)pLayer;
                    for (int j = pCompositeLayer.Count - 1; j >= 0; j--)
                    {
                        ILayer pSubLayer = pCompositeLayer.get_Layer(j);
                        IFeatureLayer pFeatureLayer = pSubLayer as IFeatureLayer;
                        if (pFeatureLayer != null)
                        {
                            //由于鹰眼地图较小，所以过滤点图层不添加
                            if (pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPoint
                                && pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryMultipoint)
                            {
                                EagleEyeMapControl.AddLayer(pLayer);
                            }
                        }
                    }
                }
                else
                {
                    IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                    if (pFeatureLayer != null)
                    {
                        if (pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPoint
                            && pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryMultipoint)
                        {
                            EagleEyeMapControl.AddLayer(pLayer);
                        }
                    }
                }
                //设置鹰眼地图全图显示  
                EagleEyeMapControl.Extent = mainMapControl.FullExtent;
                pEnv = mainMapControl.Extent as IEnvelope;
                DrawRectangle(pEnv);
                EagleEyeMapControl.ActiveView.Refresh();
            }
        }

        private void EagleEyeMapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (EagleEyeMapControl.Map.LayerCount > 0)
            {
                //按下鼠标左键移动矩形框
                if (e.button == 1)
                {
                    //如果指针落在鹰眼的矩形框中，标记可移动
                    if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
                    {
                        bCanDrag = true;
                    }
                    pMoveRectPoint = new PointClass();
                    pMoveRectPoint.PutCoords(e.mapX, e.mapY);  //记录点击的第一个点的坐标
                }
                //按下鼠标右键绘制矩形框
                else if (e.button == 2)
                {
                    IEnvelope pEnvelope = EagleEyeMapControl.TrackRectangle();

                    IPoint pTempPoint = new PointClass();
                    pTempPoint.PutCoords(pEnvelope.XMin + pEnvelope.Width / 2, pEnvelope.YMin + pEnvelope.Height / 2);
                    mainMapControl.Extent = pEnvelope;
                    //矩形框的高宽和数据试图的高宽不一定成正比，这里做一个中心调整
                    mainMapControl.CenterAt(pTempPoint);
                }
            }
        }

        //移动矩形框
        private void EagleEyeMapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
            {
                //如果鼠标移动到矩形框中，鼠标换成小手，表示可以拖动
                EagleEyeMapControl.MousePointer = esriControlsMousePointer.esriPointerHand;
                if (e.button == 2)  //如果在内部按下鼠标右键，将鼠标演示设置为默认样式
                {
                    EagleEyeMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
                }
            }
            else
            {
                //在其他位置将鼠标设为默认的样式
                EagleEyeMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }

            if (bCanDrag)
            {
                double Dx, Dy;  //记录鼠标移动的距离
                Dx = e.mapX - pMoveRectPoint.X;
                Dy = e.mapY - pMoveRectPoint.Y;
                pEnv.Offset(Dx, Dy); //根据偏移量更改 pEnv 位置
                pMoveRectPoint.PutCoords(e.mapX, e.mapY);
                DrawRectangle(pEnv);
                mainMapControl.Extent = pEnv;
            }
        }

        private void EagleEyeMapControl_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            if (e.button == 1 && pMoveRectPoint != null)
            {
                if (e.mapX == pMoveRectPoint.X && e.mapY == pMoveRectPoint.Y)
                {
                    mainMapControl.CenterAt(pMoveRectPoint);
                }
                bCanDrag = false;
            }
        }

        //绘制矩形框
        private void mainMapControl_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            //得到当前视图范围
            pEnv = (IEnvelope)e.newEnvelope;
            DrawRectangle(pEnv);
        }

        //在鹰眼地图上面画矩形框
        private void DrawRectangle(IEnvelope pEnvelope)
        {
            //在绘制前，清除鹰眼中之前绘制的矩形框
            IGraphicsContainer pGraphicsContainer = EagleEyeMapControl.Map as IGraphicsContainer;
            IActiveView pActiveView = pGraphicsContainer as IActiveView;
            pGraphicsContainer.DeleteAllElements();
            //得到当前视图范围
            IRectangleElement pRectangleElement = new RectangleElementClass();
            IElement pElement = pRectangleElement as IElement;
            pElement.Geometry = pEnvelope;
            //设置矩形框（实质为中间透明度面）
            IRgbColor pColor = new RgbColorClass();
            pColor = GetRgbColor(255, 0, 0);
            pColor.Transparency = 255;
            ILineSymbol pOutLine = new SimpleLineSymbolClass();
            pOutLine.Width = 2;
            pOutLine.Color = pColor;

            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pColor = new RgbColorClass();
            pColor.Transparency = 0;
            pFillSymbol.Color = pColor;
            pFillSymbol.Outline = pOutLine;
            //向鹰眼中添加矩形框
            IFillShapeElement pFillShapeElement = pElement as IFillShapeElement;
            pFillShapeElement.Symbol = pFillSymbol;
            pGraphicsContainer.AddElement((IElement)pFillShapeElement, 0);
            //刷新
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        #endregion

        #region AOI书签
        //创建AOI书签
        public void CreateBookmark(string sBookmarkName)
        {
            //获取书签
            IAOIBookmark aoibookmark = new AOIBookmarkClass();
            if (aoibookmark != null)
            {
                aoibookmark.Location = mainMapControl.ActiveView.Extent;
                aoibookmark.Name = sBookmarkName;
            }
            //在地图上增加书签
            IMapBookmarks bookmarks = mainMapControl.Map as IMapBookmarks;
            if (bookmarks != null)
            {
                bookmarks.AddBookmark(aoibookmark);
            }
            cbBookmarkList.Items.Add(aoibookmark.Name);
        }
        //输入书签名
        private void miCreateBookmark_Click(object sender, EventArgs e)
        {
            AdmitBookmarkName frmABN = new AdmitBookmarkName(this);
            frmABN.ShowDialog();
        }

        //创建书签索引
        private void cbBookmarkList_SelectedIndexChanged(object sender, EventArgs e)
        {
            IMapBookmarks bookmarks = mainMapControl.Map as IMapBookmarks;
            IEnumSpatialBookmark enumSpatialBookmark = bookmarks.Bookmarks;
            enumSpatialBookmark.Reset();
            ISpatialBookmark spatialbookmark = enumSpatialBookmark.Next();
            while (spatialbookmark != null)
            {
                if (cbBookmarkList.SelectedItem.ToString() == spatialbookmark.Name)
                {
                    spatialbookmark.ZoomTo((IMap)mainMapControl.ActiveView);
                    mainMapControl.ActiveView.Refresh();
                    break;
                }
                spatialbookmark = enumSpatialBookmark.Next();
            }
        }
        #endregion

        #region Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;       //设置窗口最大化
            mainMapControl.Map.Name = "Layer";      //修改图层名
        }
        #endregion

        #region 访问图层数据
        private void miAccessData_Click(object sender, EventArgs e)
        {
            DataOperator dataOperator = new DataOperator(mainMapControl.Map);
            DataBoard dataBoard = new DataBoard("各大洲洲名", dataOperator.GetContinentsNames());
            dataBoard.Show();
        }
        #endregion

        #region 地图渲染
        //为主窗体的“简单渲染图层”菜单项生成“点击”事件响应函数，并添加代码实现对“World Cirtis”图层的简单渲染。
        private void miRenderSimply_Click(object sender, EventArgs e)
        {
            //获取“World Cities”图层
            DataOperator dataOperator = new DataOperator(mainMapControl.Map);
            ILayer layer = dataOperator.GetLayerByName("World Cities");
            //通过IRgbColor接口新建RgbColor类型对象，将其设置为红色
            IRgbColor rgbColor = new RgbColorClass();
            rgbColor.Red = 255;
            rgbColor.Green = 0;
            rgbColor.Blue = 0;
            //获取“World Cities”图层的符号信息，并通过IColor接口访问设置好的颜色对象
            ISymbol symbol = MapComposer.GetSymbolFromLayer(layer);     //判断图层名或地图对象是否为空
            IColor color = rgbColor as IColor;
            //实现“World Cities”图层的简单渲染，并判断是否成功
            //若函数返回true，当前活动视图刷新，显示渲染效果，并使得“图层简单渲染”菜单项不再可用
            //若函数返回false，消息框提示“简单渲染图层失败！”
            bool bRes = MapComposer.RenderSimply(layer, color);
            if (bRes)
            {
                axTOCControl1.ActiveView.ContentsChanged();
                mainMapControl.ActiveView.Refresh();
                miRenderSimply.Enabled = false;
            }
            else
            {
                MessageBox.Show("简单渲染图层失败！");
            }
        }

        private void miGeRendererInfo_Click(object sender, EventArgs e)
        {
            DataOperator dataOperator = new DataOperator(mainMapControl.Map);
            ILayer layer = dataOperator.GetLayerByName("World Cities");
            //消息框提示该图层的渲染器类型信息
            string RendererType = MapComposer.GetRendererTypeByLayer(layer);
            MessageBox.Show(RendererType);
        }
        #endregion

        #region 创建shapefile
        private void miCreateShapefile_Click(object sender, EventArgs e)
        {
            //创建Shape文件，将其以要素类形式获取
            DataOperator dataOperator = new DataOperator(mainMapControl.Map);
            IFeatureClass featureClass = dataOperator.CreateShapefile("L:\\", "ShpapefileWorkspace", "ShapefileSample");
            if (featureClass == null)
            {
                MessageBox.Show("创建Shape文件失败！");
                return;
            }
            bool bRes = dataOperator.AddFeatureClassToMap(featureClass, "Observation Stations");
            if (bRes)
            {
                miCreateShapefile.Enabled = false;
                miAddFeature.Enabled = true;
                return;
            }
            else
            {
                MessageBox.Show("将新建Shape文件加入地图失败！");
                return;
            }
        }
        #endregion

        #region 添加要素
        private void miAddFeature_Click(object sender, EventArgs e)
        {
            if (miAddFeature.Checked == false)
            {
                miAddFeature.Checked = true;
            }
            else
            {
                miAddFeature.Checked = false;
            }
        }
        private void mainMapControl_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            //在“添加要素”菜单项被勾选时，进行如下操作
            if (e.button == 1)
            {
                mainMapControl.Pan();       //地图漫游
            }
            else if (e.button == 2)
            {
                if (miAddFeature.Checked == true)
                {
                    //新建point对象，保存鼠标按下位置的坐标信息
                    IPoint point = new PointClass();
                    point.PutCoords(e.mapX, e.mapY);
                    //在新建图层中添加要素，要素的名称统一设置为“观测站”。
                    DataOperator dataOperator = new DataOperator(mainMapControl.Map);
                    dataOperator.AddFeatureToLayer("Observation Stations", "观测站", point);
                    return;
                }
            }
        }
        #endregion

        #region 空间查询
        private void miSpatialFilter_Click(object sender, EventArgs e)
        {
            MapAnalysis mapAnalysis = new MapAnalysis();
            if (mapAnalysis.QueryIntersect("World Cities",
                "Continents",
                mainMapControl.Map,
                esriSpatialRelationEnum.esriSpatialRelationIntersection) == true)
            {
                IActiveView activeView;
                activeView = mainMapControl.ActiveView;
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, 0, mainMapControl.Extent);
                MessageBox.Show("空间查询成功", "查询结果", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }

        }
        #endregion

        #region 缓冲区分析
        private void miBuffer_Click(object sender, EventArgs e)
        {
            MapAnalysis mapAnalysis = new MapAnalysis();
            if (mapAnalysis.Buffer("World Cities", "CITY_NAME='Beijing'", 1.0, mainMapControl.Map) == true)
            {
                IActiveView activeView = mainMapControl.ActiveView;
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, 0, mainMapControl.Extent);
                MessageBox.Show("缓冲区分析成功", "分析结果", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 要素统计
        private void miStatistic_Click(object sender, EventArgs e)
        {
            MapAnalysis mapAnalysis = new MapAnalysis();
            string sMsg;
            sMsg = mapAnalysis.Statistic("Continents", "sQMI", mainMapControl.Map);
            MessageBox.Show(sMsg, "统计结果");
        }
        #endregion

        #region 创建栅格数据集
        private void miCreateRaster_Click(object sender, EventArgs e)
        {
            createRaster Raster = new createRaster();
            Raster.ShowDialog();
        }
        #endregion

        #region 栅格数据格式转换
        private void miRasterConvert_Click(object sender, EventArgs e)
        {
            RasterCon conRast = new RasterCon();
            conRast.ShowDialog();
        }
        #endregion

        #region 栅格影像镶嵌
        private void miRasterMosaic_Click(object sender, EventArgs e)
        {
            RasterUtil rasUtil = new RasterUtil();
            if (rasUtil.Mosaic(@"L:\Raster\RasterCatalog.mdb", "RasterCatalog", @"L:\Raster", "MosaicRaster.tif") == true)
            {
                MessageBox.Show("镶嵌成功", "信息", 0, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 栅格统计
        private void miRasterStatistic_Click(object sender, EventArgs e)
        {
            RasterUtil rasterUtil=new RasterUtil();
            string statistic = rasterUtil.RasterStistics(@"L:\Raster", "MosaicRaster.tif");
            MessageBox.Show(statistic,"统计信息",0,MessageBoxIcon.Information);
        }
        #endregion
    }
}