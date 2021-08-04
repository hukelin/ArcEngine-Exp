using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Carto;

namespace _5_8AOI
{
    class RasterUtil
    {
        #region 打开一个给定文件夹的栅格工作空间
        IRasterWorkspaceEx OpenRasterWorkspaceFromFileGDB(string filePath)
        {
            IWorkspaceFactory wsFactory = new FileGDBWorkspaceFactoryClass();
            IRasterWorkspaceEx ws = wsFactory.OpenFromFile(filePath, 0) as IRasterWorkspaceEx;
            return ws;
        }
        IRasterWorkspace OpenRasterWorkspaceFromFile(string filePath)
        {
            IWorkspaceFactory wsFactory = new RasterWorkspaceFactoryClass();
            IRasterWorkspace ws = wsFactory.OpenFromFile(filePath, 0) as IRasterWorkspace;
            return ws;
        }
        #endregion

        #region 创建栅格数据集
        public bool CreateRaster(string filePath, string rasterName)
        {
            try
            {
                //AccessWorkspaceFactory fac = new AccessWorkspaceFactoryClass();
                //FileGDBWorkspaceFactoryClass pFileGDBWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                //IRasterWorkspaceEx rasterWorkspace = pFileGDBWorkspaceFactory.OpenFromFile(filePath, 0) as IRasterWorkspaceEx;
                IRasterWorkspaceEx rasterWorkspace = this.OpenRasterWorkspaceFromFileGDB(filePath);     //this表示当前类的对象，这里可省略
                IRasterDataset rasterDataset;
                //设置存储参数
                IRasterStorageDef rasterStorageDef = new RasterStorageDefClass();
                rasterStorageDef.CompressionType = esriRasterCompressionType.esriRasterCompressionUncompressed;
                rasterStorageDef.PyramidLevel = 1;
                rasterStorageDef.PyramidResampleType = rstResamplingTypes.RSP_BilinearInterpolation;
                rasterStorageDef.TileHeight = 128;
                rasterStorageDef.TileWidth = 128;
                //提供对控制栅格列定义的成员的访问
                IRasterDef rasterDef = new RasterDefClass();
                //提供对控制空间参考的成员的访问
                ISpatialReferenceFactory2 srFactory = new SpatialReferenceEnvironmentClass();
                int gcsType = (int)esriSRGeoCSType.esriSRGeoCS_WGS1984;
                IGeographicCoordinateSystem geoCoordSystem = srFactory.CreateGeographicCoordinateSystem(gcsType);
                ISpatialReference spatialRef = geoCoordSystem as ISpatialReference;
                rasterDef.SpatialReference = spatialRef;
                rasterDataset = rasterWorkspace.CreateRasterDataset(rasterName, 3, rstPixelType.PT_FLOAT, rasterStorageDef, "DEFAULTS", rasterDef, null);
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "异常信息");
                return false;
            }
        }
        #endregion

        #region 栅格数据格式转换
        public bool RasterConvert(string fileGDB, string oldRasterName, string filePath, string newRasterName)
        {
            try
            {
                IWorkspace workspace;
                IRasterWorkspaceEx rasterWorkspaceEx;
                //打开输入工作空间
                rasterWorkspaceEx = this.OpenRasterWorkspaceFromFileGDB(fileGDB);
                //打开栅格数据集
                IRasterDataset rasterDataset = rasterWorkspaceEx.OpenRasterDataset(oldRasterName);
                //得到栅格波段
                IRasterBandCollection rasterBands = rasterDataset as IRasterBandCollection;
                //打开输出工作空间
                workspace = this.OpenRasterWorkspaceFromFile(filePath) as IWorkspace;
                //另存为给定文件名的图像文件
                rasterBands.SaveAs(newRasterName, workspace, "TIFF");
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "异常信息");
                return false;
            }
        }
        #endregion

        #region 影像镶嵌
        public bool Mosaic(string GDBName, string catalogName, string outputFolder, string outputName)
        {
            try
            {
                //打开个人数据库
                IWorkspaceFactory workspaceGDBFactory = new AccessWorkspaceFactoryClass();
                IWorkspace GDBworkspace = workspaceGDBFactory.OpenFromFile(GDBName, 0);
                //打开要被镶嵌的影像所在的栅格目录
                IRasterWorkspaceEx rasterWorkspaceEX = GDBworkspace as IRasterWorkspaceEx;
                IRasterCatalog rasterCatalog;
                rasterCatalog = rasterWorkspaceEX.OpenRasterCatalog(catalogName);
                //定义一个影像镶嵌对象
                IMosaicRaster mosaicRaster = new MosaicRasterClass();
                //镶嵌栅格目录中的所有影像到一个输出栅格数据集
                mosaicRaster.RasterCatalog = rasterCatalog;
                //设置镶嵌选项
                mosaicRaster.MosaicColormapMode = rstMosaicColormapMode.MM_MATCH;
                mosaicRaster.MosaicOperatorType = rstMosaicOperatorType.MT_LAST;
                //打开输出栅格数据集所在的工作空间
                IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
                IWorkspace workspace = workspaceFactory.OpenFromFile(outputFolder, 0);
                //保存到目标栅格数据集
                ISaveAs saveas = mosaicRaster as ISaveAs;
                saveas.SaveAs(outputName, workspace, "TIFF");
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "异常信息");
                return false;
            }
        }
        #endregion

        #region 栅格统计
        public string RasterStistics(string filePath, string rasterName)
        {
            try
            {
                IWorkspace workspace;
                IRasterWorkspace rasterWorkspace;
                //打开工作空间
                rasterWorkspace = OpenRasterWorkspaceFromFile(filePath) as IRasterWorkspace;
                workspace = rasterWorkspace as IWorkspace;
                //打开栅格数据集
                IRasterDataset rasterDataset = rasterWorkspace.OpenRasterDataset(rasterName);
                //得到栅格波段
                IRasterBandCollection rasterBands = rasterDataset as IRasterBandCollection;
                IEnumRasterBand enumRasterBand = rasterBands.Bands;
                //定义一个字符串记录统计结果
                string sRasterStisticsResult = "栅格统计结果：\n";
                //逐个波段统计，每个波段的均值和标准差
                IRasterBand rasterBand = enumRasterBand.Next();
                while (rasterBand != null)
                {
                    //rasterBand = enumRasterBand.Next();
                    sRasterStisticsResult += this.GetRasterStatistics(rasterBand);
                    rasterBand = enumRasterBand.Next();
                }
                /*IRasterDataset2 rd2 = rasterDataset as IRasterDataset2;
                //创建包含此栅格数据集中所有波段的栅格
                IRaster raster = rd2.CreateFullRaster();
                IRasterBandCollection rbc = (IRasterBandCollection)raster;
                //定义一个字符串记录统计结果
                string statisticsResult = "栅格统计结果：\n";
                //得到栅格波段
                for (int i = 0; i < rbc.Count; i++)
                {
                    IRasterBand rb = rbc.Item(i);
                    statisticsResult += this.GetRasterStatistics(rb);
                }*/
                return sRasterStisticsResult;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private string GetRasterStatistics(IRasterBand rb)
        {
            bool tmpBool;
            rb.HasStatistics(out tmpBool);
            if (!tmpBool)
                rb.ComputeStatsAndHist();
            IRasterHistogram rh = rb.Histogram;
            IRasterStatistics rs = rb.Statistics;
            string statisticsResult = "均值为：" + rs.Mean.ToString() + "\n标准差为：" + rs.StandardDeviation.ToString();
            return statisticsResult;
        }
        #endregion
    }
}

