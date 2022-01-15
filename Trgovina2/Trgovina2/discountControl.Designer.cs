namespace Trgovina2
{
    partial class discountControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.percentTextBox = new System.Windows.Forms.TextBox();
            this.changeButton = new System.Windows.Forms.Button();
            this.chooseComboBox = new System.Windows.Forms.ComboBox();
            this.fromTextBox = new System.Windows.Forms.TextBox();
            this.toTextBox = new System.Windows.Forms.TextBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // percentTextBox
            // 
            this.percentTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.percentTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.percentTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.percentTextBox.Location = new System.Drawing.Point(3, 11);
            this.percentTextBox.Name = "percentTextBox";
            this.percentTextBox.ReadOnly = true;
            this.percentTextBox.Size = new System.Drawing.Size(88, 17);
            this.percentTextBox.TabIndex = 0;
            this.percentTextBox.Text = "Postotak";
            // 
            // changeButton
            // 
            this.changeButton.AutoSize = true;
            this.changeButton.Location = new System.Drawing.Point(428, 4);
            this.changeButton.Name = "changeButton";
            this.changeButton.Size = new System.Drawing.Size(94, 33);
            this.changeButton.TabIndex = 3;
            this.changeButton.Text = "Promijeni";
            this.changeButton.UseVisualStyleBackColor = true;
            this.changeButton.Click += new System.EventHandler(this.changeButton_Click);
            // 
            // chooseComboBox
            // 
            this.chooseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chooseComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.chooseComboBox.FormattingEnabled = true;
            this.chooseComboBox.Items.AddRange(new object[] {
            "Datum početka",
            "Datum završetka",
            "Postotak"});
            this.chooseComboBox.Location = new System.Drawing.Point(539, 10);
            this.chooseComboBox.Name = "chooseComboBox";
            this.chooseComboBox.Size = new System.Drawing.Size(149, 24);
            this.chooseComboBox.TabIndex = 4;
            // 
            // fromTextBox
            // 
            this.fromTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.fromTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fromTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.fromTextBox.Location = new System.Drawing.Point(118, 10);
            this.fromTextBox.Name = "fromTextBox";
            this.fromTextBox.ReadOnly = true;
            this.fromTextBox.Size = new System.Drawing.Size(138, 17);
            this.fromTextBox.TabIndex = 5;
            this.fromTextBox.Text = "Datum početka";
            // 
            // toTextBox
            // 
            this.toTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.toTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.toTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.toTextBox.Location = new System.Drawing.Point(273, 10);
            this.toTextBox.Name = "toTextBox";
            this.toTextBox.ReadOnly = true;
            this.toTextBox.Size = new System.Drawing.Size(135, 17);
            this.toTextBox.TabIndex = 6;
            this.toTextBox.Text = "Datum završetka";
            // 
            // deleteButton
            // 
            this.deleteButton.AutoSize = true;
            this.deleteButton.Location = new System.Drawing.Point(705, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(112, 34);
            this.deleteButton.TabIndex = 7;
            this.deleteButton.Text = "Obriši";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // discountControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.toTextBox);
            this.Controls.Add(this.fromTextBox);
            this.Controls.Add(this.chooseComboBox);
            this.Controls.Add(this.changeButton);
            this.Controls.Add(this.percentTextBox);
            this.Name = "discountControl";
            this.Size = new System.Drawing.Size(820, 42);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox percentTextBox;
        private System.Windows.Forms.Button changeButton;
        private System.Windows.Forms.ComboBox chooseComboBox;
        private System.Windows.Forms.TextBox fromTextBox;
        private System.Windows.Forms.TextBox toTextBox;
        private System.Windows.Forms.Button deleteButton;
    }
}
