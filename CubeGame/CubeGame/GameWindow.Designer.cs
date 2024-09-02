using System.Windows.Forms;

namespace CubeGame
{
    partial class GameWindow
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
            this.PlayerCubeObject = new System.Windows.Forms.PictureBox();
            this.GameTick = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCubeObject)).BeginInit();
            this.SuspendLayout();
            // 
            // PlayerCubeObject
            // 
            this.PlayerCubeObject.BackColor = System.Drawing.SystemColors.ControlDark;
            this.PlayerCubeObject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlayerCubeObject.Location = new System.Drawing.Point(0, 0);
            this.PlayerCubeObject.Name = "PlayerCubeObject";
            this.PlayerCubeObject.Size = new System.Drawing.Size(50, 50);
            this.PlayerCubeObject.TabIndex = 0;
            this.PlayerCubeObject.TabStop = false;
            // 
            // GameTick
            // 
            this.GameTick.Tick += new System.EventHandler(this.GameTick_Tick);
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 340);
            this.Controls.Add(this.PlayerCubeObject);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "GameWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "GameWindow";
            this.Load += new System.EventHandler(this.GameWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PlayerCubeObject)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer GameTick;
        public System.Windows.Forms.PictureBox PlayerCubeObject;
    }
}