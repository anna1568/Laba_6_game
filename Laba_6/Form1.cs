using System;
using System.Drawing;
using System.Windows.Forms;

namespace Laba_6
{
    public partial class Form1 : Form
    {
        private int score = 0;
        private int elapsedSeconds = 0;
        private bool isGameOver = false;
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

            // Инициализация трекбаров
            trackBarParticleCount.Minimum = 10;
            trackBarParticleCount.Maximum = 300;
            trackBarParticleCount.Value = waterJet.ParticleCount;
            trackBarParticleCount.TickFrequency = 10;
            trackBarParticleCount.ValueChanged += TrackBarParticleCount_ValueChanged;

            trackBarParticleSpeed.Minimum = 1;
            trackBarParticleSpeed.Maximum = 20;
            trackBarParticleSpeed.Value = (int)(waterJet.SpeedMultiplier * 10);
            trackBarParticleSpeed.TickFrequency = 1;
            trackBarParticleSpeed.ValueChanged += TrackBarParticleSpeed_ValueChanged;

            timer1.Interval = 30;
            timer1.Tick += Timer1_Tick;
            timer1.Start();

            picDisplay.MouseMove += picDisplay_MouseMove;

        }

        private void TrackBarParticleCount_ValueChanged(object sender, EventArgs e)
        {
            waterJet.ParticleCount = trackBarParticleCount.Value;
        }

        private void TrackBarParticleSpeed_ValueChanged(object sender, EventArgs e)
        {
            waterJet.SpeedMultiplier = trackBarParticleSpeed.Value / 10f;
        }

        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            waterJet.UpdatePosition(e.X, e.Y);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            waterJet.Update();
            emitter.UpdateGrowth(waterJet.GetParticles());


            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);
                emitter.Render(g);
                waterJet.Render(g);
            }

            picDisplay.Invalidate();
        }

        private void labelTime_Click(object sender, EventArgs e)
        {

        }
    }
}
