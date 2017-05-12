namespace PR_Printer
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
            this.accessDatabaseButton = new System.Windows.Forms.Button();
            this.accessDatabaseLabel = new System.Windows.Forms.Label();
            this.retrieveDataButton = new System.Windows.Forms.Button();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // accessDatabaseButton
            // 
            this.accessDatabaseButton.Location = new System.Drawing.Point(12, 35);
            this.accessDatabaseButton.Name = "accessDatabaseButton";
            this.accessDatabaseButton.Size = new System.Drawing.Size(111, 23);
            this.accessDatabaseButton.TabIndex = 0;
            this.accessDatabaseButton.Text = "Access Database";
            this.accessDatabaseButton.UseVisualStyleBackColor = true;
            this.accessDatabaseButton.Click += new System.EventHandler(this.accessDatabaseButton_Click);
            // 
            // accessDatabaseLabel
            // 
            this.accessDatabaseLabel.AutoSize = true;
            this.accessDatabaseLabel.Location = new System.Drawing.Point(144, 40);
            this.accessDatabaseLabel.Name = "accessDatabaseLabel";
            this.accessDatabaseLabel.Size = new System.Drawing.Size(35, 13);
            this.accessDatabaseLabel.TabIndex = 1;
            this.accessDatabaseLabel.Text = "label1";
            // 
            // retrieveDataButton
            // 
            this.retrieveDataButton.Location = new System.Drawing.Point(12, 76);
            this.retrieveDataButton.Name = "retrieveDataButton";
            this.retrieveDataButton.Size = new System.Drawing.Size(111, 23);
            this.retrieveDataButton.TabIndex = 2;
            this.retrieveDataButton.Text = "Retrieve Data";
            this.retrieveDataButton.UseVisualStyleBackColor = true;
            this.retrieveDataButton.Click += new System.EventHandler(this.retrieveDataButton_Click);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // printButton
            // 
            this.printButton.Location = new System.Drawing.Point(12, 163);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(111, 23);
            this.printButton.TabIndex = 3;
            this.printButton.Text = "Print";
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.retrieveDataButton);
            this.Controls.Add(this.accessDatabaseLabel);
            this.Controls.Add(this.accessDatabaseButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button accessDatabaseButton;
        private System.Windows.Forms.Label accessDatabaseLabel;
        private System.Windows.Forms.Button retrieveDataButton;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.Button printButton;
    }
}

