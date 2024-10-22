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
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Main");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Main");
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.checkBoxDebugMode = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.radioBtnCreateLineMode = new System.Windows.Forms.RadioButton();
            this.radioBtnEditLineMode = new System.Windows.Forms.RadioButton();
            this.checkBoxLockInToLineEnds = new System.Windows.Forms.CheckBox();
            this.selectedLinesTreeView = new System.Windows.Forms.TreeView();
            this.tabFile = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnReloadFile = new System.Windows.Forms.Button();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.btnSaveFile = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.fileLocationLabel = new System.Windows.Forms.Label();
            this.fileLocationBox = new System.Windows.Forms.TextBox();
            this.tabCanvas = new System.Windows.Forms.TabPage();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnCancelLineManipulation = new System.Windows.Forms.Button();
            this.selectedNodeLabel2 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnDraw = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
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
            this.linesTreeView = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabFile.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabCanvas.SuspendLayout();
            this.panel8.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lineKeyBox)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Canvas.BackColor = System.Drawing.Color.White;
            this.Canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Canvas.Location = new System.Drawing.Point(181, 3);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(700, 409);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.lineCanvas_Paint);
            this.Canvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseClick);
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.checkBoxDebugMode);
            this.panel5.Location = new System.Drawing.Point(3, 189);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(166, 23);
            this.panel5.TabIndex = 18;
            // 
            // checkBoxDebugMode
            // 
            this.checkBoxDebugMode.AutoSize = true;
            this.checkBoxDebugMode.Location = new System.Drawing.Point(3, 3);
            this.checkBoxDebugMode.Name = "checkBoxDebugMode";
            this.checkBoxDebugMode.Size = new System.Drawing.Size(88, 17);
            this.checkBoxDebugMode.TabIndex = 12;
            this.checkBoxDebugMode.Text = "Debug Mode";
            this.checkBoxDebugMode.UseVisualStyleBackColor = true;
            this.checkBoxDebugMode.CheckedChanged += new System.EventHandler(this.checkBoxLabelLines_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.radioBtnCreateLineMode);
            this.panel3.Controls.Add(this.radioBtnEditLineMode);
            this.panel3.Controls.Add(this.checkBoxLockInToLineEnds);
            this.panel3.Location = new System.Drawing.Point(3, 214);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(166, 70);
            this.panel3.TabIndex = 13;
            // 
            // radioBtnCreateLineMode
            // 
            this.radioBtnCreateLineMode.AutoSize = true;
            this.radioBtnCreateLineMode.Checked = true;
            this.radioBtnCreateLineMode.Location = new System.Drawing.Point(3, 49);
            this.radioBtnCreateLineMode.Name = "radioBtnCreateLineMode";
            this.radioBtnCreateLineMode.Size = new System.Drawing.Size(109, 17);
            this.radioBtnCreateLineMode.TabIndex = 17;
            this.radioBtnCreateLineMode.TabStop = true;
            this.radioBtnCreateLineMode.Text = "Create Line Mode";
            this.radioBtnCreateLineMode.UseVisualStyleBackColor = true;
            this.radioBtnCreateLineMode.CheckedChanged += new System.EventHandler(this.radioBtnCreateLineMode_CheckedChanged);
            // 
            // radioBtnEditLineMode
            // 
            this.radioBtnEditLineMode.AutoSize = true;
            this.radioBtnEditLineMode.Location = new System.Drawing.Point(3, 26);
            this.radioBtnEditLineMode.Name = "radioBtnEditLineMode";
            this.radioBtnEditLineMode.Size = new System.Drawing.Size(96, 17);
            this.radioBtnEditLineMode.TabIndex = 16;
            this.radioBtnEditLineMode.Text = "Edit Line Mode";
            this.radioBtnEditLineMode.UseVisualStyleBackColor = true;
            this.radioBtnEditLineMode.CheckedChanged += new System.EventHandler(this.radioBtnEditLineMode_CheckedChanged);
            // 
            // checkBoxLockInToLineEnds
            // 
            this.checkBoxLockInToLineEnds.AutoSize = true;
            this.checkBoxLockInToLineEnds.Checked = true;
            this.checkBoxLockInToLineEnds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLockInToLineEnds.Location = new System.Drawing.Point(3, 3);
            this.checkBoxLockInToLineEnds.Name = "checkBoxLockInToLineEnds";
            this.checkBoxLockInToLineEnds.Size = new System.Drawing.Size(128, 17);
            this.checkBoxLockInToLineEnds.TabIndex = 13;
            this.checkBoxLockInToLineEnds.Text = "Lock In To Line Ends";
            this.checkBoxLockInToLineEnds.UseVisualStyleBackColor = true;
            this.checkBoxLockInToLineEnds.CheckedChanged += new System.EventHandler(this.checkBoxLockInToLineEnds_CheckedChanged);
            // 
            // selectedLinesTreeView
            // 
            this.selectedLinesTreeView.Location = new System.Drawing.Point(3, 3);
            this.selectedLinesTreeView.Name = "selectedLinesTreeView";
            treeNode11.Name = "Node0";
            treeNode11.Tag = "main";
            treeNode11.Text = "Main";
            this.selectedLinesTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode11});
            this.selectedLinesTreeView.Size = new System.Drawing.Size(166, 126);
            this.selectedLinesTreeView.TabIndex = 1;
            this.selectedLinesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.selectedLinesTreeView_AfterSelect);
            // 
            // tabFile
            // 
            this.tabFile.BackColor = System.Drawing.Color.White;
            this.tabFile.Controls.Add(this.panel6);
            this.tabFile.Controls.Add(this.panel4);
            this.tabFile.Location = new System.Drawing.Point(4, 22);
            this.tabFile.Name = "tabFile";
            this.tabFile.Size = new System.Drawing.Size(172, 387);
            this.tabFile.TabIndex = 2;
            this.tabFile.Text = "File";
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.btnReloadFile);
            this.panel6.Controls.Add(this.btnOpenFile);
            this.panel6.Controls.Add(this.btnSaveFile);
            this.panel6.Location = new System.Drawing.Point(3, 51);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(166, 62);
            this.panel6.TabIndex = 14;
            // 
            // btnReloadFile
            // 
            this.btnReloadFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReloadFile.Location = new System.Drawing.Point(45, 32);
            this.btnReloadFile.Name = "btnReloadFile";
            this.btnReloadFile.Size = new System.Drawing.Size(70, 23);
            this.btnReloadFile.TabIndex = 4;
            this.btnReloadFile.Text = "Reload File";
            this.btnReloadFile.UseVisualStyleBackColor = true;
            this.btnReloadFile.Click += new System.EventHandler(this.btnReloadFile_Click);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFile.Location = new System.Drawing.Point(85, 3);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(70, 23);
            this.btnOpenFile.TabIndex = 3;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // btnSaveFile
            // 
            this.btnSaveFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveFile.Location = new System.Drawing.Point(9, 3);
            this.btnSaveFile.Name = "btnSaveFile";
            this.btnSaveFile.Size = new System.Drawing.Size(70, 23);
            this.btnSaveFile.TabIndex = 2;
            this.btnSaveFile.Text = "Save File";
            this.btnSaveFile.UseVisualStyleBackColor = true;
            this.btnSaveFile.Click += new System.EventHandler(this.btnSaveFile_Click);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.fileLocationLabel);
            this.panel4.Controls.Add(this.fileLocationBox);
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(166, 46);
            this.panel4.TabIndex = 13;
            // 
            // fileLocationLabel
            // 
            this.fileLocationLabel.AutoSize = true;
            this.fileLocationLabel.Location = new System.Drawing.Point(3, 5);
            this.fileLocationLabel.Name = "fileLocationLabel";
            this.fileLocationLabel.Size = new System.Drawing.Size(70, 13);
            this.fileLocationLabel.TabIndex = 1;
            this.fileLocationLabel.Text = "File Location:";
            // 
            // fileLocationBox
            // 
            this.fileLocationBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileLocationBox.Location = new System.Drawing.Point(3, 21);
            this.fileLocationBox.Name = "fileLocationBox";
            this.fileLocationBox.ReadOnly = true;
            this.fileLocationBox.Size = new System.Drawing.Size(158, 20);
            this.fileLocationBox.TabIndex = 0;
            // 
            // tabCanvas
            // 
            this.tabCanvas.BackColor = System.Drawing.Color.White;
            this.tabCanvas.Controls.Add(this.panel8);
            this.tabCanvas.Controls.Add(this.selectedLinesTreeView);
            this.tabCanvas.Controls.Add(this.panel5);
            this.tabCanvas.Controls.Add(this.panel3);
            this.tabCanvas.Location = new System.Drawing.Point(4, 22);
            this.tabCanvas.Name = "tabCanvas";
            this.tabCanvas.Padding = new System.Windows.Forms.Padding(3);
            this.tabCanvas.Size = new System.Drawing.Size(172, 387);
            this.tabCanvas.TabIndex = 0;
            this.tabCanvas.Text = "Canvas";
            // 
            // panel8
            // 
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.btnCancelLineManipulation);
            this.panel8.Controls.Add(this.selectedNodeLabel2);
            this.panel8.Location = new System.Drawing.Point(3, 135);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(166, 48);
            this.panel8.TabIndex = 19;
            // 
            // btnCancelLineManipulation
            // 
            this.btnCancelLineManipulation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelLineManipulation.Location = new System.Drawing.Point(3, 19);
            this.btnCancelLineManipulation.Name = "btnCancelLineManipulation";
            this.btnCancelLineManipulation.Size = new System.Drawing.Size(158, 23);
            this.btnCancelLineManipulation.TabIndex = 12;
            this.btnCancelLineManipulation.Text = "Cancel";
            this.btnCancelLineManipulation.UseVisualStyleBackColor = true;
            this.btnCancelLineManipulation.Click += new System.EventHandler(this.btnCancelLineManipulation_Click);
            // 
            // selectedNodeLabel2
            // 
            this.selectedNodeLabel2.AutoSize = true;
            this.selectedNodeLabel2.Location = new System.Drawing.Point(3, 3);
            this.selectedNodeLabel2.Name = "selectedNodeLabel2";
            this.selectedNodeLabel2.Size = new System.Drawing.Size(94, 13);
            this.selectedNodeLabel2.TabIndex = 13;
            this.selectedNodeLabel2.Text = "Selected node: [0]";
            // 
            // tabControl
            // 
            this.tabControl.BackColor = System.Drawing.Color.White;
            this.tabControl.Controls.Add(this.panel2);
            this.tabControl.Controls.Add(this.panel1);
            this.tabControl.Controls.Add(this.linesTreeView);
            this.tabControl.Location = new System.Drawing.Point(4, 22);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Windows.Forms.Padding(3);
            this.tabControl.Size = new System.Drawing.Size(172, 387);
            this.tabControl.TabIndex = 1;
            this.tabControl.Text = "Control";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnCreate);
            this.panel2.Controls.Add(this.btnDraw);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnRemove);
            this.panel2.Location = new System.Drawing.Point(3, 86);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(166, 55);
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
            this.panel1.Size = new System.Drawing.Size(166, 81);
            this.panel1.TabIndex = 10;
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
            this.selectedNodeLabel.Location = new System.Drawing.Point(67, 58);
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
            // linesTreeView
            // 
            this.linesTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.linesTreeView.Location = new System.Drawing.Point(3, 143);
            this.linesTreeView.Name = "linesTreeView";
            treeNode12.Name = "Node0";
            treeNode12.Tag = "main";
            treeNode12.Text = "Main";
            treeNode12.ToolTipText = "Root";
            this.linesTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode12});
            this.linesTreeView.Size = new System.Drawing.Size(166, 240);
            this.linesTreeView.TabIndex = 0;
            this.linesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.linesTreeView_AfterSelect);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabControl);
            this.tabControl1.Controls.Add(this.tabCanvas);
            this.tabControl1.Controls.Add(this.tabFile);
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(180, 413);
            this.tabControl1.TabIndex = 0;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 416);
            this.Controls.Add(this.Canvas);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "LineDrawerDemo";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabFile.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabCanvas.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lineKeyBox)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.CheckBox checkBoxDebugMode;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox checkBoxLockInToLineEnds;
        private System.Windows.Forms.RadioButton radioBtnCreateLineMode;
        private System.Windows.Forms.RadioButton radioBtnEditLineMode;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TreeView selectedLinesTreeView;
        private System.Windows.Forms.TabPage tabFile;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnReloadFile;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnSaveFile;
        private System.Windows.Forms.Label fileLocationLabel;
        private System.Windows.Forms.TextBox fileLocationBox;
        private System.Windows.Forms.TabPage tabCanvas;
        private System.Windows.Forms.TabPage tabControl;
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
        private System.Windows.Forms.TreeView linesTreeView;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnCancelLineManipulation;
        private System.Windows.Forms.Label selectedNodeLabel2;
        private System.Windows.Forms.Panel panel8;
    }
}

