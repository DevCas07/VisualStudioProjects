namespace OrbitSimulatorDemo
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Main");
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Control = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timeClock = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.lineKeyBox = new System.Windows.Forms.NumericUpDown();
            this.lineKeyLabel = new System.Windows.Forms.Label();
            this.selectedNodeLabel = new System.Windows.Forms.Label();
            this.x1Label = new System.Windows.Forms.Label();
            this.x2Box = new System.Windows.Forms.TextBox();
            this.y1Box = new System.Windows.Forms.TextBox();
            this.y2Label = new System.Windows.Forms.Label();
            this.y1Label = new System.Windows.Forms.Label();
            this.x2Label = new System.Windows.Forms.Label();
            this.x1Box = new System.Windows.Forms.TextBox();
            this.y2Box = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnDraw = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.linesTreeView = new System.Windows.Forms.TreeView();
            this.tabControl1.SuspendLayout();
            this.Control.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lineKeyBox)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Control);
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(180, 400);
            this.tabControl1.TabIndex = 0;
            // 
            // Control
            // 
            this.Control.Controls.Add(this.linesTreeView);
            this.Control.Controls.Add(this.panel2);
            this.Control.Controls.Add(this.panel1);
            this.Control.Location = new System.Drawing.Point(4, 22);
            this.Control.Name = "Control";
            this.Control.Size = new System.Drawing.Size(172, 374);
            this.Control.TabIndex = 0;
            this.Control.Text = "Control";
            this.Control.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(181, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 400);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // timeClock
            // 
            this.timeClock.Interval = 10;
            this.timeClock.Tick += new System.EventHandler(this.timeClock_Tick);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lineKeyBox);
            this.panel1.Controls.Add(this.lineKeyLabel);
            this.panel1.Controls.Add(this.selectedNodeLabel);
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
            this.panel1.Size = new System.Drawing.Size(166, 99);
            this.panel1.TabIndex = 11;
            // 
            // lineKeyBox
            // 
            this.lineKeyBox.Location = new System.Drawing.Point(30, 56);
            this.lineKeyBox.Name = "lineKeyBox";
            this.lineKeyBox.Size = new System.Drawing.Size(34, 20);
            this.lineKeyBox.TabIndex = 12;
            // 
            // lineKeyLabel
            // 
            this.lineKeyLabel.AutoSize = true;
            this.lineKeyLabel.Location = new System.Drawing.Point(3, 58);
            this.lineKeyLabel.Name = "lineKeyLabel";
            this.lineKeyLabel.Size = new System.Drawing.Size(28, 13);
            this.lineKeyLabel.TabIndex = 11;
            this.lineKeyLabel.Text = "Key:";
            // 
            // selectedNodeLabel
            // 
            this.selectedNodeLabel.AutoSize = true;
            this.selectedNodeLabel.Location = new System.Drawing.Point(3, 79);
            this.selectedNodeLabel.Name = "selectedNodeLabel";
            this.selectedNodeLabel.Size = new System.Drawing.Size(94, 13);
            this.selectedNodeLabel.TabIndex = 9;
            this.selectedNodeLabel.Text = "Selected node: [0]";
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
            this.x2Box.Location = new System.Drawing.Point(113, 3);
            this.x2Box.Name = "x2Box";
            this.x2Box.Size = new System.Drawing.Size(48, 20);
            this.x2Box.TabIndex = 5;
            // 
            // y1Box
            // 
            this.y1Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.y1Box.Location = new System.Drawing.Point(30, 30);
            this.y1Box.Name = "y1Box";
            this.y1Box.Size = new System.Drawing.Size(48, 20);
            this.y1Box.TabIndex = 1;
            // 
            // y2Label
            // 
            this.y2Label.AutoSize = true;
            this.y2Label.Location = new System.Drawing.Point(84, 32);
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
            this.x2Label.Location = new System.Drawing.Point(84, 5);
            this.x2Label.Name = "x2Label";
            this.x2Label.Size = new System.Drawing.Size(23, 13);
            this.x2Label.TabIndex = 7;
            this.x2Label.Text = "X2:";
            // 
            // x1Box
            // 
            this.x1Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.x1Box.Location = new System.Drawing.Point(30, 3);
            this.x1Box.Name = "x1Box";
            this.x1Box.Size = new System.Drawing.Size(48, 20);
            this.x1Box.TabIndex = 0;
            // 
            // y2Box
            // 
            this.y2Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.y2Box.Location = new System.Drawing.Point(113, 30);
            this.y2Box.Name = "y2Box";
            this.y2Box.Size = new System.Drawing.Size(48, 20);
            this.y2Box.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnCreate);
            this.panel2.Controls.Add(this.btnDraw);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnRemove);
            this.panel2.Location = new System.Drawing.Point(3, 108);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(166, 55);
            this.panel2.TabIndex = 13;
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
            // 
            // btnDraw
            // 
            this.btnDraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDraw.Location = new System.Drawing.Point(89, 27);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(68, 23);
            this.btnDraw.TabIndex = 11;
            this.btnDraw.Text = "Draw";
            this.btnDraw.UseVisualStyleBackColor = true;
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
            // 
            // linesTreeView
            // 
            this.linesTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.linesTreeView.Location = new System.Drawing.Point(3, 169);
            this.linesTreeView.Name = "linesTreeView";
            treeNode3.Name = "Node0";
            treeNode3.Tag = "main";
            treeNode3.Text = "Main";
            treeNode3.ToolTipText = "Root";
            this.linesTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.linesTreeView.Size = new System.Drawing.Size(166, 205);
            this.linesTreeView.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 404);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.Control.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lineKeyBox)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabPage Control;
        private System.Windows.Forms.Timer timeClock;
        private System.Windows.Forms.TreeView linesTreeView;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnDraw;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown lineKeyBox;
        private System.Windows.Forms.Label lineKeyLabel;
        private System.Windows.Forms.Label selectedNodeLabel;
        private System.Windows.Forms.Label x1Label;
        private System.Windows.Forms.TextBox x2Box;
        private System.Windows.Forms.TextBox y1Box;
        private System.Windows.Forms.Label y2Label;
        private System.Windows.Forms.Label y1Label;
        private System.Windows.Forms.Label x2Label;
        private System.Windows.Forms.TextBox x1Box;
        private System.Windows.Forms.TextBox y2Box;
    }
}

