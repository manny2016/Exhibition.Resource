using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Measure
{
    public partial class Form1 : Form
    {
        public Form1(int idx)
        {
            InitializeComponent();
            this.KeyDown += Form1_KeyDown;
            this.MouseMove += Form1_MouseMove;
            this.Load += Form1_Load1;
            this.index = idx;
        }
        private int index = 0;
        private void Form1_Load1(object sender, EventArgs e)
        {
            
            this.Locating(index);
        }
        private void Locating(int idx)
        {

            
                var screen = Screen.AllScreens[idx];
                this.Location = new Point(screen.Bounds.X, screen.Bounds.Y);
                this.Top = 0;
                this.Left = screen.WorkingArea.Left;
                this.Width = screen.WorkingArea.Width;
                this.Height = screen.WorkingArea.Height;
            
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
          
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {           

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Locating();
        }
        bool first = true;
        private void button1_Click(object sender, EventArgs e)
        {

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                var location = this.textBox1.Text.Split(',');
                this.panel1.Location = new Point(int.Parse(location[0]), int.Parse(location[1]));
                this.panel1.Size = new Size(int.Parse(location[2]),int.Parse(location[3]));
            }
            catch
            {

            }
        }
    }
}
