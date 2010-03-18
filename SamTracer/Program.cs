using System.Drawing;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace SamTracer
{
    class Program
    {
        public partial class RayTracerForm : Form
        {
            Bitmap bitmap;
            PictureBox pictureBox;
            const int width = 600;
            const int height = 600;

            public RayTracerForm()
            {
                bitmap = new Bitmap(width, height);

                pictureBox = new PictureBox();
                pictureBox.Dock = DockStyle.Fill;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Image = bitmap;

                ClientSize = new System.Drawing.Size(width, height + 24);
                Controls.Add(pictureBox);
                Text = "Ray Tracer";
                Load += RayTracerForm_Load;

                Show();
            }

            private void RayTracerForm_Load(object sender, EventArgs e)
            {
                this.Show();
                RayTracer rayTracer = new RayTracer(width, height,
                    (int x, int y, System.Drawing.Color pix) =>
                    {
                        bitmap.SetPixel(x, y, pix);
                        if (x == 0) pictureBox.Refresh();
                    });
                rayTracer.castRays();
                pictureBox.Invalidate();

            }
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();

                Application.Run(new RayTracerForm());
            }


        }
    }
}
