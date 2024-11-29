namespace MånghörningarDrawingDemo
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Main");
            this.CanvasBox = new System.Windows.Forms.PictureBox();
            this.pointsTreeView = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.CanvasBox)).BeginInit();
            this.SuspendLayout();
            // 
            // CanvasBox
            // 
            this.CanvasBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CanvasBox.Location = new System.Drawing.Point(12, 12);
            this.CanvasBox.Name = "CanvasBox";
            this.CanvasBox.Size = new System.Drawing.Size(360, 337);
            this.CanvasBox.TabIndex = 0;
            this.CanvasBox.TabStop = false;
            this.CanvasBox.Paint += new System.Windows.Forms.PaintEventHandler(this.CanvasBox_Paint);
            // 
            // pointsTreeView
            // 
            this.pointsTreeView.Location = new System.Drawing.Point(378, 12);
            this.pointsTreeView.Name = "pointsTreeView";
            treeNode1.Name = "Node0";
            treeNode1.Tag = "main";
            treeNode1.Text = "Main";
            this.pointsTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.pointsTreeView.Size = new System.Drawing.Size(174, 337);
            this.pointsTreeView.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 361);
            this.Controls.Add(this.pointsTreeView);
            this.Controls.Add(this.CanvasBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CanvasBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox CanvasBox;
        private System.Windows.Forms.TreeView pointsTreeView;
    }
}

