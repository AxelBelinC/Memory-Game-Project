namespace memory_game.Forms
{
    partial class FormMenu
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
            this.lblName = new System.Windows.Forms.Label();
            this.txtPlayerName = new System.Windows.Forms.TextBox();
            this.lblDiff = new System.Windows.Forms.Label();
            this.cmbDifficulty = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(312, 164);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(66, 24);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            // 
            // txtPlayerName
            // 
            this.txtPlayerName.Location = new System.Drawing.Point(384, 169);
            this.txtPlayerName.Name = "txtPlayerName";
            this.txtPlayerName.Size = new System.Drawing.Size(100, 20);
            this.txtPlayerName.TabIndex = 2;
            // 
            // lblDiff
            // 
            this.lblDiff.AutoSize = true;
            this.lblDiff.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiff.Location = new System.Drawing.Point(296, 244);
            this.lblDiff.Name = "lblDiff";
            this.lblDiff.Size = new System.Drawing.Size(82, 24);
            this.lblDiff.TabIndex = 3;
            this.lblDiff.Text = "Difficulty:";
            // 
            // cmbDifficulty
            // 
            this.cmbDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDifficulty.FormattingEnabled = true;
            this.cmbDifficulty.Location = new System.Drawing.Point(384, 249);
            this.cmbDifficulty.Name = "cmbDifficulty";
            this.cmbDifficulty.Size = new System.Drawing.Size(121, 21);
            this.cmbDifficulty.TabIndex = 4;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(355, 308);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(129, 72);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 349);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "pruebaxd";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::memory_game.Properties.Resources.logo1;
            this.pictureBox1.Location = new System.Drawing.Point(496, 51);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(219, 110);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // FormMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::memory_game.Properties.Resources.Diseño_sin_título;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.cmbDifficulty);
            this.Controls.Add(this.lblDiff);
            this.Controls.Add(this.txtPlayerName);
            this.Controls.Add(this.lblName);
            this.Name = "FormMenu";
            this.Text = "Form1";
            this.Click += new System.EventHandler(this.btnStart_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtPlayerName;
        private System.Windows.Forms.Label lblDiff;
        private System.Windows.Forms.ComboBox cmbDifficulty;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}