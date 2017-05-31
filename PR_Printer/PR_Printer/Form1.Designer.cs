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
            this.meetIDTextBox = new System.Windows.Forms.TextBox();
            this.meetIDLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.currentDataSourceLabel = new System.Windows.Forms.Label();
            this.changeDataSource = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // accessDatabaseButton
            // 
            this.accessDatabaseButton.Location = new System.Drawing.Point(272, 53);
            this.accessDatabaseButton.Name = "accessDatabaseButton";
            this.accessDatabaseButton.Size = new System.Drawing.Size(111, 23);
            this.accessDatabaseButton.TabIndex = 0;
            this.accessDatabaseButton.Text = "Get PR\'s";
            this.accessDatabaseButton.UseVisualStyleBackColor = true;
            this.accessDatabaseButton.Click += new System.EventHandler(this.accessDatabaseButton_Click);
            // 
            // accessDatabaseLabel
            // 
            this.accessDatabaseLabel.AutoSize = true;
            this.accessDatabaseLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accessDatabaseLabel.Location = new System.Drawing.Point(129, 173);
            this.accessDatabaseLabel.Name = "accessDatabaseLabel";
            this.accessDatabaseLabel.Size = new System.Drawing.Size(157, 20);
            this.accessDatabaseLabel.TabIndex = 1;
            this.accessDatabaseLabel.Text = "All Pdf\'s are stored in";
            // 
            // meetIDTextBox
            // 
            this.meetIDTextBox.Location = new System.Drawing.Point(59, 55);
            this.meetIDTextBox.Name = "meetIDTextBox";
            this.meetIDTextBox.Size = new System.Drawing.Size(100, 20);
            this.meetIDTextBox.TabIndex = 4;
            // 
            // meetIDLabel
            // 
            this.meetIDLabel.AutoSize = true;
            this.meetIDLabel.Location = new System.Drawing.Point(88, 39);
            this.meetIDLabel.Name = "meetIDLabel";
            this.meetIDLabel.Size = new System.Drawing.Size(45, 13);
            this.meetIDLabel.TabIndex = 5;
            this.meetIDLabel.Text = "Meet ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(105, 193);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "My Documents -> PR_Cards";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(414, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "Instructions: Enter the meet ID found in your Hy-Tek Database";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(88, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(345, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "of the newest completed meet. Then click Get PR\'s";
            // 
            // currentDataSourceLabel
            // 
            this.currentDataSourceLabel.AutoSize = true;
            this.currentDataSourceLabel.Location = new System.Drawing.Point(69, 6);
            this.currentDataSourceLabel.Name = "currentDataSourceLabel";
            this.currentDataSourceLabel.Size = new System.Drawing.Size(100, 13);
            this.currentDataSourceLabel.TabIndex = 9;
            this.currentDataSourceLabel.Text = "currentDataSource:";
            // 
            // changeDataSource
            // 
            this.changeDataSource.Location = new System.Drawing.Point(329, 1);
            this.changeDataSource.Name = "changeDataSource";
            this.changeDataSource.Size = new System.Drawing.Size(75, 23);
            this.changeDataSource.TabIndex = 10;
            this.changeDataSource.Text = "Change";
            this.changeDataSource.UseVisualStyleBackColor = true;
            this.changeDataSource.Click += new System.EventHandler(this.changeDataSource_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 283);
            this.Controls.Add(this.changeDataSource);
            this.Controls.Add(this.currentDataSourceLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.meetIDLabel);
            this.Controls.Add(this.meetIDTextBox);
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
        private System.Windows.Forms.TextBox meetIDTextBox;
        private System.Windows.Forms.Label meetIDLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label currentDataSourceLabel;
        private System.Windows.Forms.Button changeDataSource;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

