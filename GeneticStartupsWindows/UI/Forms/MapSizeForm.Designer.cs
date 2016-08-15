namespace GeneticStartupsWindows
{
    partial class MapSizeform
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
            this.textBoxNumCols = new System.Windows.Forms.TextBox();
            this.textBoxNumRows = new System.Windows.Forms.TextBox();
            this.labelNumCols = new System.Windows.Forms.Label();
            this.labelNumRows = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxNumCols
            // 
            this.textBoxNumCols.Location = new System.Drawing.Point(130, 38);
            this.textBoxNumCols.Name = "textBoxNumCols";
            this.textBoxNumCols.Size = new System.Drawing.Size(100, 20);
            this.textBoxNumCols.TabIndex = 0;
            this.textBoxNumCols.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBoxNumRows
            // 
            this.textBoxNumRows.Location = new System.Drawing.Point(130, 87);
            this.textBoxNumRows.Name = "textBoxNumRows";
            this.textBoxNumRows.Size = new System.Drawing.Size(100, 20);
            this.textBoxNumRows.TabIndex = 1;
            // 
            // labelNumCols
            // 
            this.labelNumCols.AutoSize = true;
            this.labelNumCols.Location = new System.Drawing.Point(13, 41);
            this.labelNumCols.Name = "labelNumCols";
            this.labelNumCols.Size = new System.Drawing.Size(98, 13);
            this.labelNumCols.TabIndex = 2;
            this.labelNumCols.Text = "Number of columns";
            this.labelNumCols.Click += new System.EventHandler(this.label1_Click);
            // 
            // labelNumRows
            // 
            this.labelNumRows.AutoSize = true;
            this.labelNumRows.Location = new System.Drawing.Point(13, 90);
            this.labelNumRows.Name = "labelNumRows";
            this.labelNumRows.Size = new System.Drawing.Size(86, 13);
            this.labelNumRows.TabIndex = 3;
            this.labelNumRows.Text = "Number of Rows";
            this.labelNumRows.Click += new System.EventHandler(this.label2_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(85, 136);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 181);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelNumRows);
            this.Controls.Add(this.labelNumCols);
            this.Controls.Add(this.textBoxNumRows);
            this.Controls.Add(this.textBoxNumCols);
            this.Name = "Form2";
            this.Text = "Map Size";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxNumCols;
        private System.Windows.Forms.TextBox textBoxNumRows;
        private System.Windows.Forms.Label labelNumCols;
        private System.Windows.Forms.Label labelNumRows;
        private System.Windows.Forms.Button buttonSave;
    }
}