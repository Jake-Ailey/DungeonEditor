using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSTool
{
    public partial class Form1 : Form
    {
        public NewWindow newWindow;
        public PictureBox[,] pGrid;

        private int pTileNum;


        public Form1()
        {
            InitializeComponent();
            newWindow = new NewWindow(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newWindow.Show();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

        }

        //Picture panel - stores reference to all level tiles
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            PictureBox[,] pTile;
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        //Margin, Dock, Backcolour, Location, Size, 
        //GRID CREATION, gets called within NewWindow.CreateButton
        public void createGrid(int gridH, int gridW, int cellSize)
        {
            pGrid = new PictureBox[gridH, gridW]; //Initialising the grid array

            for (int row = 0; row < gridH; row++)
            {
                for (int col = 0; col < gridW; col++)
                {
                    //Welp, it compiles. Progress
                    pGrid[row, col] = new PictureBox();
                    pGrid[row, col].Margin = new Padding(1, 1, 1, 1);
                    pGrid[row, col].Dock = DockStyle.None;
                    pGrid[row, col].BackColor = Color.White;
                    pGrid[row, col].BorderStyle = BorderStyle.FixedSingle;
                    pGrid[row, col].Location = new Point(row, col);
                    pGrid[row, col].Show();
                }
            }
        }

       
    }
}
