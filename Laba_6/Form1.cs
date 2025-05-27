using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Laba_6
{
    public partial class Form1 : Form
    {
        TopEmitter emitter;
        List<ColorPoint> colorPoints = new List<ColorPoint>();

        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            emitter = new TopEmitter
            {
                Width = picDisplay.Width,
                X = Particle.rand.Next(picDisplay.Width),
                Y = 0,
                ParticlesCount = 500
            };

            // Создаем цветные точки и добавляем их и в emitter, и в colorPoints
            colorPoints.Add(new ColorPoint { X = 70, Y = 120, Color = Color.Red });
            colorPoints.Add(new ColorPoint { X = 120, Y = 160, Color = Color.Orange });
            colorPoints.Add(new ColorPoint { X = 180, Y = 180, Color = Color.Yellow });
            colorPoints.Add(new ColorPoint { X = 242, Y = 190, Color = Color.Lime });
            colorPoints.Add(new ColorPoint { X = 304, Y = 180, Color = Color.Cyan });
            colorPoints.Add(new ColorPoint { X = 365, Y = 160, Color = Color.Blue });
            colorPoints.Add(new ColorPoint { X = 414, Y = 120, Color = Color.Violet });

            foreach (var point in colorPoints)
            {
                emitter.impactPoints.Add(point);
            }

            timer1.Interval = 20;
            timer1.Start();

            // Установка начальных значений для трекбаров
            tbPoint1X.Value = (int)colorPoints[0].X;
            tbPoint1Y.Value = (int)colorPoints[0].Y;

            tbPoint2X.Value = (int)colorPoints[1].X;
            tbPoint2Y.Value = (int)colorPoints[1].Y;

            tbPoint3X.Value = (int)colorPoints[2].X;
            tbPoint3Y.Value = (int)colorPoints[2].Y;

            tbPoint4X.Value = (int)colorPoints[3].X;
            tbPoint4Y.Value = (int)colorPoints[3].Y;

            tbPoint5X.Value = (int)colorPoints[4].X;
            tbPoint5Y.Value = (int)colorPoints[4].Y;

            tbPoint6X.Value = (int)colorPoints[5].X;
            tbPoint6Y.Value = (int)colorPoints[5].Y;

            tbPoint7X.Value = (int)colorPoints[6].X;
            tbPoint7Y.Value = (int)colorPoints[6].Y;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            // Обновляем координаты всех точек по значениям трекбаров
            colorPoints[0].X = tbPoint1X.Value;
            colorPoints[0].Y = tbPoint1Y.Value;

            colorPoints[1].X = tbPoint2X.Value;
            colorPoints[1].Y = tbPoint2Y.Value;

            colorPoints[2].X = tbPoint3X.Value;
            colorPoints[2].Y = tbPoint3Y.Value;

            colorPoints[3].X = tbPoint4X.Value;
            colorPoints[3].Y = tbPoint4Y.Value;

            colorPoints[4].X = tbPoint5X.Value;
            colorPoints[4].Y = tbPoint5Y.Value;

            colorPoints[5].X = tbPoint6X.Value;
            colorPoints[5].Y = tbPoint6Y.Value;

            colorPoints[6].X = tbPoint7X.Value;
            colorPoints[6].Y = tbPoint7Y.Value;

            emitter.UpdateState();
            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);
                emitter.Render(g);
            }

            int x1 = tbPoint1X.Value;
            int y1 = tbPoint1Y.Value;
            picDisplay.Invalidate();
        }

        private void tbPoint1X_Scroll(object sender, EventArgs e)
        {
            colorPoints[0].X = tbPoint1X.Value;
        }

        private void tbPoint1Y_Scroll(object sender, EventArgs e)
        {
            colorPoints[0].Y = tbPoint1Y.Value;
        }

        private void tbPoint2X_Scroll(object sender, EventArgs e)
        {
            colorPoints[1].X = tbPoint2X.Value;
        }

        private void tbPoint2Y_Scroll(object sender, EventArgs e)
        {
            colorPoints[1].Y = tbPoint2Y.Value;
        }

        private void tbPoint3X_Scroll(object sender, EventArgs e)
        {
            colorPoints[2].X = tbPoint3X.Value;
        }

        private void tbPoint3Y_Scroll(object sender, EventArgs e)
        {
            colorPoints[2].Y = tbPoint3Y.Value;
        }

        private void tbPoint4X_Scroll(object sender, EventArgs e)
        {
            colorPoints[3].X = tbPoint4X.Value;
        }

        private void tbPoint4Y_Scroll(object sender, EventArgs e)
        {
            colorPoints[3].Y = tbPoint4Y.Value;
        }

        private void tbPoint5X_Scroll(object sender, EventArgs e)
        {
            colorPoints[4].X = tbPoint5X.Value;
        }

        private void tbPoint5Y_Scroll(object sender, EventArgs e)
        {
            colorPoints[4].Y = tbPoint5Y.Value;
        }


        private void tbPoint6X_Scroll(object sender, EventArgs e)
        {
            colorPoints[5].X = tbPoint6X.Value;
        }

        private void tbPoint6Y_Scroll(object sender, EventArgs e)
        {
            colorPoints[5].Y = tbPoint6Y.Value;
        }

        private void tbPoint7X_Scroll(object sender, EventArgs e)
        {
            colorPoints[6].X = tbPoint7X.Value;
        }

        private void tbPoint7Y_Scroll(object sender, EventArgs e)
        {
            colorPoints[6].Y = tbPoint7Y.Value;
        }


    }
}
