namespace _5_8AOI
{
    partial class AdmitBookmarkName
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
            this.tbBookmarkName = new System.Windows.Forms.TextBox();
            this.btnAdmit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbBookmarkName
            // 
            this.tbBookmarkName.Location = new System.Drawing.Point(92, 27);
            this.tbBookmarkName.Name = "tbBookmarkName";
            this.tbBookmarkName.Size = new System.Drawing.Size(119, 21);
            this.tbBookmarkName.TabIndex = 0;
            // 
            // btnAdmit
            // 
            this.btnAdmit.Location = new System.Drawing.Point(92, 75);
            this.btnAdmit.Name = "btnAdmit";
            this.btnAdmit.Size = new System.Drawing.Size(75, 23);
            this.btnAdmit.TabIndex = 1;
            this.btnAdmit.Text = "确认";
            this.btnAdmit.UseVisualStyleBackColor = true;
            this.btnAdmit.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "书签名：";
            // 
            // AdmitBookmarkName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 110);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAdmit);
            this.Controls.Add(this.tbBookmarkName);
            this.Name = "AdmitBookmarkName";
            this.Text = "书签名称设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbBookmarkName;
        private System.Windows.Forms.Button btnAdmit;
        private System.Windows.Forms.Label label1;

    }
}