namespace LineDrawerDemo
{
    partial class MainWindow
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
            this.xCoordBox = new System.Windows.Forms.TextBox();
            this.yCoordBox = new System.Windows.Forms.TextBox();
            this.btnInitiate = new System.Windows.Forms.Button();
            this.xLabel = new System.Windows.Forms.Label();
            this.yLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.lineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xCoordBox
            // 
            this.xCoordBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.xCoordBox.Location = new System.Drawing.Point(27, 10);
            this.xCoordBox.Name = "xCoordBox";
            this.xCoordBox.Size = new System.Drawing.Size(48, 20);
            this.xCoordBox.TabIndex = 0;
            // 
            // yCoordBox
            // 
            this.yCoordBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.yCoordBox.Location = new System.Drawing.Point(27, 37);
            this.yCoordBox.Name = "yCoordBox";
            this.yCoordBox.Size = new System.Drawing.Size(48, 20);
            this.yCoordBox.TabIndex = 1;
            // 
            // btnInitiate
            // 
            this.btnInitiate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInitiate.Location = new System.Drawing.Point(27, 116);
            this.btnInitiate.Name = "btnInitiate";
            this.btnInitiate.Size = new System.Drawing.Size(48, 23);
            this.btnInitiate.TabIndex = 2;
            this.btnInitiate.Text = "Draw";
            this.btnInitiate.UseVisualStyleBackColor = true;
            this.btnInitiate.Click += new System.EventHandler(this.btnInitiate_Click);
            // 
            // xLabel
            // 
            this.xLabel.AutoSize = true;
            this.xLabel.Location = new System.Drawing.Point(4, 12);
            this.xLabel.Name = "xLabel";
            this.xLabel.Size = new System.Drawing.Size(17, 13);
            this.xLabel.TabIndex = 3;
            this.xLabel.Text = "X:";
            // 
            // yLabel
            // 
            this.yLabel.AutoSize = true;
            this.yLabel.Location = new System.Drawing.Point(4, 39);
            this.yLabel.Name = "yLabel";
            this.yLabel.Size = new System.Drawing.Size(17, 13);
            this.yLabel.TabIndex = 4;
            this.yLabel.Text = "Y:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnInitiate);
            this.groupBox1.Controls.Add(this.xCoordBox);
            this.groupBox1.Controls.Add(this.yLabel);
            this.groupBox1.Controls.Add(this.xLabel);
            this.groupBox1.Controls.Add(this.yCoordBox);
            this.groupBox1.Location = new System.Drawing.Point(457, 153);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(81, 145);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lineToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(550, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // lineToolStripMenuItem
            // 
            this.lineToolStripMenuItem.Name = "lineToolStripMenuItem";
            this.lineToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.lineToolStripMenuItem.Text = "line";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 310);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseClick);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox xCoordBox;
        private System.Windows.Forms.TextBox yCoordBox;
        private System.Windows.Forms.Button btnInitiate;
        private System.Windows.Forms.Label xLabel;
        private System.Windows.Forms.Label yLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem lineToolStripMenuItem;
    }
}

