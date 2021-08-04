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

        #region ��*.mxd�ĵ�
        private void miOpenMxd_Click(object sender, EventArgs e)
        {
            OpenFileDialog pOpenFileDialog = new OpenFileDialog();
            pOpenFileDialog.CheckFileExists = true;
            pOpenFileDialog.Title = "�򿪵�ͼ�ĵ�";
            pOpenFileDialog.Filter = "ArcMap Documents(*.mxd)|*.mxd;|ArcMAP ģ��(*.mxt)|(*.mxt);|������ͼ�ļ�(*.pmf)|(*.pmf)|���е�ͼ��ʽ(*.mxd;*.mxt;*.pmf)|(*.mxd;*.mxt;*.pmf)";
            pOpenFileDialog.Multiselect = false;        //���������ļ�ͬʱѡ��
            pOpenFileDialog.RestoreDirectory = true;    //�洢�򿪵��ļ�·�� 
            if (pOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string pFileName = pOpenFileDialog.FileName;
                if (pFileName == "")
                {
                    return;
                }
                if (mainMapControl.CheckMxFile(pFileName))       //����ͼ�ĵ�����Ч��
                {
                    mainMapControl.LoadMxFile(pFileName);
                }
                else
                {
                    MessageBox.Show(pFileName + "����Ч�ĵ�ͼ�ĵ���", "��Ϣ��ʾ");
                    return;
                }
            }
        }
        #endregion

        #region ��*.shp�ļ�
        private void miOpenShapefile_Click(object sender, EventArgs e)
        {
            OpenFileDialog pOpenFileDialog = new OpenFileDialog();
            pOpenFileDialog.CheckFileExists = true;
            pOpenFileDialog.Title = "��Shape�ļ�";
            pOpenFileDialog.Filter = "Shape�ļ�(*.shp)|*.shp";
            pOpenFileDialog.ShowDialog();
            //��ȡ�ļ�·��
            IWorkspaceFactory pWorkspaceFactory;
            IFeatureWorkspace pFeatureWorkspace;
            IFeatureLayer pFeatureLayer;
            string pFullPath = pOpenFileDialog.FileName;
            if (pFullPath == "")
                return;
            int pIndex = pFullPath.LastIndexOf("\\");
            string pFilePath = pFullPath.Substring(0, pIndex);       //�ļ�·��
            string pFileName = pFullPath.Substring(pIndex + 1);     //�ļ�
            //ʵ����ShapefileWorkspaceFactory�����ռ䣬��Shapefile�ļ�
            pWorkspaceFactory = new ShapefileWorkspaceFactory();
            pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(pFilePath, 0);
            //������ʵ����Ҫ�ؼ�
            IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(pFileName);
            pFeatureLayer = new FeatureLayer();
            pFeatureLayer.FeatureClass = pFeatureClass;
            pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
            ClearAllData();
            mainMapControl.Map.AddLayer(pFeatureLayer);
            mainMapControl.ActiveView.Refresh();
            //ͬ��ӥ��
            SynchronizeEagleEye();
        }
        #endregion

        #region ���ع����ռ��е�����
        private void AddAllDataset(IWorkspace pWorkspace, AxMapControl mapControl)
        {
            IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTAny);
            pEnumDataset.Reset();   //�������ݼ����У�ʹָ��λ�ڵ�һ�����ݼ�֮ǰ
            IDataset pDataset = pEnumDataset.Next();    //���ݼ��е�����һ��һ����ȡ
            //�ж����ݼ��Ƿ�������
            while (pDataset != null)
            {
                if (pDataset is IFeatureDataset) //Ҫ�����ݼ�
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
                        if (pDataset1 is IFeatureClass) //Ҫ����
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
                else if (pDataset is IFeatureClass)  //Ҫ����
                {
                    IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                    IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                    pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(pDataset.Name);
                    pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                    mapControl.Map.AddLayer(pFeatureLayer);
                }
                else if (pDataset is IRasterDataset)    //դ�����ݼ�
                {
                    IRasterWorkspaceEx pRasterworkspace = pWorkspace as IRasterWorkspaceEx;
                    IRasterDataset pRasterDataset = pRasterworkspace.OpenRasterDataset(pDataset.Name);
                    //Ӱ��������ж��봴��
                    IRasterPyramid3 pRasPyrmid = pRasterDataset as IRasterPyramid3;
                    if (pRasPyrmid != null)
                    {
                        if (!(pRasPyrmid.Present))  //Indicates whether pyramid layers exist
                        {
                            pRasPyrmid.Create();    //����������
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
            //ͬ��ӥ��
            SynchronizeEagleEye();
        }
        #endregion

        #region ���ظ��˵������ݿ�
        private void miOpenAccess_Click(object sender, EventArgs e)
        {
            OpenFileDialog pOenFileDialog = new OpenFileDialog();
            pOenFileDialog.Filter = "Personal Geodatabase(*.mdb)|*.mdb";
            pOenFileDialog.Title = "��PersonGeodatabase�ļ�";
            pOenFileDialog.ShowDialog();    //����ģʽ����
            string pFullPath = pOenFileDialog.FileName;
            if (pFullPath == "") return;
            IWorkspaceFactory pAccessWorkspaceFactory = new AccessWorkspaceFactory();
            IWorkspace pWorkspace = pAccessWorkspaceFactory.OpenFromFile(pFullPath, 0);
            //���ع����ռ��������
            ClearAllData();     //ɾ�������Ѽ��ص�����
            AddAllDataset(pWorkspace, mainMapControl);
        }
        #endregion

        #region ��Raster����
        private void miOpenRaster_Click(object sender, EventArgs e)
        {
            OpenFileDialog pOpenFileDialog = new OpenFileDialog();      //��ʼ��һ��OpenFileDialog���ʵ��
            pOpenFileDialog.CheckFileExists = true;         //CheckFileExists���Ի�ȡ������һ��ֵ�������ֵָʾ�û�ָ�������ڵ��ļ����Ի����Ƿ���ʾ����
            pOpenFileDialog.Title = "��Raster�ļ�";        //Title���Ի�ȡ�������ļ��Ի������
            pOpenFileDialog.Filter = "դ���ļ�(*.*)|*.bmp;*.tif;*.jpg;*.img|(*.bmp)|*.bmp|(*.tif)|*.tif|(*.jpg)|*.jpg|(*.img)|*.img";   //Filter���Ի�ȡ�����õ�ǰ�ļ���ɸѡ���ַ��������ַ��������Ի���ġ����Ϊ�ļ��������ļ��������г��ֵ�ѡ������
            pOpenFileDialog.ShowDialog();   //ShowDialog()������Ĭ�ϵ�����������ͨ�öԻ���
            string pRasterFilename = pOpenFileDialog.FileName;  //string����һ���ַ�������pRasterFilename��FileName���Ի�ȡ������һ���������ļ��Ի�����ѡ�����ļ������ַ���
            if (pRasterFilename == "") return;
            string pPath = System.IO.Path.GetDirectoryName(pRasterFilename);    //����ָ��·���ַ�����Ŀ¼��Ϣ
            string pFileName = System.IO.Path.GetFileName(pRasterFilename);     //����ָ��·���ַ������ļ�������չ��
            IWorkspaceFactory pWorkspaceFactory = new RasterWorkspaceFactory();     //ʵ����դ���ļ������ռ�     
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(pPath, 0);       //Workspace�����ͨ��WorkspaceFactory���OpenFormFile����������
            IRasterWorkspace pRasterWorkspace = pWorkspace as IRasterWorkspace;     //��pWorkspace�����ռ�ת����IRasterWorkspace�ӿ�����
            IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset(pFileName);      //��IRasterWorkspace�ӿڵ�OpenRasterDataset�������դ���ļ������ݼ�����ʵ����դ���ļ�����
            //Ӱ��������ж��봴��(ע��Ӱ�����������ԭʼӰ��һ���������ɵ���ϸ���ֲ�ͬ�ֱ��ʵ�Ӱ�񼯡��������ĵײ���ͼ��ĸ߷ֱ��ʱ�ʾ��Ҳ����ԭʼͼ�񣬶������ǵͷֱ��ʵĽ��ơ���ײ�ķֱ�����ߣ�����������������Ų��������ӣ���ֱ����𽥽��ͣ�������Ҳ���������١�)
            IRasterPyramid3 pRasPyrmid;     //����pRasPyrmid�������ͣ�Pyramid����������
            pRasPyrmid = pRasterDataset as IRasterPyramid3;     //��pRasterDataset�ļ�����ת����IRasterPyramid3�ӿ�����
            //��դ�����ݼ��Ƿ���н����������ж�
            if (pRasPyrmid != null)
            {
                if (!(pRasPyrmid.Present))   //դ���ļ������н������򴴽�������
                {
                    pRasPyrmid.Create();    //����������
                }
            }
            IRaster pRaster;
            pRaster = pRasterDataset.CreateDefaultRaster();
            IRasterLayer pRasterLayer;
            pRasterLayer = new RasterLayerClass();
            pRasterLayer.CreateFromRaster(pRaster);     //Ϊһ��դ����󴴽�һ��ͼ��
            ILayer pLayer = pRasterLayer as ILayer;
            mainMapControl.AddLayer(pLayer, 0);
            //ͬ��ӥ��
            SynchronizeEagleEye();
        }
        #endregion

        #region ɾ�������Ѽ��ص�����
        private void ClearAllData()
        {
            if (mainMapControl.Map != null && mainMapControl.Map.LayerCount > 0)
            {
                //�½�mainMapControl��Map
                IMap dataMap = new MapClass();
                dataMap.Name = "Map";
                mainMapControl.DocumentFilename = string.Empty;
                mainMapControl.Map = dataMap;

                //�½�EagleEyeMapControl��Map
                IMap eagleEyeMap = new MapClass();
                eagleEyeMap.Name = "eagleEyeMap";
                EagleEyeMapControl.DocumentFilename = string.Empty;
                EagleEyeMapControl.Map = eagleEyeMap;
            }
        }
        #endregion

        #region ӥ�۵�ʵ�ּ�ͬ��
        //ӥ��ͬ��
        private bool bCanDrag;              //ӥ�۵�ͼ�ϵľ��ο���ƶ��ı�־
        private IPoint pMoveRectPoint;      //��¼���ƶ�ӥ�۵�ͼ�ϵľ��ο�ʱ����λ��
        private IEnvelope pEnv;             //��¼������ͼ��Extent
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
            //����ӥ�ۺ�����ͼ������ϵͳһ��
            EagleEyeMapControl.SpatialReference = mainMapControl.SpatialReference;
            for (int i = mainMapControl.LayerCount - 1; i >= 0; i--)
            {
                //ʹӥ����ͼ��������ͼ��ͼ������˳�򱣳�һ��
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
                            //����ӥ�۵�ͼ��С�����Թ��˵�ͼ�㲻���
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
                //����ӥ�۵�ͼȫͼ��ʾ  
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
                //�����������ƶ����ο�
                if (e.button == 1)
                {
                    //���ָ������ӥ�۵ľ��ο��У���ǿ��ƶ�
                    if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
                    {
                        bCanDrag = true;
                    }
                    pMoveRectPoint = new PointClass();
                    pMoveRectPoint.PutCoords(e.mapX, e.mapY);  //��¼����ĵ�һ���������
                }
                //��������Ҽ����ƾ��ο�
                else if (e.button == 2)
                {
                    IEnvelope pEnvelope = EagleEyeMapControl.TrackRectangle();

                    IPoint pTempPoint = new PointClass();
                    pTempPoint.PutCoords(pEnvelope.XMin + pEnvelope.Width / 2, pEnvelope.YMin + pEnvelope.Height / 2);
                    mainMapControl.Extent = pEnvelope;
                    //���ο�ĸ߿��������ͼ�ĸ߿�һ�������ȣ�������һ�����ĵ���
                    mainMapControl.CenterAt(pTempPoint);
                }
            }
        }

        //�ƶ����ο�
        private void EagleEyeMapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (e.mapX > pEnv.XMin && e.mapY > pEnv.YMin && e.mapX < pEnv.XMax && e.mapY < pEnv.YMax)
            {
                //�������ƶ������ο��У���껻��С�֣���ʾ�����϶�
                EagleEyeMapControl.MousePointer = esriControlsMousePointer.esriPointerHand;
                if (e.button == 2)  //������ڲ���������Ҽ����������ʾ����ΪĬ����ʽ
                {
                    EagleEyeMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
                }
            }
            else
            {
                //������λ�ý������ΪĬ�ϵ���ʽ
                EagleEyeMapControl.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }

            if (bCanDrag)
            {
                double Dx, Dy;  //��¼����ƶ��ľ���
                Dx = e.mapX - pMoveRectPoint.X;
                Dy = e.mapY - pMoveRectPoint.Y;
                pEnv.Offset(Dx, Dy); //����ƫ�������� pEnv λ��
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

        //���ƾ��ο�
        private void mainMapControl_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            //�õ���ǰ��ͼ��Χ
            pEnv = (IEnvelope)e.newEnvelope;
            DrawRectangle(pEnv);
        }

        //��ӥ�۵�ͼ���滭���ο�
        private void DrawRectangle(IEnvelope pEnvelope)
        {
            //�ڻ���ǰ�����ӥ����֮ǰ���Ƶľ��ο�
            IGraphicsContainer pGraphicsContainer = EagleEyeMapControl.Map as IGraphicsContainer;
            IActiveView pActiveView = pGraphicsContainer as IActiveView;
            pGraphicsContainer.DeleteAllElements();
            //�õ���ǰ��ͼ��Χ
            IRectangleElement pRectangleElement = new RectangleElementClass();
            IElement pElement = pRectangleElement as IElement;
            pElement.Geometry = pEnvelope;
            //���þ��ο�ʵ��Ϊ�м�͸�����棩
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
            //��ӥ������Ӿ��ο�
            IFillShapeElement pFillShapeElement = pElement as IFillShapeElement;
            pFillShapeElement.Symbol = pFillSymbol;
            pGraphicsContainer.AddElement((IElement)pFillShapeElement, 0);
            //ˢ��
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        #endregion

        #region AOI��ǩ
        //����AOI��ǩ
        public void CreateBookmark(string sBookmarkName)
        {
            //��ȡ��ǩ
            IAOIBookmark aoibookmark = new AOIBookmarkClass();
            if (aoibookmark != null)
            {
                aoibookmark.Location = mainMapControl.ActiveView.Extent;
                aoibookmark.Name = sBookmarkName;
            }
            //�ڵ�ͼ��������ǩ
            IMapBookmarks bookmarks = mainMapControl.Map as IMapBookmarks;
            if (bookmarks != null)
            {
                bookmarks.AddBookmark(aoibookmark);
            }
            cbBookmarkList.Items.Add(aoibookmark.Name);
        }
        //������ǩ��
        private void miCreateBookmark_Click(object sender, EventArgs e)
        {
            AdmitBookmarkName frmABN = new AdmitBookmarkName(this);
            frmABN.ShowDialog();
        }

        //������ǩ����
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
            this.WindowState = FormWindowState.Maximized;       //���ô������
            mainMapControl.Map.Name = "Layer";      //�޸�ͼ����
        }
        #endregion

        #region ����ͼ������
        private void miAccessData_Click(object sender, EventArgs e)
        {
            DataOperator dataOperator = new DataOperator(mainMapControl.Map);
            DataBoard dataBoard = new DataBoard("����������", dataOperator.GetContinentsNames());
            dataBoard.Show();
        }
        #endregion

        #region ��ͼ��Ⱦ
        //Ϊ������ġ�����Ⱦͼ�㡱�˵������ɡ�������¼���Ӧ����������Ӵ���ʵ�ֶԡ�World Cirtis��ͼ��ļ���Ⱦ��
        private void miRenderSimply_Click(object sender, EventArgs e)
        {
            //��ȡ��World Cities��ͼ��
            DataOperator dataOperator = new DataOperator(mainMapControl.Map);
            ILayer layer = dataOperator.GetLayerByName("World Cities");
            //ͨ��IRgbColor�ӿ��½�RgbColor���Ͷ��󣬽�������Ϊ��ɫ
            IRgbColor rgbColor = new RgbColorClass();
            rgbColor.Red = 255;
            rgbColor.Green = 0;
            rgbColor.Blue = 0;
            //��ȡ��World Cities��ͼ��ķ�����Ϣ����ͨ��IColor�ӿڷ������úõ���ɫ����
            ISymbol symbol = MapComposer.GetSymbolFromLayer(layer);     //�ж�ͼ�������ͼ�����Ƿ�Ϊ��
            IColor color = rgbColor as IColor;
            //ʵ�֡�World Cities��ͼ��ļ���Ⱦ�����ж��Ƿ�ɹ�
            //����������true����ǰ���ͼˢ�£���ʾ��ȾЧ������ʹ�á�ͼ�����Ⱦ���˵���ٿ���
            //����������false����Ϣ����ʾ������Ⱦͼ��ʧ�ܣ���
            bool bRes = MapComposer.RenderSimply(layer, color);
            if (bRes)
            {
                axTOCControl1.ActiveView.ContentsChanged();
                mainMapControl.ActiveView.Refresh();
                miRenderSimply.Enabled = false;
            }
            else
            {
                MessageBox.Show("����Ⱦͼ��ʧ�ܣ�");
            }
        }

        private void miGeRendererInfo_Click(object sender, EventArgs e)
        {
            DataOperator dataOperator = new DataOperator(mainMapControl.Map);
            ILayer layer = dataOperator.GetLayerByName("World Cities");
            //��Ϣ����ʾ��ͼ�����Ⱦ��������Ϣ
            string RendererType = MapComposer.GetRendererTypeByLayer(layer);
            MessageBox.Show(RendererType);
        }
        #endregion

        #region ����shapefile
        private void miCreateShapefile_Click(object sender, EventArgs e)
        {
            //����Shape�ļ���������Ҫ������ʽ��ȡ
            DataOperator dataOperator = new DataOperator(mainMapControl.Map);
            IFeatureClass featureClass = dataOperator.CreateShapefile("L:\\", "ShpapefileWorkspace", "ShapefileSample");
            if (featureClass == null)
            {
                MessageBox.Show("����Shape�ļ�ʧ�ܣ�");
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
                MessageBox.Show("���½�Shape�ļ������ͼʧ�ܣ�");
                return;
            }
        }
        #endregion

        #region ���Ҫ��
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
            //�ڡ����Ҫ�ء��˵����ѡʱ���������²���
            if (e.button == 1)
            {
                mainMapControl.Pan();       //��ͼ����
            }
            else if (e.button == 2)
            {
                if (miAddFeature.Checked == true)
                {
                    //�½�point���󣬱�����갴��λ�õ�������Ϣ
                    IPoint point = new PointClass();
                    point.PutCoords(e.mapX, e.mapY);
                    //���½�ͼ�������Ҫ�أ�Ҫ�ص�����ͳһ����Ϊ���۲�վ����
                    DataOperator dataOperator = new DataOperator(mainMapControl.Map);
                    dataOperator.AddFeatureToLayer("Observation Stations", "�۲�վ", point);
                    return;
                }
            }
        }
        #endregion

        #region �ռ��ѯ
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
                MessageBox.Show("�ռ��ѯ�ɹ�", "��ѯ���", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }

        }
        #endregion

        #region ����������
        private void miBuffer_Click(object sender, EventArgs e)
        {
            MapAnalysis mapAnalysis = new MapAnalysis();
            if (mapAnalysis.Buffer("World Cities", "CITY_NAME='Beijing'", 1.0, mainMapControl.Map) == true)
            {
                IActiveView activeView = mainMapControl.ActiveView;
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, 0, mainMapControl.Extent);
                MessageBox.Show("�����������ɹ�", "�������", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region Ҫ��ͳ��
        private void miStatistic_Click(object sender, EventArgs e)
        {
            MapAnalysis mapAnalysis = new MapAnalysis();
            string sMsg;
            sMsg = mapAnalysis.Statistic("Continents", "sQMI", mainMapControl.Map);
            MessageBox.Show(sMsg, "ͳ�ƽ��");
        }
        #endregion

        #region ����դ�����ݼ�
        private void miCreateRaster_Click(object sender, EventArgs e)
        {
            createRaster Raster = new createRaster();
            Raster.ShowDialog();
        }
        #endregion

        #region դ�����ݸ�ʽת��
        private void miRasterConvert_Click(object sender, EventArgs e)
        {
            RasterCon conRast = new RasterCon();
            conRast.ShowDialog();
        }
        #endregion

        #region դ��Ӱ����Ƕ
        private void miRasterMosaic_Click(object sender, EventArgs e)
        {
            RasterUtil rasUtil = new RasterUtil();
            if (rasUtil.Mosaic(@"L:\Raster\RasterCatalog.mdb", "RasterCatalog", @"L:\Raster", "MosaicRaster.tif") == true)
            {
                MessageBox.Show("��Ƕ�ɹ�", "��Ϣ", 0, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region դ��ͳ��
        private void miRasterStatistic_Click(object sender, EventArgs e)
        {
            RasterUtil rasterUtil=new RasterUtil();
            string statistic = rasterUtil.RasterStistics(@"L:\Raster", "MosaicRaster.tif");
            MessageBox.Show(statistic,"ͳ����Ϣ",0,MessageBoxIcon.Information);
        }
        #endregion
    }
}