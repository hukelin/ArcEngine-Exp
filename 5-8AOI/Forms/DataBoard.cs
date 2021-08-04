using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _5_8AOI
{
    public partial class DataBoard : Form
    {
        public DataBoard(string sDataName, DataTable dataTable)
        {
            InitializeComponent();
            tbDataName.Name = sDataName;
            dataGridView.DataSource = dataTable;
        }
    }
}
