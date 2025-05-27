using System;
using System.Drawing;

namespace Laba_6
{
    public class Particle
    {
        public int Radius;
        public float X, Y;
        public float SpeedX = 0, SpeedY = 0;
        public float Life = 100;

        public static Random rand = new Random();

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
        public float Scale = 1f; // масштаб частицы

        public override void Draw(Graphics g)
        {
            float k = Math.Min(1f, Life / 100);
            int alpha = (int)(k * Color.A); // используем альфу цвета!
            var color = Color.FromArgb(alpha, Color.R, Color.G, Color.B);
            int scaledRadius = (int)(Radius * Scale);
            using (var b = new SolidBrush(color))
                g.FillEllipse(b, X - scaledRadius, Y - scaledRadius, scaledRadius * 2, scaledRadius * 2);
        }

    }


}
