namespace LineDrawerDemo
{
    partial class Form2
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.xCoordBox = new System.Windows.Forms.TextBox();
            this.y1Label = new System.Windows.Forms.Label();
            this.x1Label = new System.Windows.Forms.Label();
            this.yCoordBox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.y2Label = new System.Windows.Forms.Label();
            this.x2Label = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.y2Label);
            this.groupBox1.Controls.Add(this.x2Label);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.xCoordBox);
            this.groupBox1.Controls.Add(this.y1Label);
            this.groupBox1.Controls.Add(this.x1Label);
            this.groupBox1.Controls.Add(this.yCoordBox);
            this.groupBox1.Location = new System.Drawing.Point(23, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(81, 145);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(7, 116);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Confirm";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // xCoordBox
            // 
            this.xCoordBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.xCoordBox.Location = new System.Drawing.Point(27, 10);
            this.xCoordBox.Name = "xCoordBox";
            this.xCoordBox.Size = new System.Drawing.Size(48, 20);
            this.xCoordBox.TabIndex = 0;
            // 
            // y1Label
            // 
            this.y1Label.AutoSize = true;
            this.y1Label.Location = new System.Drawing.Point(4, 39);
            this.y1Label.Name = "y1Label";
            this.y1Label.Size = new System.Drawing.Size(23, 13);
            this.y1Label.TabIndex = 4;
            this.y1Label.Text = "Y1:";
            // 
            // x1Label
            // 
            this.x1Label.AutoSize = true;
            this.x1Label.Location = new System.Drawing.Point(4, 12);
            this.x1Label.Name = "x1Label";
            this.x1Label.Size = new System.Drawing.Size(23, 13);
            this.x1Label.TabIndex = 3;
            this.x1Label.Text = "X1:";
            // 
            // yCoordBox
            // 
            this.yCoordBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.yCoordBox.Location = new System.Drawing.Point(27, 37);
            this.yCoordBox.Name = "yCoordBox";
            this.yCoordBox.Size = new System.Drawing.Size(48, 20);
            this.yCoordBox.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(27, 63);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(48, 20);
            this.textBox1.TabIndex = 5;
            // 
            // y2Label
            // 
            this.y2Label.AutoSize = true;
            this.y2Label.Location = new System.Drawing.Point(4, 92);
            this.y2Label.Name = "y2Label";
            this.y2Label.Size = new System.Drawing.Size(23, 13);
            this.y2Label.TabIndex = 8;
            this.y2Label.Text = "Y2:";
            // 
            // x2Label
            // 
            this.x2Label.AutoSize = true;
            this.x2Label.Location = new System.Drawing.Point(4, 65);
            this.x2Label.Name = "x2Label";
            this.x2Label.Size = new System.Drawing.Size(23, 13);
            this.x2Label.TabIndex = 7;
            this.x2Label.Text = "X2:";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Location = new System.Drawing.Point(27, 90);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(48, 20);
            this.textBox2.TabIndex = 6;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(128, 165);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label y2Label;
        private System.Windows.Forms.Label x2Label;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox xCoordBox;
        private System.Windows.Forms.Label y1Label;
        private System.Windows.Forms.Label x1Label;
        private System.Windows.Forms.TextBox yCoordBox;
    }
}