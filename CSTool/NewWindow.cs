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
        public int gridWidth;
        public int gridHeight;

        public NewWindow()
        {
            InitializeComponent();
        }

        private void NewWindow_Load(object sender, EventArgs e)
        {

        }

        private void gridHeight_TextChanged(object sender, EventArgs e)
        {
            gridHeight = (int)sender;
        }

        private void gridWidth_TextChanged(object sender, EventArgs e)
        {
            gridWidth = (int)sender;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < gridHeight; i++)
            {
                for (int j = 0; j < gridWidth; j++)
                {
                    Panel p = new Panel();
                    p.Location = new Point(gridHeight * i + j, gridWidth * i + j);
                    p.Size = new Size(16, 16);
                    p.BorderStyle = BorderStyle.FixedSingle;
                    p.Margin = new Padding(1, 1, 1, 1);
                    p.Dock = DockStyle.None;
                    p.BackColor = Color.White;


                }
            }
        }
    }
}
