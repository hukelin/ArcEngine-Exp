using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;

namespace _5_8AOI
{
    public partial class RasterCon : Form
    {
        public RasterCon()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            RasterUtil rastUtil = new RasterUtil();
            if (rastUtil.RasterConvert(@"L:\Raster\Raster.gdb", this.textBox1.Text.Trim(), @"L:\Raster", this.textBox2.Text.Trim()) == true
                && this.textBox1.Text.Trim() != "" && this.textBox2.Text.Trim() != "")
            {
                MessageBox.Show("转换完成", "信息", 0, MessageBoxIcon.Information);
            }
        }
    }
}
