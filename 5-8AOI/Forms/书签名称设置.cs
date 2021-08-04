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
    public partial class AdmitBookmarkName : Form
    {
        public Form1 m_frmMain;
        public AdmitBookmarkName(Form1 frm)
        {
            InitializeComponent();
            this.tbBookmarkName.Focus();
            if (frm != null)
            {
                m_frmMain = frm;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if ((m_frmMain != null) || (tbBookmarkName.Text == ""))
            {
                m_frmMain.CreateBookmark(tbBookmarkName.Text);
            }
            this.Close();
        }
    }
}
