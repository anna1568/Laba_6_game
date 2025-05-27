using System;
using System.Drawing;

namespace Laba_6
{
    public class Particle
    {
        public int Radius;
        public float X, Y;
        public float SpeedX, SpeedY;
        public float Life;

        public static Random rand = new Random();

        public Particle()
        {
            Life = 100;
            Radius = 5;
        }

        public virtual void Draw(Graphics g)
        {
            float k = Math.Min(1f, Life / 100);
            int alpha = (int)(k * 255);
            var color = Color.FromArgb(alpha, Color.White);
            using (var b = new SolidBrush(color))
                g.FillEllipse(b, X - Radius, Y - Radius, Radius * 2, Radius * 2);
        }
    }

    public class ParticleColorful : Particle
    {
        public Color Color = Color.White;

        public override void Draw(Graphics g)
        {
            float k = Math.Min(1f, Life / 100);
            int alpha = (int)(k * 255);
            var color = Color.FromArgb(alpha, Color.R, Color.G, Color.B);
            using (var b = new SolidBrush(color))
                g.FillEllipse(b, X - Radius, Y - Radius, Radius * 2, Radius * 2);
        }
    }
}
