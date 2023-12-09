namespace detekce
{
    partial class Form1
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_openImg = new System.Windows.Forms.Button();
            this.pbx_result = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_result)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_openImg
            // 
            this.btn_openImg.Location = new System.Drawing.Point(13, 12);
            this.btn_openImg.Name = "btn_openImg";
            this.btn_openImg.Size = new System.Drawing.Size(112, 23);
            this.btn_openImg.TabIndex = 1;
            this.btn_openImg.Text = "Načíst foto";
            this.btn_openImg.UseVisualStyleBackColor = true;
            this.btn_openImg.Click += new System.EventHandler(this.btn_openImg_Click);
            // 
            // pbx_result
            // 
            this.pbx_result.Location = new System.Drawing.Point(13, 41);
            this.pbx_result.Name = "pbx_result";
            this.pbx_result.Size = new System.Drawing.Size(853, 188);
            this.pbx_result.TabIndex = 3;
            this.pbx_result.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 240);
            this.Controls.Add(this.pbx_result);
            this.Controls.Add(this.btn_openImg);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbx_result)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_openImg;
        private System.Windows.Forms.PictureBox pbx_result;
    }
}

