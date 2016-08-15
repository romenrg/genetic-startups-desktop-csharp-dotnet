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
    public partial class MainForm : Form
    {
        private Genetics genetics;
        private int numCols = 24;
        private int numRows = 11;
        private int numSteps = 35;
        private int cellWidth = 45;
        private int cellHeight = 45;
        private int cellPadding = 5;
        private UIAritmetics uiAritmetics;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMap;
        private System.Windows.Forms.PictureBox[,] tableCells;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button buttonGenerateMap;
        private System.Windows.Forms.Button buttonStartEvolution;
        private System.Windows.Forms.MenuStrip menuStripMainMenu;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem algorithmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem squaresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem licenseToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;

        // -----------------------------
        //  Public methods
        // -----------------------------

        public MainForm()
        {
            //InitializeComponent();
            // Initialize helpers
            this.uiAritmetics = new UIAritmetics();
            // Generate first screen (including table)
            this.createBasicLayout();
            this.createDynamicTable();
            this.setLayout();
        }

        public void createBasicLayout()
        {
            //Icon
            this.Icon = GeneticStartupsWindows.Properties.Resources.entrepreneur_starting_ico;
            // Creating elements
            this.tableLayoutPanelMap = new System.Windows.Forms.TableLayoutPanel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.buttonGenerateMap = new System.Windows.Forms.Button();
            this.buttonStartEvolution = new System.Windows.Forms.Button();
            this.menuStripMainMenu = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.algorithmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.squaresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.licenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanelMap.SuspendLayout();
            this.menuStripMainMenu.SuspendLayout();
            this.SuspendLayout();
            // labelTitle
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Size = new System.Drawing.Size(200, 23);
            this.labelTitle.AutoSize = true;
            this.labelTitle.TabIndex = 4;
            this.labelTitle.Text = "......... Welcome to Genetic Startups world .........";
            // buttonGenerateMap
            this.buttonGenerateMap.Name = "buttonGenerateMap";
            this.buttonGenerateMap.Size = new System.Drawing.Size(92, 23);
            this.buttonGenerateMap.TabIndex = 1;
            this.buttonGenerateMap.Text = "Generate Map";
            this.buttonGenerateMap.UseVisualStyleBackColor = true;
            this.buttonGenerateMap.Click += new System.EventHandler(this.buttonGenerateMap_Click);
            // buttonStartEvolution
            this.buttonStartEvolution.Name = "buttonStartEvolution";
            this.buttonStartEvolution.Size = new System.Drawing.Size(90, 23);
            this.buttonStartEvolution.TabIndex = 2;
            this.buttonStartEvolution.Text = "Start Evolution";
            this.buttonStartEvolution.Enabled = false;
            this.buttonStartEvolution.UseVisualStyleBackColor = true;
            this.buttonStartEvolution.Click += new System.EventHandler(this.buttonStartEvolution_Click);
            this.buttonStartEvolution.Enabled = false;
            // Form1
            this.Controls.Add(this.buttonStartEvolution);
            this.Controls.Add(this.buttonGenerateMap);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.tableLayoutPanelMap);
            this.Controls.Add(this.menuStripMainMenu);
            this.MainMenuStrip = this.menuStripMainMenu;
            this.Name = "Form1";
            this.Text = "Genetic Startups, by @romenrg88";
            this.Load += new System.EventHandler(this.Form1_Load);
            // menuStripMainMenu
            this.menuStripMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                                this.infoToolStripMenuItem,
                                                this.settingsToolStripMenuItem
                                          });
            this.menuStripMainMenu.Location = new System.Drawing.Point(0, 0);
            this.menuStripMainMenu.Name = "menuStripMainMenu";
            this.menuStripMainMenu.Size = new System.Drawing.Size(473, 24);
            this.menuStripMainMenu.TabIndex = 3;
            this.menuStripMainMenu.Text = "Menu";
            // infoToolStripMenuItem
            this.infoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.algorithmToolStripMenuItem,
                this.squaresToolStripMenuItem,
                this.licenseToolStripMenuItem
            });
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.infoToolStripMenuItem.Text = "Info";
            // algorithmToolStripMenuItem
            this.algorithmToolStripMenuItem.Name = "algorithmStripMenuItem";
            this.algorithmToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.algorithmToolStripMenuItem.Text = "The Algorithm";
            this.algorithmToolStripMenuItem.Click += new System.EventHandler(this.algorithmToolStripMenuItem_Click);
            // squaresToolStripMenuItem
            this.squaresToolStripMenuItem.Name = "squaresToolStripMenuItem";
            this.squaresToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.squaresToolStripMenuItem.Text = "Square Types";
            this.squaresToolStripMenuItem.Click += new System.EventHandler(this.squaresToolStripMenuItem_Click);
            // licenseToolStripMenuItem
            this.licenseToolStripMenuItem.Name = "licenseStripMenuItem";
            this.licenseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.licenseToolStripMenuItem.Text = "License";
            this.licenseToolStripMenuItem.Click += new System.EventHandler(this.licenseToolStripMenuItem_Click);
            // settingsToolStripMenuItem
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapSizeToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // mapSizeToolStripMenuItem
            this.mapSizeToolStripMenuItem.Name = "mapSizeToolStripMenuItem";
            this.mapSizeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.mapSizeToolStripMenuItem.Text = "Map Size";
            this.mapSizeToolStripMenuItem.Click += new System.EventHandler(this.mapSizeToolStripMenuItem_Click);
        }

        public void createDynamicTable()
        {
            this.tableLayoutPanelMap.Controls.Clear();
            this.tableLayoutPanelMap.RowStyles.Clear();
            this.tableLayoutPanelMap.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanelMap.ColumnCount = this.numCols;
            this.tableLayoutPanelMap.RowCount = this.numRows;
            this.tableLayoutPanelMap.Name = "tableLayoutPanelMap";
            this.tableCells = new System.Windows.Forms.PictureBox[this.numCols, this.numRows];
            for (int i=0; i<this.numCols; i++)
            {
                this.tableLayoutPanelMap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, this.cellHeight));
                for (int j=0; j<this.numRows; j++) {
                    this.tableLayoutPanelMap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, this.cellWidth));
                    this.tableCells[i, j] = new System.Windows.Forms.PictureBox();
                    this.tableLayoutPanelMap.Controls.Add(this.tableCells[i, j], i, j);
                }
            }
            int tableWidth = (this.numCols * (this.cellWidth + 2)) + 2;
            int tableHeight = (this.numRows * (this.cellHeight + 2)) + 2;
            this.tableLayoutPanelMap.Size = new System.Drawing.Size(tableWidth, tableHeight);
        }

        public void setLayout()
        {
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = this.uiAritmetics.calculateClientSize(this.tableLayoutPanelMap.Size, this.menuStripMainMenu.Size, this.cellHeight, this.cellWidth, this.buttonGenerateMap.Size, this.buttonStartEvolution.Size);
            this.tableLayoutPanelMap.Location = this.uiAritmetics.calculateTableLocationCenteredInContainer(this.tableLayoutPanelMap.Size, this.ClientSize);
            this.labelTitle.Location = new System.Drawing.Point(this.ClientSize.Width / 2 - this.labelTitle.Size.Width / 2, this.menuStripMainMenu.Size.Height + this.cellHeight * 2 / 5);
            this.buttonGenerateMap.Location = new System.Drawing.Point(this.ClientSize.Width / 5, this.tableLayoutPanelMap.Size.Height + this.menuStripMainMenu.Size.Height + this.cellHeight * 5 / 3);
            this.buttonStartEvolution.Location = new System.Drawing.Point(this.ClientSize.Width / 5 * 4 - this.buttonStartEvolution.Size.Width, this.tableLayoutPanelMap.Size.Height + this.menuStripMainMenu.Size.Height + this.cellHeight * 5 / 3);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanelMap.ResumeLayout(false);
            this.menuStripMainMenu.ResumeLayout(false);
            this.menuStripMainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public void generateMap()
        {
            // In future maybe set the number of columns and rows like
            // http://stackoverflow.com/questions/15623461/adding-pictureboxes-to-tablelayoutpanel-is-very-slow
            Cursor.Current = Cursors.WaitCursor;
            this.genetics = new Genetics(this.numCols, this.numRows, this.numSteps);
            this.genetics.createBoard();
            for (int i = 0; i < this.tableLayoutPanelMap.ColumnCount; i++)
            {
                for (int j = 0; j < this.tableLayoutPanelMap.RowCount; j++)
                {
                    this.tableCells[i, j].Image = this.genetics.getIconForPos(i, j);
                    this.tableCells[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            Cursor.Current = Cursors.Default;
            this.buttonStartEvolution.Enabled = true;
        }


        // -----------------------------
        //  Private methods
        // -----------------------------
        private async Task showBestCandidateOfGeneration()
        {
            Tuple<int, int>[] bestIndividualCellsPath = genetics.getBestIndividualCellsPath();
            int x, y;
            for (int j = 0; j < bestIndividualCellsPath.Length; j++)
            {
                x = bestIndividualCellsPath[j].Item1;
                y = bestIndividualCellsPath[j].Item2;
                if (genetics.isCellInsideMap(x, y))
                {
                    this.applyStylesToVisitedCell(x, y);
                    await Task.Delay(750);
                }
            }
        }

        private void applyStylesToVisitedCell(int x, int y)
        {
            this.tableCells[x, y].Padding = new Padding(this.cellPadding);
            if (this.tableCells[x, y].BackColor == Color.LightBlue)
            {
                this.tableCells[x, y].BackColor = Color.DeepSkyBlue;
            }
            else if (this.tableCells[x, y].BackColor == Color.DeepSkyBlue)
            {
                this.tableCells[x, y].BackColor = Color.Blue;
            }
            else if (this.tableCells[x, y].BackColor == Color.Blue)
            {
                this.tableCells[x, y].BackColor = Color.Blue;
            }
            else
            {
                this.tableCells[x, y].BackColor = Color.LightBlue;
            }
        }

        private void cleanMap()
        {
            for (int i = 0; i<this.numCols; i++)
            {
                for (int j=0; j<this.numRows; j++)
                {
                    this.tableCells[i, j].BackColor = default(Color);
                    this.tableCells[i, j].Padding = new Padding(0);
                }
            }
        }


        // -----------------------------
        //  Event Listeners
        // -----------------------------

        private void buttonGenerateMap_Click(object sender, EventArgs e)
        {
            this.generateMap();
            this.buttonStartEvolution.Enabled = true;
        }

        private async void buttonStartEvolution_Click(object sender, EventArgs e)
        {
            this.genetics.generatePopulation(25);
            this.genetics.generateScores();
            this.labelTitle.Text = "Generation: 1" + " / " + Genetics.NUM_GENERATIONS + ". Best score (displayed): " + genetics.individualsSortedByScore[0].Value;
            for (int i = 1; i < Genetics.NUM_GENERATIONS; i++)
            {
                await this.showBestCandidateOfGeneration();
                this.cleanMap();
                await Task.Delay(750);
                this.genetics.newGeneration();
                this.genetics.generateScores();
                this.labelTitle.Text = "Generation: " + (i + 1) + " / " + Genetics.NUM_GENERATIONS + ". Best score (displayed): " + genetics.individualsSortedByScore[0].Value;
            }
            await this.showBestCandidateOfGeneration();
        }

        private void mapSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MapSizeform form2 = new MapSizeform();
            var result = form2.ShowDialog();
            if (result == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                this.numCols = form2.numCols;
                this.numRows = form2.numRows;
                this.numSteps = this.numCols + this.numRows;
                this.createDynamicTable();
                this.setLayout();
                Cursor.Current = Cursors.Default;
            }
        }

        private void algorithmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TheAlgorithmForm theAlgorithmform = new TheAlgorithmForm();
            var result = theAlgorithmform.ShowDialog();
            if (result == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                Cursor.Current = Cursors.Default;
            }
        }

        private void squaresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TypesOfSquaresForm form3 = new TypesOfSquaresForm();
            var result = form3.ShowDialog();
            if (result == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                Cursor.Current = Cursors.Default;
            }
        }

        private void licenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LicenseForm licenseForm = new LicenseForm();
            var result = licenseForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                Cursor.Current = Cursors.Default;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
