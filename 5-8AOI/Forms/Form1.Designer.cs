namespace _5_8AOI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenMxd = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenShapefile = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenAccess = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenRaster = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.miCreateBookmark = new System.Windows.Forms.ToolStripMenuItem();
            this.cbBookmarkList = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.miAccessData = new System.Windows.Forms.ToolStripMenuItem();
            this.miCarto = new System.Windows.Forms.ToolStripMenuItem();
            this.miRenderSimply = new System.Windows.Forms.ToolStripMenuItem();
            this.miGeRendererInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.miData = new System.Windows.Forms.ToolStripMenuItem();
            this.miCreateShapefile = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddFeature = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.miSpatialFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.miBuffer = new System.Windows.Forms.ToolStripMenuItem();
            this.miStatistic = new System.Windows.Forms.ToolStripMenuItem();
            this.miRastermgmt = new System.Windows.Forms.ToolStripMenuItem();
            this.miCreateRaster = new System.Windows.Forms.ToolStripMenuItem();
            this.miRasterConvert = new System.Windows.Forms.ToolStripMenuItem();
            this.miRasterMosaic = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMapControl = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.EagleEyeMapControl = new ESRI.ArcGIS.Controls.AxMapControl();
            this.miRasterStatistic = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainMapControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EagleEyeMapControl)).BeginInit();
            this.SuspendLayout();
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(702, 0);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.miCarto,
            this.miData,
            this.toolStripMenuItem8,
            this.miRastermgmt});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(734, 25);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOpenMxd,
            this.miOpenShapefile,
            this.miOpenAccess,
            this.miOpenRaster});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(68, 21);
            this.toolStripMenuItem1.Text = "加载数据";
            // 
            // miOpenMxd
            // 
            this.miOpenMxd.Name = "miOpenMxd";
            this.miOpenMxd.Size = new System.Drawing.Size(153, 22);
            this.miOpenMxd.Text = "打开地图文档";
            this.miOpenMxd.Click += new System.EventHandler(this.miOpenMxd_Click);
            // 
            // miOpenShapefile
            // 
            this.miOpenShapefile.Name = "miOpenShapefile";
            this.miOpenShapefile.Size = new System.Drawing.Size(153, 22);
            this.miOpenShapefile.Text = "打开Shapefile";
            this.miOpenShapefile.Click += new System.EventHandler(this.miOpenShapefile_Click);
            // 
            // miOpenAccess
            // 
            this.miOpenAccess.Name = "miOpenAccess";
            this.miOpenAccess.Size = new System.Drawing.Size(153, 22);
            this.miOpenAccess.Text = "打开Access";
            this.miOpenAccess.Click += new System.EventHandler(this.miOpenAccess_Click);
            // 
            // miOpenRaster
            // 
            this.miOpenRaster.Name = "miOpenRaster";
            this.miOpenRaster.Size = new System.Drawing.Size(153, 22);
            this.miOpenRaster.Text = "打开栅格数据";
            this.miOpenRaster.Click += new System.EventHandler(this.miOpenRaster_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCreateBookmark,
            this.cbBookmarkList});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(66, 21);
            this.toolStripMenuItem2.Text = "AOI书签";
            // 
            // miCreateBookmark
            // 
            this.miCreateBookmark.Name = "miCreateBookmark";
            this.miCreateBookmark.Size = new System.Drawing.Size(181, 22);
            this.miCreateBookmark.Text = "创建书签";
            this.miCreateBookmark.Click += new System.EventHandler(this.miCreateBookmark_Click);
            // 
            // cbBookmarkList
            // 
            this.cbBookmarkList.Name = "cbBookmarkList";
            this.cbBookmarkList.Size = new System.Drawing.Size(121, 25);
            this.cbBookmarkList.SelectedIndexChanged += new System.EventHandler(this.cbBookmarkList_SelectedIndexChanged);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAccessData});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(68, 21);
            this.toolStripMenuItem3.Text = "空间数据";
            // 
            // miAccessData
            // 
            this.miAccessData.Name = "miAccessData";
            this.miAccessData.Size = new System.Drawing.Size(148, 22);
            this.miAccessData.Text = "访问图层数据";
            this.miAccessData.Click += new System.EventHandler(this.miAccessData_Click);
            // 
            // miCarto
            // 
            this.miCarto.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRenderSimply,
            this.miGeRendererInfo});
            this.miCarto.Name = "miCarto";
            this.miCarto.Size = new System.Drawing.Size(68, 21);
            this.miCarto.Text = "地图表现";
            // 
            // miRenderSimply
            // 
            this.miRenderSimply.Name = "miRenderSimply";
            this.miRenderSimply.Size = new System.Drawing.Size(160, 22);
            this.miRenderSimply.Text = "简单渲染图层";
            this.miRenderSimply.Click += new System.EventHandler(this.miRenderSimply_Click);
            // 
            // miGeRendererInfo
            // 
            this.miGeRendererInfo.Name = "miGeRendererInfo";
            this.miGeRendererInfo.Size = new System.Drawing.Size(160, 22);
            this.miGeRendererInfo.Text = "获取渲染器信息";
            this.miGeRendererInfo.Click += new System.EventHandler(this.miGeRendererInfo_Click);
            // 
            // miData
            // 
            this.miData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCreateShapefile,
            this.miAddFeature});
            this.miData.Name = "miData";
            this.miData.Size = new System.Drawing.Size(68, 21);
            this.miData.Text = "数据操作";
            // 
            // miCreateShapefile
            // 
            this.miCreateShapefile.Name = "miCreateShapefile";
            this.miCreateShapefile.Size = new System.Drawing.Size(153, 22);
            this.miCreateShapefile.Text = "创建Shapefile";
            this.miCreateShapefile.Click += new System.EventHandler(this.miCreateShapefile_Click);
            // 
            // miAddFeature
            // 
            this.miAddFeature.Enabled = false;
            this.miAddFeature.Name = "miAddFeature";
            this.miAddFeature.Size = new System.Drawing.Size(153, 22);
            this.miAddFeature.Text = "添加要素";
            this.miAddFeature.Click += new System.EventHandler(this.miAddFeature_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSpatialFilter,
            this.miBuffer,
            this.miStatistic});
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(64, 21);
            this.toolStripMenuItem8.Text = "GIS分析";
            // 
            // miSpatialFilter
            // 
            this.miSpatialFilter.Name = "miSpatialFilter";
            this.miSpatialFilter.Size = new System.Drawing.Size(152, 22);
            this.miSpatialFilter.Text = "空间查询";
            this.miSpatialFilter.Click += new System.EventHandler(this.miSpatialFilter_Click);
            // 
            // miBuffer
            // 
            this.miBuffer.Name = "miBuffer";
            this.miBuffer.Size = new System.Drawing.Size(152, 22);
            this.miBuffer.Text = "缓冲区分析";
            this.miBuffer.Click += new System.EventHandler(this.miBuffer_Click);
            // 
            // miStatistic
            // 
            this.miStatistic.Name = "miStatistic";
            this.miStatistic.Size = new System.Drawing.Size(152, 22);
            this.miStatistic.Text = "要素统计";
            this.miStatistic.Click += new System.EventHandler(this.miStatistic_Click);
            // 
            // miRastermgmt
            // 
            this.miRastermgmt.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCreateRaster,
            this.miRasterConvert,
            this.miRasterMosaic,
            this.miRasterStatistic});
            this.miRastermgmt.Name = "miRastermgmt";
            this.miRastermgmt.Size = new System.Drawing.Size(68, 21);
            this.miRastermgmt.Text = "栅格管理";
            // 
            // miCreateRaster
            // 
            this.miCreateRaster.Name = "miCreateRaster";
            this.miCreateRaster.Size = new System.Drawing.Size(160, 22);
            this.miCreateRaster.Text = "创建栅格数据集";
            this.miCreateRaster.Click += new System.EventHandler(this.miCreateRaster_Click);
            // 
            // miRasterConvert
            // 
            this.miRasterConvert.Name = "miRasterConvert";
            this.miRasterConvert.Size = new System.Drawing.Size(160, 22);
            this.miRasterConvert.Text = "格式转换";
            this.miRasterConvert.Click += new System.EventHandler(this.miRasterConvert_Click);
            // 
            // miRasterMosaic
            // 
            this.miRasterMosaic.Name = "miRasterMosaic";
            this.miRasterMosaic.Size = new System.Drawing.Size(160, 22);
            this.miRasterMosaic.Text = "影像镶嵌";
            this.miRasterMosaic.Click += new System.EventHandler(this.miRasterMosaic_Click);
            // 
            // mainMapControl
            // 
            this.mainMapControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mainMapControl.Location = new System.Drawing.Point(253, 59);
            this.mainMapControl.Name = "mainMapControl";
            this.mainMapControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mainMapControl.OcxState")));
            this.mainMapControl.Size = new System.Drawing.Size(481, 524);
            this.mainMapControl.TabIndex = 3;
            this.mainMapControl.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.mainMapControl_OnMouseDown);
            this.mainMapControl.OnExtentUpdated += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnExtentUpdatedEventHandler(this.mainMapControl_OnExtentUpdated);
            this.mainMapControl.OnMapReplaced += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMapReplacedEventHandler(this.mainMapControl_OnMapReplaced);
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 25);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(734, 28);
            this.axToolbarControl1.TabIndex = 5;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.axTOCControl1.Location = new System.Drawing.Point(0, 59);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(256, 360);
            this.axTOCControl1.TabIndex = 2;
            // 
            // EagleEyeMapControl
            // 
            this.EagleEyeMapControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.EagleEyeMapControl.Location = new System.Drawing.Point(0, 419);
            this.EagleEyeMapControl.Name = "EagleEyeMapControl";
            this.EagleEyeMapControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("EagleEyeMapControl.OcxState")));
            this.EagleEyeMapControl.Size = new System.Drawing.Size(256, 164);
            this.EagleEyeMapControl.TabIndex = 6;
            this.EagleEyeMapControl.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.EagleEyeMapControl_OnMouseDown);
            this.EagleEyeMapControl.OnMouseUp += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseUpEventHandler(this.EagleEyeMapControl_OnMouseUp);
            this.EagleEyeMapControl.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.EagleEyeMapControl_OnMouseMove);
            // 
            // miRasterStatistic
            // 
            this.miRasterStatistic.Name = "miRasterStatistic";
            this.miRasterStatistic.Size = new System.Drawing.Size(160, 22);
            this.miRasterStatistic.Text = "栅格统计";
            this.miRasterStatistic.Click += new System.EventHandler(this.miRasterStatistic_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 583);
            this.Controls.Add(this.EagleEyeMapControl);
            this.Controls.Add(this.axTOCControl1);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.mainMapControl);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "地图操作";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainMapControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EagleEyeMapControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private ESRI.ArcGIS.Controls.AxMapControl mainMapControl;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem miCreateBookmark;
        private System.Windows.Forms.ToolStripComboBox cbBookmarkList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem miAccessData;
        private System.Windows.Forms.ToolStripMenuItem miOpenMxd;
        private System.Windows.Forms.ToolStripMenuItem miOpenShapefile;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private System.Windows.Forms.ToolStripMenuItem miCarto;
        private System.Windows.Forms.ToolStripMenuItem miRenderSimply;
        private System.Windows.Forms.ToolStripMenuItem miGeRendererInfo;
        private System.Windows.Forms.ToolStripMenuItem miData;
        private System.Windows.Forms.ToolStripMenuItem miCreateShapefile;
        private System.Windows.Forms.ToolStripMenuItem miOpenAccess;
        private System.Windows.Forms.ToolStripMenuItem miAddFeature;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private ESRI.ArcGIS.Controls.AxMapControl EagleEyeMapControl;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem miSpatialFilter;
        private System.Windows.Forms.ToolStripMenuItem miBuffer;
        private System.Windows.Forms.ToolStripMenuItem miStatistic;
        private System.Windows.Forms.ToolStripMenuItem miRastermgmt;
        private System.Windows.Forms.ToolStripMenuItem miCreateRaster;
        private System.Windows.Forms.ToolStripMenuItem miOpenRaster;
        private System.Windows.Forms.ToolStripMenuItem miRasterConvert;
        private System.Windows.Forms.ToolStripMenuItem miRasterMosaic;
        private System.Windows.Forms.ToolStripMenuItem miRasterStatistic;
    }
}

