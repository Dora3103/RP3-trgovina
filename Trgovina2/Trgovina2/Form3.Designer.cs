
namespace Trgovina2
{
    partial class Newproduct
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
            this.addProduct1 = new Trgovina2.addProduct();
            this.SuspendLayout();
            // 
            // addProduct1
            // 
            this.addProduct1.Location = new System.Drawing.Point(37, 36);
            this.addProduct1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.addProduct1.Name = "addProduct1";
            this.addProduct1.Size = new System.Drawing.Size(376, 356);
            this.addProduct1.TabIndex = 0;
            this.addProduct1.dodaj += new System.EventHandler<Trgovina2.proizvod>(this.addProduct1_dodaj);
            // 
            // Newproduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(488, 397);
            this.Controls.Add(this.addProduct1);
            this.Name = "Newproduct";
            this.Text = "Novi proizvod";
            this.ResumeLayout(false);

        }

        #endregion

        private addProduct addProduct1;
    }
}