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
        public Form1()
        {
            InitializeComponent();
            this.KeyDown += Form1_KeyDown;
            this.MouseMove += Form1_MouseMove;
            this.Load += Form1_Load1;
        }

        private void Form1_Load1(object sender, EventArgs e)
        {
            
        }
        private void Locating(int index)
        {
           
            if (Screen.AllScreens.Length > 1)
            {
                var screen = Screen.AllScreens[index];
                this.Location = new Point(screen.Bounds.X, screen.Bounds.Y);
                this.Top = 0;
                this.Left = screen.WorkingArea.Left;
                this.Width = screen.WorkingArea.Width;
                this.Height = screen.WorkingArea.Height;
              
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            this.label1.Text = $"x={e.Location.X},\r\n y={e.Location.Y};\r\n width={this.Width},\r\n height={this.Height}";
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
            Locating(0);
        }
        bool first = true;
        private void button1_Click(object sender, EventArgs e)
        {

            Locating(first?0:1);
            first = !first;
        }
    }
}
