using System;
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

namespace CSTool
{
    public partial class Form1 : Form
    {
        //Window where you customise your grid
        public NewWindow newWindow;

        //Tile set
        public PictureBox[,] pGrid;
        public int gridHeight;
        public int gridWidth;
        public int cellSize;

        //Resource Tiles
        public PictureBox[] imageDir = new PictureBox[10];

        //Keeps track of how many total cells we have in the grid
        private int tileNum;

        //VARIABLES FOR DRAG AND DROP FUNCTIONALITY
        bool vData;
        string path;
        Image image;
        Image thumbnail;
        Thread imageThread;

 



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

        //THIS IS WHERE WE GON TRY AND SAVE THESE FILES WOOT
        public void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //INSERT BINARY SAVING HERE
            //Look into the BinaryWriter class on MS Docs
            

            SaveFileDialog save = new SaveFileDialog();             
                                                                    
            save.Filter = "Save Files(*.edtr)|*.edtr";              
            save.Title = "Save your Level File";                    
            save.ShowDialog();                                      
                                                                    
            //If the file name is not empty, continue with saving   
            if (save.FileName != "")                                
            {                                                       
                FileStream fs = (FileStream)save.OpenFile();

                //Write to save here
               
                

                fs.Close();                                         
            }                                                       



        }
        public void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //INSERT BINARY LOADING HERE
            //Look into the BinaryReader class on Microsoft Docs

            
        }

        //Do we really need to save buttons? Not really! Am I gonna get rid of one? Absolutely not!
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //Hopefully we print the piccy out someday?
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

        //GRID CREATION, gets called within NewWindow.CreateButton
        public void createGrid(int gridH, int gridW, int cellsize)
        {
            pGrid = new PictureBox[gridH, gridW]; //Initialising the grid array
            gridHeight = gridH;
            gridWidth = gridW;
            cellSize = cellsize;

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
                    tileNum++; //Keeping count of how many cells we have
                }
            }
           panel2.Refresh();
        }

        //IMPORT Button, for importing an image into the resources window
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            //Setting the image types that the program will look for and accept
            open.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
            
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

         private void saveImage()
        {
            image = new Bitmap(path);
        }

        private bool GetImage(out string fileName, DragEventArgs e)
        {
           bool retrn = false;
           fileName = string.Empty;

           if((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileDrop") as Array;
                if(data != null)
                {
                    if((data.Length == 1) && (data.GetValue(0)) is string)
                    {
                        fileName = ((string[])data)[0];
                        string extension = System.IO.Path.GetExtension(fileName).ToLower();
                        if((extension == ".jpg") || extension == ".jpeg" || extension == ".png" || extension == ".bmp")
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
                            //Need PointToClient so that we get the position relative to the control, not the screen
                            //Check if mouse is hovering over a cell, by testing it's location with reference to it's cellsize
                            //if ((MousePosition.X >= pGrid[row, col].Location.X && MousePosition.Y >= pGrid[row, col].Location.Y)
                            //&& ((MousePosition.X + cellSize) <= (pGrid[row, col].Location.X + cellSize)
                            //&& (MousePosition.Y + cellSize) <= (pGrid[row, col].Location.Y + cellSize)))              
                            
                            pGrid[row, col].SizeMode = PictureBoxSizeMode.StretchImage;
                            pGrid[row, col].Image = image;                            
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

        //Drag leave, for when the user drags an image out of the picture boxes and into the grid
        private void pictureBox1_DragLeave(object sender, EventArgs e)
        {
            //We can only drag out a picture if one exists
            if (pictureBox1.Image != null)
            {
                //Returns a thumbnail of the image;
                //You ever look at a word long enough and it starts to not look like a word anymore? Thumbnail.
               thumbnail =  image.GetThumbnailImage(100, 100, ThumbnailCallbackAbort, IntPtr.Zero);
               
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
         
        }

        //Function to allow us to drag images from the Image repository into the picture box grid
        //private void pGrid_DragEnter(object sender, DragEventArgs e)
        //{
        //    DragEnterImport(e);
        //    //CHECKLIST:
        //    // - Image stretch into cell;
        //    // - Image thumbnail (not quite necessary)
        //}

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
    }
}