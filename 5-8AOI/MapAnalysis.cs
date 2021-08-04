using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace _5_8AOI
{
    class MapAnalysis
    {
        #region 空间查询
        //空间查询
        public bool QueryIntersect(string scrLayerName,
            string tgtLayerName, IMap iMap,
            esriSpatialRelationEnum spatialRel)
        {
            try
            {
                DataOperator dataOperator = new DataOperator(iMap);
                //定义并根据图层名称获取图层对象
                IFeatureLayer iSrcLayer = dataOperator.GetLayerByName(scrLayerName) as IFeatureLayer;
                IFeatureLayer iTgtLayer = dataOperator.GetLayerByName(tgtLayerName) as IFeatureLayer;
                //通过查询过滤获取Continents层中亚洲的几何
                IGeometry geom;
                IFeature feature;
                IFeatureCursor featureCursor;
                IFeatureClass srcFeatureClass;
                IQueryFilter queryFilter = new QueryFilter();
                queryFilter.WhereClause = "CONTINENT='Asia'";   //设置查询条件
                featureCursor = iTgtLayer.FeatureClass.Search(queryFilter, false);
                feature = featureCursor.NextFeature();
                geom = feature.Shape;   //获取亚洲图形几何
                //根据所选择的几何对城市图层进行属性与空间过滤
                srcFeatureClass = iSrcLayer.FeatureClass;
                ISpatialFilter spatialFilter = new SpatialFilter();
                spatialFilter.Geometry = geom;
                spatialFilter.WhereClause = "POP_RANK=5";
                spatialFilter.SpatialRel = (esriSpatialRelEnum)spatialRel;
                //定义要素选择对象，以要素搜索图层进行实例化
                IFeatureSelection featSelect = iSrcLayer as IFeatureSelection;
                //以空间过滤器对要素进行选择，并建立新选择集
                featSelect.SelectFeatures(spatialFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                return true;
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show(ex.Message,"异常信息");
                return false;
            }
        }
        #endregion

        #region 缓冲区分析
        //增加“缓冲分析”方法
        public bool Buffer(string layerName, string sWhere, double iSize, IMap iMap)
        {
            try
            {
                //根据过滤条件获取所需要素
                DataOperator dataOperator = new DataOperator(iMap);
                IFeatureLayer featLayer = dataOperator.GetLayerByName(layerName) as IFeatureLayer;
                IFeatureClass featClass = featLayer.FeatureClass;
                IQueryFilter queryFilter = new QueryFilter();
                queryFilter.WhereClause = sWhere;   //设置过滤条件
                IFeatureCursor featCursor;
                featCursor = featClass.Search(queryFilter, false) as IFeatureCursor;    //获取查询过滤确定的要素
                int count = featClass.FeatureCount(queryFilter);
                IFeature feature = featCursor.NextFeature();
                IGeometry iGom = feature.Shape;     //获取查询过滤的要素的几何
                //设置空间的缓冲区作为空间查询的几何范围
                ITopologicalOperator ipTO = iGom as ITopologicalOperator;
                IGeometry iGeomBuffer = ipTO.Buffer(iSize);
                //根据缓冲区几何对城市图层进行空间过滤
                ISpatialFilter spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = iGeomBuffer;
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIndexIntersects;
                //定义要素选择对象，以要素搜索图层进行实例化
                IFeatureSelection featSelect = featLayer as IFeatureSelection;
                //以空间过滤器对要素进行选择，并建立新选择集
                featSelect.SelectFeatures(spatialFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                return true;
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show(ex.Message,"异常信息");
                return false;
            }
        }
        #endregion

        #region 要素统计
        //增加“要素统计”方法
        public string Statistic(string layerName,string fieldName,IMap iMap)
        {
            try
            {
                //根据给定图层名称获取图层对象
                DataOperator dataOperator = new DataOperator(iMap);
                IFeatureLayer featLayer = dataOperator.GetLayerByName(layerName) as IFeatureLayer;
                //获取图层的数据统计对象
                IFeatureClass featClass = featLayer.FeatureClass;
                IDataStatistics dataStatistic = new DataStatisticsClass();
                IFeatureCursor featCursor = featClass.Search(null, false);
                ICursor cursor = featCursor as ICursor;
                dataStatistic.Cursor = cursor;
                //指定统计字段为面积字段，统计出最小面积、最大面积及平均面积
                dataStatistic.Field = fieldName;
                IStatisticsResults statResult = dataStatistic.Statistics;   //	统计当前的游标上的当前字段，并把结果赋给统计结果对象
                double dMax = statResult.Maximum;
                double dMin = statResult.Minimum;
                double dMean = statResult.Mean;
                string sResult;
                sResult = "最大面积为" + dMax.ToString()
                    + "\n最小面积为" + dMin.ToString()
                    + "\n平均面积为" + dMean.ToString();
                return sResult;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}
