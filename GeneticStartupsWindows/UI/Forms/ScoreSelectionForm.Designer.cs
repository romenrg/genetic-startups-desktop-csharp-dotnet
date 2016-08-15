namespace GeneticStartupsWindows
{
    partial class ScoreSelectionForm
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
            this.comboBoxScoreFunction = new System.Windows.Forms.ComboBox();
            this.labelScoreFunction = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxScoreFunction
            // 
            this.comboBoxScoreFunction.Location = new System.Drawing.Point(130, 38);
            this.comboBoxScoreFunction.Items.Add(Genetics.ScoreFunctions.Sum.ToString());
            this.comboBoxScoreFunction.Items.Add(Genetics.ScoreFunctions.Average.ToString());
            this.comboBoxScoreFunction.SelectedIndex = this.comboBoxScoreFunction.FindStringExact(Genetics.ScoreFunctions.Sum.ToString());
            this.comboBoxScoreFunction.Name = "comboBoxScoreFunction";
            this.comboBoxScoreFunction.Size = new System.Drawing.Size(100, 20);
            this.comboBoxScoreFunction.TabIndex = 0;
            this.comboBoxScoreFunction.SelectedIndexChanged += new System.EventHandler(this.comboBoxScoreFunction_SelectedIndexChanged);
            // 
            // labelScoreFunction
            // 
            this.labelScoreFunction.AutoSize = true;
            this.labelScoreFunction.Location = new System.Drawing.Point(13, 41);
            this.labelScoreFunction.Name = "labelScoreFunction";
            this.labelScoreFunction.Size = new System.Drawing.Size(112, 13);
            this.labelScoreFunction.TabIndex = 2;
            this.labelScoreFunction.Text = "Select Score Function";
            this.labelScoreFunction.Click += new System.EventHandler(this.label1_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(85, 98);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSaveScoreFuncion_Click);
            // 
            // ScoreSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 153);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelScoreFunction);
            this.Controls.Add(this.comboBoxScoreFunction);
            this.Name = "ScoreSelectionForm";
            this.Text = "Score Function Selection";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxScoreFunction;
        private System.Windows.Forms.Label labelScoreFunction;
        private System.Windows.Forms.Button buttonSave;
    }
}