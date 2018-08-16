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
        //Tile set
        public PictureBox[,] pGrid;
        //Resource Tiles
        public PictureBox[] imageDir = new PictureBox[10];
       

        private int pTileNum;


        public Form1()
        {
            InitializeComponent();
            newWindow = new NewWindow(this);

            //Initilising the picture boxes into an array for later use
            imageDir[0] = pictureBox1;
            imageDir[1] = pictureBox2;
            imageDir[2] = pictureBox3;
            imageDir[3] = pictureBox4;
            imageDir[4] = pictureBox5;
            imageDir[5] = pictureBox6;
            imageDir[6] = pictureBox7;
            imageDir[7] = pictureBox8;
            imageDir[8] = pictureBox9;
            imageDir[9] = pictureBox10;

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
                    pGrid[row, col].Margin = new Padding(0, 0, 0, 0);
                    pGrid[row, col].Dock = DockStyle.None;
                    pGrid[row, col].BackColor = Color.White;
                    pGrid[row, col].BorderStyle = BorderStyle.FixedSingle;
                    pGrid[row, col].Location = new Point(row, col);
                    pGrid[row, col].Show();
                }
            }
        }

        //IMPORT Button, for importing an image into the resources window
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";
            
            if(open.ShowDialog() == DialogResult.OK)
            {
                //Iterating through all text boxes to find an empty one
                for (int i = 0; i < 10; i++)
                {
                    if(imageDir[i].Image == null)
                    {
                        //Stretches the image so that it fits in the 100 x 100 box
                        imageDir[i].SizeMode = PictureBoxSizeMode.StretchImage;
                        imageDir[i].Image = new Bitmap(open.FileName);
                        break;
                    }
                }
            }
        }

        //Drag and drop into pictureBox1 FROM an outside source. Triggers when an object is dragged into the controll's bounds
        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            
        }

        //Called when the drag and drop is actually completed 
        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {

        }
    }
}
