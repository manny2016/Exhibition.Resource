using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Exhibition.Core;
using Exhibition.Core.Models;
using System.IO;

namespace Exhibition.Components
{
    public delegate void ImageAnimationDelegate(Bitmap obmp, Bitmap bmp, PictureBox pic);
    public partial class ImagePlayer : UserControl, IOperate
    {
        private readonly Resource resource;
        private ImageAnimationDelegate[] animations = null;
        private Timer timer = new Timer();
        private int tick = 0;
        public ImagePlayer(Resource resource) :
            this()
        {
            this.resource = resource;
            this.animations = new ImageAnimationDelegate[] {
                new ImageAnimationDelegate(this.Effect_BaiYeH),
                new ImageAnimationDelegate(this.Effect_BaiYeV),
                //new ImageAnimationDelegate(this.Effect_D2U),
                //new ImageAnimationDelegate(this.Effect_U2D),
                new ImageAnimationDelegate(this.Effect_L2R),
                new ImageAnimationDelegate(this.Effect_R2L)
            };
            this.Load += ImagePlayer_Load;
            this.timer.Tick += Timer_Tick;
            this.timer.Interval = 1000 * 5;
            this.LoadImage(this.resource);
        }
        private Queue<string> images = new Queue<string>();
        private Bitmap current = null;
        public ImagePlayer()
        {
            InitializeComponent();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (tick == int.MaxValue)
            {
                tick = 0;
            }
            var image = images.Dequeue();
            var bitmap = new Bitmap(image);
            this.picbox.Image = bitmap;
           // this.animations[tick % this.animations.Length](this.current, bitmap, this.picbox);
            images.Enqueue(image);
            this.current = bitmap;
            tick++;
        }

        private void ImagePlayer_Load(object sender, EventArgs e)
        {
            this.Width = this.Parent.Width;
            this.Height = this.Parent.Height;
            this.picbox.Width = this.Width;
            this.picbox.Height = this.Height;
            var image = images.Dequeue();
            this.current = new Bitmap(image);
            this.picbox.Image = this.current;
            this.picbox.SizeMode = PictureBoxSizeMode.StretchImage;
            images.Enqueue(image);
            this.timer.Start();

        }

        private void LoadImage(Resource resource)
        {
            //var directory = new DirectoryInfo(resource.FullName);
            //this.images = new Queue<string>(directory.GetFiles().Where((ctx) =>
            // {
            //     return Constants.EXTENSION_IMAGE_RESOURCE.Any(o => o.Equals(ctx.Extension, StringComparison.OrdinalIgnoreCase));
            // }).Select(o => o.FullName));
        }
        public void Stop()
        {
            this.timer.Stop();
            if (this.IsDisposed == false)
            {
                this.Parent.Controls.Remove(this);
                base.Dispose(true);
            }

        }

