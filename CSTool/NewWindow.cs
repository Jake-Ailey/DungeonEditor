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
    public partial class NewWindow : Form
    {
        public int gridHeight;
        public int gridWidth;
        public int cellSize;

        public NewWindow()
        {
            InitializeComponent();
        }

        private void NewWindow_Load(object sender, EventArgs e)
        {

        }

        //GRID HEIGHT textBox3
        private void gridHeight_TextChanged(object sender, EventArgs e)
        {
            //The textbox will only accept 2 digits, 
            textBox3.MaxLength = 2;

            if(textBox3.TextLength == 0)
            {
                gridHeight = 0;
                return;
            }

            gridHeight = Int32.Parse(textBox3.Text);

            if (gridWidth != 0 && cellSize != 0)
                panel2.Refresh();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only allows numeric input, and does not allow decimal places
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //GRID WIDTH textBox4
        private void gridWidth_TextChanged(object sender, EventArgs e)
        {
            //The textbox will only accept 2 digits
            textBox4.MaxLength = 2;

            if (textBox4.TextLength == 0)
            {
                gridHeight = 0;
                return;
            }

            gridWidth = Int32.Parse(textBox4.Text);

            if(gridHeight != 0 && cellSize != 0)
            panel2.Refresh();
        }

       
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only allows numeric input, and does not allow decimal places
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //CELL SIZE textbox6
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            //The textbox will only accept 2 digits
            textBox6.MaxLength = 2;

            if (textBox6.TextLength == 0)
            {
                cellSize = 0;
                return;
            }

            cellSize = Int32.Parse(textBox6.Text);

            if (gridHeight != 0 && gridWidth != 0)
                panel2.Refresh();
        }

        //Function that only allows the user to enter in numerical keys
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only allows numeric input, and does not allow decimal places
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            //Live preview of how big your grid will be

            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Black);

            for (int y = 0; y < gridHeight; ++y)
            {
                g.DrawLine(p, 0, y * cellSize, gridHeight * cellSize, y * cellSize);
            }
            //Draws an extra line on the right to close off the grid
            g.DrawLine(p, gridWidth * cellSize, 0, gridWidth * cellSize, gridHeight * cellSize);

            for (int x = 0; x < gridWidth; ++x)
            {
                g.DrawLine(p, x * cellSize, 0, x * cellSize, gridWidth * cellSize);
            }
            //Draws an extra line on the bottom to close off the grid
            g.DrawLine(p, 0, gridHeight * cellSize, gridWidth * cellSize, gridHeight * cellSize);
        }       
    }
}
