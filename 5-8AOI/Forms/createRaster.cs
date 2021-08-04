using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;

namespace _5_8AOI
{
    public partial class createRaster : Form
    {
        public createRaster()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RasterUtil rastUtil = new RasterUtil();
            if (rastUtil.CreateRaster(@"L:\Raster\Raster.gdb", this.textBox1.Text.Trim()) == true
                && this.textBox1.Text.Trim() != "")
            {
                MessageBox.Show("栅格数据集创建成功", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