        public void Play(Resource resource)
        {
            this.timer.Start();
        }
        #region 切换动画
        /// <summary>
        /// 水平百叶窗
        /// </summary>
        /// <param name="obmp"></param>
        /// <param name="bmp"></param>
        /// <param name="pic"></param>
        public void Effect_BaiYeH(Bitmap obmp, Bitmap bmp, PictureBox pic)
        {
            int step = 20;
            try
            {
                Bitmap bmp1 = (Bitmap)bmp.Clone();
                int height = bmp1.Height / step;
                int width = bmp1.Width;
                Graphics g = Graphics.FromImage(obmp);
                Point[] MyPoint = new Point[step];
                for (int y = 0; y < step; y++)
                {
                    MyPoint[y].X = 0;
                    MyPoint[y].Y = y * height;
                }
                Bitmap bitmap = new Bitmap(bmp.Width, bmp.Height);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < step; j++)
                    {
                        for (int k = 0; k < width; k++)
                        {
                            bitmap.SetPixel(MyPoint[j].X + k, MyPoint[j].Y + i, bmp.GetPixel(MyPoint[j].X + k, MyPoint[j].Y + i));
                        }
                    }
                    pic.Refresh();
                    pic.Image = bitmap;
                    System.Threading.Thread.Sleep(1);
                }
                g.Dispose();
                bmp1.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }

        }
        /// <summary>
        /// 垂直百叶窗
        /// </summary>
        /// <param name="obmp"></param>
        /// <param name="bmp"></param>
        /// <param name="pic"></param>
        public void Effect_BaiYeV(Bitmap obmp, Bitmap bmp, PictureBox pic)
        {
            int step = 1;
            try
            {
                Bitmap bmp1 = (Bitmap)bmp.Clone();
                int dw = bmp1.Width / step;
                int dh = bmp1.Height;
                Graphics g = Graphics.FromImage(obmp);
                Point[] MyPoint = new Point[step];
                for (int x = 0; x < step; x++)
                {
                    MyPoint[x].Y = 0;
                    MyPoint[x].X = x * dw;
                }
                Bitmap bitmap = new Bitmap(bmp1.Width, bmp1.Height);
                for (int i = 0; i < dw; i++)
                {
                    for (int j = 0; j < step; j++)
                    {
                        for (int k = 0; k < dh; k++)
                        {
                            bitmap.SetPixel(MyPoint[j].X + i, MyPoint[j].Y + k, bmp1.GetPixel(MyPoint[j].X + i, MyPoint[j].Y + k));
                        }
                    }
                    pic.Refresh();
                    pic.Image = bitmap;

                    System.Threading.Thread.Sleep(1);
                }
                g.Dispose();
                bmp1.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
        }
        /// <summary>
        /// 从上向下
        /// </summary>
        /// <param name="obmp"></param>
        /// <param name="bmp"></param>
        /// <param name="pic"></param>
        public void Effect_U2D(Bitmap obmp, Bitmap bmp, PictureBox pic)
        {
            try
            {
                int width = bmp.Width;
                int height = bmp.Height;

                Graphics g = pic.CreateGraphics();
                g.DrawImage(obmp, 0, 0, width, height);
                for (int y = 1; y <= height; y += 40)
                {
                    Bitmap bitmap = bmp.Clone(new Rectangle(0, 0, width, y), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    g.DrawImage(bitmap, 0, 0);
                    System.Threading.Thread.Sleep(100);
                }
                g.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
        }
        /// <summary>
        /// 从下至于上
        /// </summary>
        /// <param name="obmp"></param>
        /// <param name="bmp"></param>
        /// <param name="pic"></param>
        public void Effect_D2U(Bitmap obmp, Bitmap bmp, PictureBox pic)
        {
            try
            {
                int width = bmp.Width;
                int height = bmp.Height;

                Graphics g = pic.CreateGraphics();
                g.DrawImage(obmp, 0, 0, width, height);

                for (int y = 1; y <= height; y += 40)
                {
                    Bitmap bitmap = bmp.Clone(new Rectangle(0, height - y, width, y), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    g.DrawImage(bitmap, 0, height - y);
                    System.Threading.Thread.Sleep(100);
                }
                g.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
        }

        public void Effect_L2R(Bitmap obmp, Bitmap bmp, PictureBox pic)
        {
            try
            {
                int width = bmp.Width;
                int height = bmp.Height;
                Graphics g = pic.CreateGraphics();
                g.DrawImage(obmp, 0, 0, width, height);
                for (int x = 1; x <= width; x += 100)
                {
                    Bitmap bitmap = bmp.Clone(new Rectangle(0, 0, x, height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    g.DrawImage(bitmap, 0, 0);
                    System.Threading.Thread.Sleep(10);
                }
                g.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
        }

        public void Effect_R2L(Bitmap obmp, Bitmap bmp, PictureBox pic)
        {
            try
            {
                int width = bmp.Width;
                int height = bmp.Height;
                Graphics g = pic.CreateGraphics();
                g.DrawImage(obmp, 0, 0, width, height);
                for (int x = 1; x <= width; x += 100)
                {
                    //----------------------------------------------w, 0,  0,  h  ||  w-x, 0, +x, h 
                    Bitmap bitmap = bmp.Clone(new Rectangle(width - x, 0, x, height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    g.DrawImage(bitmap, width - x, 0);
                    System.Threading.Thread.Sleep(10);
                }
                g.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
        }

        #endregion
    }
}
