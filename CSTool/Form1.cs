﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace CSTool
{
    public partial class Form1 : Form
    {
        //Window where you customise your grid
        public NewWindow newWindow;

        //Tile set
        public PictureBox[,] pGrid = new PictureBox[,] { }; //Creation of an empty Grid
        public bool[,] pGridCheck;
        public int gridHeight;
        public int gridWidth;
        public int cellSize;

        public string[,] tempGrid;

        //Info to save to computer for later loading functionality
        public string[] gridFP; //gridFilePath
        public string[] resourceFP = new string[10]; //Won't ever be more than 10 source pics
        public int[] gridInfo = new int[3]; //gridHeight, gridWidth, cellSize. In that order.

        //Resource Tiles
        public PictureBox[] imageDir = new PictureBox[10];

        //Keeps track of how many total cells we have in the grid
        private int tileNum;

        //VARIABLES FOR DRAG AND DROP FUNCTIONALITY
        bool vData;
        string path;
        Image image;
        Thread imageThread;
        Image highlightImage;

        public delegate void AssignImageDlgt();

        public Form1()
        {
            InitializeComponent();
            newWindow = new NewWindow(this);

            //Initilising the picture boxes into an array for later use and easier accessibility 
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

            AllowDrop = true;
            panel1.AllowDrop = true;
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

        public void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Creates a an array with enough space to save the entire grid
            gridFP = new string[pGrid.Length];

            SaveFileDialog save = new SaveFileDialog();

            save.Filter = "Save Files(*.xml)|*.xml";
            save.Title = "Save your Level File";

            if (save.ShowDialog() == DialogResult.OK)
            {

                //If the file name is not empty, continue with saving   
                if (save.FileName != "")
                {
                    Stream fs = save.OpenFile();
                    StreamWriter sw = new StreamWriter(fs);

                    //Writing the grid info first, as this will be the first to be read
                    sw.WriteLine(gridHeight);
                    sw.WriteLine(gridWidth);
                    sw.WriteLine(cellSize);

                    //Saving all filepaths within the grid
                    for (int i = 0; i < gridHeight; i++)
                    {
                        for (int j = 0; j < gridWidth; j++)
                        {
                            //If the cell doesn't have an image in it, write null to the .xml file. We'll use this to check whether to import an image or not
                            if (pGrid[i, j].Image == null)
                            {
                                sw.WriteLine("null");
                            }

                            //Writing our 1D array into a text file
                            else
                                sw.WriteLine(pGrid[i, j].ImageLocation.ToString());
                        }
                    }
                    sw.Close();
                }
            }
        }

        public void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog load = new OpenFileDialog();

            load.Filter = "Load Files(*.xml)|*.xml";
            load.Title = "Load your Level File";

            if (load.ShowDialog() == DialogResult.OK)
            {

                Stream fs = load.OpenFile();
                StreamReader sr = new StreamReader(fs);

                //When we saved, the first things we wrote to the .xml was the grid info, so here we read it back in the same order we wrote it.
                //Int32.Parse() is a good way to convert a string into a function
                gridHeight = Int32.Parse(sr.ReadLine());
                gridWidth = Int32.Parse(sr.ReadLine());
                cellSize = Int32.Parse(sr.ReadLine());

                //Creating the grid before we can import to it
                createGrid(gridHeight, gridWidth, cellSize);

                for (int i = 0; i < gridHeight; i++)
                {
                    for (int j = 0; j < gridWidth; j++)
                    {
                        pGrid[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                        pGrid[i, j].ImageLocation = sr.ReadLine().ToString(); ;
                    }
                }


                sr.Close();
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            destroyGrid();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
        }

        //Picture panel - stores reference to all level tiles
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        //----------------------------------------------------------------------------------||
        //GRID CREATION, gets called within NewWindow.CreateButton
        //----------------------------------------------------------------------------------||
        public void createGrid(int gridH, int gridW, int cellsize)
        {
            pGrid = new PictureBox[gridH, gridW]; //Initialising the grid array
            pGridCheck = new bool[gridH, gridW];  //Initialising the bool array
            gridHeight = gridH;
            gridWidth = gridW;
            cellSize = cellsize;

            destroyGrid();

            for (int row = 0; row < gridW; row++)
            {
                for (int col = 0; col < gridH; col++)
                {
                    //Welp, it compiles. Progress
                    pGrid[row, col] = new PictureBox();
                    pGrid[row, col].Margin = new Padding(0, 0, 0, 0);
                    pGrid[row, col].Dock = DockStyle.None;
                    pGrid[row, col].BackColor = Color.White;
                    pGrid[row, col].BorderStyle = BorderStyle.FixedSingle;
                    pGrid[row, col].Size = new Size(cellSize, cellSize);
                    pGrid[row, col].Location = new Point(row * cellSize, col * cellSize);
                    panel2.Controls.Add(pGrid[row, col]);
                    pGrid[row, col].Click += new EventHandler(this.pGrid_Click); //Creation of the Event handler for our new grid, now they can be clicked on
                    tileNum++; //Keeping count of how many cells we have
                }
            }
            panel2.Refresh();
        }

        public void createGrid(bool ha)
        {
            
                destroyGrid();
                createGrid(1, 1, 500);


            //Revised Polymorphism
            if (ha)
            {
                ImageDrawer H = new Hagrid();
                H.drawImage(pGrid);
            }

            else
            {
                ImageDrawer S = new Swagrid();
                S.drawImage(pGrid);
            }
        }
        //----------------------------------------------------------------------------------||

        //------------------------------------------------------------------||
        // Very simple inheritence with polymorphism
        //------------------------------------------------------------------||
        public class ImageDrawer 
        {
            public virtual void drawImage(PictureBox[,] pGrid)
            {
                pGrid[0, 0].Image = null;
            }
        }

        public class Hagrid : ImageDrawer
        {
            public override void drawImage(PictureBox[,] pGrid)
            {           
                pGrid[0, 0].Image = Properties.Resources.HaGrid;
            }
        }

        public class Swagrid : ImageDrawer
        {
            public override void drawImage(PictureBox[,] pGrid)
            {
                pGrid[0, 0].SizeMode = PictureBoxSizeMode.StretchImage;
                pGrid[0, 0].Image = Properties.Resources.Swagrid;
            }
        }
        //------------------------------------------------------------------||

        public void destroyGrid()
        {
            //Removing all the images from the grid
            for (int i = 0; i < gridHeight; i++)
            {
                for (int j = 0; j < gridWidth; j++)
                {
                    if (pGrid[i, j] != null)
                        pGrid[i, j].Image = null;
                    panel2.Controls.Clear();
                }
            }
        }

        //IMPORT Button, for importing an image into the resources window
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            //Setting the image types that the program will look for and accept
            open.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";

            if (open.ShowDialog() == DialogResult.OK)
            {
                //Iterating through all text boxes to find an empty one
                for (int i = 0; i < 10; i++)
                {
                    if (imageDir[i].Image == null)
                    {
                        //Stretches the image so that it fits in the 100 x 100 box
                        imageDir[i].SizeMode = PictureBoxSizeMode.StretchImage;
                        imageDir[i].Image = new Bitmap(open.FileName);
                        imageDir[i].ImageLocation = open.FileName;
                        break;
                    }
                }
            }
        }

        private void saveImage()
        {
            image = new Bitmap(path);
        }

        private bool GetImage(out string fileName, DragEventArgs e)
        {
            bool retrn = false;
            fileName = string.Empty;

            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileDrop") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0)) is string)
                    {
                        fileName = ((string[])data)[0];
                        string extension = System.IO.Path.GetExtension(fileName).ToLower();
                        if ((extension == ".jpg") || extension == ".jpeg" || extension == ".png" || extension == ".bmp")
                        {
                            retrn = true;
                        }
                    }
                }
            }
            return retrn;
        }

        public void DragEnterImport(DragEventArgs e)
        {
            string fileName;
            vData = GetImage(out fileName, e);
            if (vData)
            {
                path = fileName;
                imageThread = new Thread(new ThreadStart(saveImage));
                imageThread.Start();
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        public void DragDropImport()
        {
            if (vData)
            {
                while (imageThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);

                }
                for (int i = 0; i < 10; i++)
                {
                    if (imageDir[i].Image == null)
                    {
                        imageDir[i].SizeMode = PictureBoxSizeMode.StretchImage;
                        imageDir[i].Image = image;
                        imageDir[i].ImageLocation = path;
                        return;
                    }
                }
            }
        }

        //ISSUE CORRECTION:
        //We ran into the issue where we had all the drag and drop allowances set in the picturebox, yet the actual allowances
        //were being blocked by the overlaying form1, so now we need to rewrite it in the form
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            DragEnterImport(e);
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            DragDropImport();
        }

        //Allows us the drag into Panel 2, which is where the level grid is stored
        private void panel2_DragEnter(object sender, DragEventArgs e)
        {
            DragEnterImport(e);
        }

        private void panel2_DragDrop(object sender, DragEventArgs e)
        {
            //If the grid has not been created, we treat this action as a simple image import for the resource panel.
            if (pGrid == null)
            {
                DragDropImport();
            }

            //If the grid HAS been created, then we try and import the image into the grid
            else if (pGrid != null)
            {
                string fileName;
                vData = GetImage(out fileName, e); //Checking that the image is valid and storing the filename
                Point mouse = new Point();
                Point gPoint = new Point(5, 5); //Point to store the current grid cell's position

                mouse = MousePosition; //Gets the mouse position;
                if (vData)   //Only run the code if the data is valid - error checking
                {
                    for (int row = 0; row < gridWidth; row++)
                    {
                        for (int col = 0; col < gridHeight; col++)
                        {
                            pGrid[row, col].SizeMode = PictureBoxSizeMode.StretchImage;
                            pGrid[row, col].Image = image;
                            pGrid[row, col].ImageLocation = fileName;
                        }
                    }
                    panel2.Refresh();
                }
            }
        }

        protected bool ThumbnailCallbackAbort()
        {
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        //Drop iteration of the grid's drag and drop functionality
        private void pGrid_DragDrop(object sender, DragEventArgs e)
        {
            string fileName;
            vData = GetImage(out fileName, e); //Checking that the image is valid and storing the filename
            Point mouse = new Point();
            Point gPoint = new Point(5, 5); //Point to store the current grid cell's position

            mouse = MousePosition; //Gets the mouse position;
            if (vData)   //Only run the code if the data is valid - error checking
            {
                for (int row = 0; row < gridWidth; row++)
                {
                    for (int col = 0; col < gridHeight; col++)
                    {
                        //Need PointToClient so that we get the position relative to the control, not the screen
                        //Check if mouse is hovering over a cell, by testing it's location with reference to it's cellsize
                        if ((MousePosition.X >= pGrid[row, col].Location.X && MousePosition.Y >= pGrid[row, col].Location.Y)
                        && ((MousePosition.X + cellSize) <= (pGrid[row, col].Location.X + cellSize)
                        && (MousePosition.Y + cellSize) <= (pGrid[row, col].Location.Y + cellSize)))
                        {
                            pGrid[row, col].Image = image;
                            pGrid[row, col].ImageLocation = fileName;
                        }
                    }
                }
            }
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            DragEnterImport(e);
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            DragDropImport();
        }

        private void canvasSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pGrid == null)
                createGrid(true);

            else
                createGrid(false);
        }


        //If an image is highlighted when a grid cell is clicked, that grid cell now becomes the highlighted image
        private void pGrid_Click(object sender, EventArgs e)
        {
            //Everytime we click into the panel, get the current mouse position
            float mouseX = Cursor.Position.X;
            float mouseY = Cursor.Position.Y;

            if (highlightImage != null)
            {
                for (int i = 0; i < gridHeight; i++)
                {
                    for (int j = 0; j < gridWidth; j++)
                    {  
                        if (pGrid[i, j] == sender)
                        {
                            pGrid[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                            pGrid[i, j].Image = highlightImage;
                            pGrid[i, j].ImageLocation = path;
                            break;
                        }
                    }
                }
            }
        }
        #region Picturebox Click
        //--------------------------------------------------------------------------------------------------------|
        //  Grid Painting
        //--------------------------------------------------------------------------------------------------------|

        //Have a "pointer" that points to whichever box the user has clicked on and highlight it, then when a grid cell is clicked on, change that 
        //picture to be the same one as the highlighted box
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(pictureBox1.Image != null)
            {

                highlightImage = pictureBox1.Image;
                path = pictureBox1.ImageLocation;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if(pictureBox2.Image != null)
            {
                highlightImage = pictureBox2.Image;
                path = pictureBox2.ImageLocation;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (pictureBox3.Image != null)
            {
                highlightImage = pictureBox3.Image;
                path = pictureBox3.ImageLocation;

            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (pictureBox4.Image != null)
            {
                highlightImage = pictureBox4.Image;
                path = pictureBox4.ImageLocation;

            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (pictureBox5.Image != null)
            {
                highlightImage = pictureBox5.Image;
                path = pictureBox5.ImageLocation;

            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (pictureBox6.Image != null)
            {
                highlightImage = pictureBox6.Image;
                path = pictureBox6.ImageLocation;

            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (pictureBox7.Image != null)
            {
                highlightImage = pictureBox7.Image;
                path = pictureBox7.ImageLocation;

            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (pictureBox8.Image != null)
            {
                highlightImage = pictureBox8.Image;
                path = pictureBox8.ImageLocation;

            }
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            if (pictureBox9.Image != null)
            {
                highlightImage = pictureBox9.Image;
                path = pictureBox9.ImageLocation;

            }
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            if (pictureBox10.Image != null)
            {
                highlightImage = pictureBox10.Image;
                path = pictureBox10.ImageLocation;

            }
        }
        //--------------------------------------------------------------------------------------------------------|
        #endregion        
    }
}