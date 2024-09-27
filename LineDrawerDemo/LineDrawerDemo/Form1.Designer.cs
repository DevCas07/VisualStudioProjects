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
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Main");
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabControl = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnDraw = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.x1Label = new System.Windows.Forms.Label();
            this.x2Box = new System.Windows.Forms.TextBox();
            this.y1Box = new System.Windows.Forms.TextBox();
            this.y2Label = new System.Windows.Forms.Label();
            this.y1Label = new System.Windows.Forms.Label();
            this.x2Label = new System.Windows.Forms.Label();
            this.x1Box = new System.Windows.Forms.TextBox();
            this.y2Box = new System.Windows.Forms.TextBox();
            this.linesTreeView = new System.Windows.Forms.TreeView();
            this.tabCanvas = new System.Windows.Forms.TabPage();
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.tabControl1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabCanvas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabControl);
            this.tabControl1.Controls.Add(this.tabCanvas);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(525, 285);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Resize += new System.EventHandler(this.tabControl1_Resize);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.panel2);
            this.tabControl.Controls.Add(this.panel1);
            this.tabControl.Controls.Add(this.linesTreeView);
            this.tabControl.Location = new System.Drawing.Point(4, 22);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Windows.Forms.Padding(3);
            this.tabControl.Size = new System.Drawing.Size(517, 259);
            this.tabControl.TabIndex = 1;
            this.tabControl.Text = "Control";
            this.tabControl.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCreate);
            this.panel2.Controls.Add(this.btnDraw);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnRemove);
            this.panel2.Location = new System.Drawing.Point(3, 59);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(166, 53);
            this.panel2.TabIndex = 11;
            // 
            // btnCreate
            // 
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.Location = new System.Drawing.Point(12, 3);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(68, 23);
            this.btnCreate.TabIndex = 2;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnDraw
            // 
            this.btnDraw.Enabled = false;
            this.btnDraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDraw.Location = new System.Drawing.Point(89, 27);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(68, 23);
            this.btnDraw.TabIndex = 11;
            this.btnDraw.Text = "Draw";
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(89, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.Location = new System.Drawing.Point(12, 27);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(68, 23);
            this.btnRemove.TabIndex = 10;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.x1Label);
            this.panel1.Controls.Add(this.x2Box);
            this.panel1.Controls.Add(this.y1Box);
            this.panel1.Controls.Add(this.y2Label);
            this.panel1.Controls.Add(this.y1Label);
            this.panel1.Controls.Add(this.x2Label);
            this.panel1.Controls.Add(this.x1Box);
            this.panel1.Controls.Add(this.y2Box);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(166, 53);
            this.panel1.TabIndex = 10;
            // 
            // x1Label
            // 
            this.x1Label.AutoSize = true;
            this.x1Label.Location = new System.Drawing.Point(3, 5);
            this.x1Label.Name = "x1Label";
            this.x1Label.Size = new System.Drawing.Size(23, 13);
            this.x1Label.TabIndex = 3;
            this.x1Label.Text = "X1:";
            // 
            // x2Box
            // 
            this.x2Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.x2Box.Location = new System.Drawing.Point(115, 3);
            this.x2Box.Name = "x2Box";
            this.x2Box.Size = new System.Drawing.Size(48, 20);
            this.x2Box.TabIndex = 5;
            // 
            // y1Box
            // 
            this.y1Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.y1Box.Location = new System.Drawing.Point(32, 30);
            this.y1Box.Name = "y1Box";
            this.y1Box.Size = new System.Drawing.Size(48, 20);
            this.y1Box.TabIndex = 1;
            // 
            // y2Label
            // 
            this.y2Label.AutoSize = true;
            this.y2Label.Location = new System.Drawing.Point(86, 32);
            this.y2Label.Name = "y2Label";
            this.y2Label.Size = new System.Drawing.Size(23, 13);
            this.y2Label.TabIndex = 8;
            this.y2Label.Text = "Y2:";
            // 
            // y1Label
            // 
            this.y1Label.AutoSize = true;
            this.y1Label.Location = new System.Drawing.Point(3, 32);
            this.y1Label.Name = "y1Label";
            this.y1Label.Size = new System.Drawing.Size(23, 13);
            this.y1Label.TabIndex = 4;
            this.y1Label.Text = "Y1:";
            // 
            // x2Label
            // 
            this.x2Label.AutoSize = true;
            this.x2Label.Location = new System.Drawing.Point(86, 5);
            this.x2Label.Name = "x2Label";
            this.x2Label.Size = new System.Drawing.Size(23, 13);
            this.x2Label.TabIndex = 7;
            this.x2Label.Text = "X2:";
            // 
            // x1Box
            // 
            this.x1Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.x1Box.Location = new System.Drawing.Point(32, 3);
            this.x1Box.Name = "x1Box";
            this.x1Box.Size = new System.Drawing.Size(48, 20);
            this.x1Box.TabIndex = 0;
            // 
            // y2Box
            // 
            this.y2Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.y2Box.Location = new System.Drawing.Point(115, 30);
            this.y2Box.Name = "y2Box";
            this.y2Box.Size = new System.Drawing.Size(48, 20);
            this.y2Box.TabIndex = 6;
            // 
            // linesTreeView
            // 
            this.linesTreeView.Location = new System.Drawing.Point(172, 3);
            this.linesTreeView.Name = "linesTreeView";
            treeNode2.Name = "Node0";
            treeNode2.Tag = "main";
            treeNode2.Text = "Main";
            treeNode2.ToolTipText = "Root";
            this.linesTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.linesTreeView.Size = new System.Drawing.Size(342, 253);
            this.linesTreeView.TabIndex = 0;
            this.linesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.linesTreeView_AfterSelect);
            // 
            // tabCanvas
            // 
            this.tabCanvas.Controls.Add(this.Canvas);
            this.tabCanvas.Location = new System.Drawing.Point(4, 22);
            this.tabCanvas.Name = "tabCanvas";
            this.tabCanvas.Padding = new System.Windows.Forms.Padding(3);
            this.tabCanvas.Size = new System.Drawing.Size(517, 259);
            this.tabCanvas.TabIndex = 0;
            this.tabCanvas.Text = "Canvas";
            this.tabCanvas.UseVisualStyleBackColor = true;
            // 
            // Canvas
            // 
            this.Canvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Canvas.Location = new System.Drawing.Point(0, 0);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(517, 259);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.lineCanvas_Paint);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 310);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResizeEnd += new System.EventHandler(this.MainWindow_ResizeEnd);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseClick);
            this.tabControl1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabCanvas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabCanvas;
        private System.Windows.Forms.TabPage tabControl;
        private System.Windows.Forms.TreeView linesTreeView;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnDraw;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label x1Label;
        private System.Windows.Forms.TextBox x2Box;
        private System.Windows.Forms.TextBox y1Box;
        private System.Windows.Forms.Label y2Label;
        private System.Windows.Forms.Label y1Label;
        private System.Windows.Forms.Label x2Label;
        private System.Windows.Forms.TextBox x1Box;
        private System.Windows.Forms.TextBox y2Box;
        private System.Windows.Forms.PictureBox Canvas;
    }
}

