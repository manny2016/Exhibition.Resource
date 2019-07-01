using Exhibition.Components;
using Exhibition.Core;
using Exhibition.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace Exhibition.Agent.Show
{
    public partial class FrmLayoutInfo : Form
    {
        private readonly int DefaultMonitor;
        private readonly Timer timer = new Timer();
        private readonly Window[] winows;
        public FrmLayoutInfo(int monitor, Window[] windows)
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.DefaultMonitor = monitor;
            this.winows = windows;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Locating();
         
            this.timer.Interval = 10 * 1000;
            this.timer.Tick += Timer_Tick;
            this.timer.Start();
            foreach (var window in this.winows)
            {
                var panel = new Panel();
                panel.Location = new Point(window.Location.X, window.Location.Y);
                panel.Size = new Size(window.Size.Width, window.Size.Height);
                panel.BackColor = Color.Black;
                var label = new Label();
                label.ForeColor = Color.Red;
                label.AutoSize = false;
                label.Width = panel.Width;
                label.Height = panel.Height;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Text = $"{this.DefaultMonitor}|{window.SerializeToJson()} \r\n 屏幕分辨率:{this.Width}*{this.Height}"; 
                label.Font = new Font(FontFamily.GenericSerif, 15, FontStyle.Italic);

              
                panel.Controls.Add(label);
                this.Controls.Add(panel);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Locating()
        {
            var screen = Screen.AllScreens[this.DefaultMonitor];
            this.Location = new Point(screen.Bounds.X, screen.Bounds.Y);
            this.Top = 0;
            this.Left = screen.WorkingArea.Left;
            this.Width = screen.WorkingArea.Width;
            this.Height = screen.WorkingArea.Height;
        }
    }
}
