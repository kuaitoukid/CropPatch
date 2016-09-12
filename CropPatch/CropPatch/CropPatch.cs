using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CropPatch
{
    public partial class CropPatch : Form
    {
        public CropPatch()
        {
            InitializeComponent();
        }

        private List<string> selectedFileNames = new List<string>();
        private Image currentImage;
        private float resizeRatio = 1;
        private Graphics picBoxGraphics;
        private Point startPoint;
        private Point endPoint;
        private Point ImageLeftTop;
        private Point ImageRightBottom;

        private void btnOpenBrowser_Click(object sender, EventArgs e)
        {
            // this.picBrowserDialog.ShowDialog();
            this.selectedFileNames.Clear();
            OpenFileDialog ofg = new OpenFileDialog();
            ofg.Filter = "png(*.png), jpg(*.jpg), bmp(*.bmp)|*.png;*.jpg;*.bmp";
            ofg.Title = "Select Images";
            ofg.Multiselect = true;
            if (ofg.ShowDialog() == DialogResult.OK)
            {
                for (int fid = 0; fid < ofg.FileNames.Length; fid++)
			    {
                    this.selectedFileNames.Add(ofg.FileNames[fid]);			 
			    }
            }
            if (this.selectedFileNames.Count > 0) {
                LoadImage(this.selectedFileNames[0]);
            }

        }

        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            this.picBox.Refresh();
            this.picBoxGraphics = this.picBox.CreateGraphics();
            this.startPoint = new Point(e.X, e.Y);
            this.endPoint = new Point(e.X, e.Y);

            this.picBoxGraphics.DrawRectangle(new Pen(Color.FromArgb(255, 0, 0), 3), 
                new Rectangle(Math.Min(this.startPoint.X, this.endPoint.X), Math.Min(this.startPoint.Y, this.endPoint.Y),
                    Math.Abs(this.startPoint.X - this.endPoint.X + 1), Math.Abs(this.startPoint.Y - this.endPoint.Y + 1)));
            this.picBoxGraphics.Dispose();
        }

        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
            this.picBoxGraphics = this.picBox.CreateGraphics();
            this.endPoint = new Point(e.X, e.Y);
            this.picBoxGraphics.DrawRectangle(new Pen(Color.FromArgb(255, 0, 0), 3),
                new Rectangle(Math.Min(this.startPoint.X, this.endPoint.X), Math.Min(this.startPoint.Y, this.endPoint.Y),
                    Math.Abs(this.startPoint.X - this.endPoint.X + 1), Math.Abs(this.startPoint.Y - this.endPoint.Y + 1)));
            this.picBoxGraphics.Dispose();
        }

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.picBox.Refresh();
                this.picBoxGraphics = this.picBox.CreateGraphics();
                this.endPoint = new Point(e.X, e.Y);
                this.picBoxGraphics.DrawRectangle(new Pen(Color.FromArgb(255, 0, 0), 3),
                    new Rectangle(Math.Min(this.startPoint.X, this.endPoint.X), Math.Min(this.startPoint.Y, this.endPoint.Y),
                        Math.Abs(this.startPoint.X - this.endPoint.X + 1), Math.Abs(this.startPoint.Y - this.endPoint.Y + 1)));
                this.picBoxGraphics.Dispose();
            }
            this.lblMousePosition.Text = "X: " + e.X.ToString() + ", Y: " + e.Y.ToString();
        }

        private void LoadImage(string imageName)
        {
            this.currentImage = Image.FromFile(imageName);
            this.resizeRatio = 1.0f * Math.Max(this.picBox.Height, this.picBox.Width) / Math.Max(this.currentImage.Height, this.currentImage.Width);
            this.picBox.Image = this.currentImage;
            int centerX = this.picBox.Width / 2;
            int centerY = this.picBox.Height / 2;
            int halfWidth = (int)(this.resizeRatio * this.currentImage.Width / 2);
            int halfHeight = (int)(this.resizeRatio * this.currentImage.Height / 2);
            
            this.ImageLeftTop = new Point(centerX - halfWidth, centerY - halfHeight);
            this.ImageRightBottom = new Point(centerX + halfWidth, centerY + halfHeight);


        }
    }
}
