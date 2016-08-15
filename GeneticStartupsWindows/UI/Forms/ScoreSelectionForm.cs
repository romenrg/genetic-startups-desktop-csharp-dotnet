using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticStartupsWindows
{
    public partial class ScoreSelectionForm : Form
    {
        public ScoreSelectionForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxScoreFunction_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void buttonSaveScoreFuncion_Click(object sender, EventArgs e)
        {
            this.scoreFunction = (Genetics.ScoreFunctions)Enum.Parse(typeof(Genetics.ScoreFunctions), comboBoxScoreFunction.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public Genetics.ScoreFunctions scoreFunction { get; set; }
    }
}
