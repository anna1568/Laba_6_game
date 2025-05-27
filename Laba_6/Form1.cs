using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Laba_6
{
    public partial class Form1 : Form
    {
        TopEmitter emitter;
        WaterJet waterJet;

        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            emitter = new TopEmitter
            {
                Width = picDisplay.Width,
                Height = picDisplay.Height
            };
            emitter.GenerateFlowers(8);

            waterJet = new WaterJet();

            timer1.Interval = 30; // примерно 30 FPS
            timer1.Tick += Timer1_Tick;
            timer1.Start();

            picDisplay.MouseMove += picDisplay_MouseMove;
        }

        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            waterJet.UpdatePosition(e.X, e.Y);
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            waterJet.Update();
            emitter.UpdateGrowth(waterJet.GetParticles()); // теперь передаём частицы

            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);
                emitter.Render(g);
                waterJet.Render(g);
            }

            picDisplay.Invalidate();
        }

    }
}
