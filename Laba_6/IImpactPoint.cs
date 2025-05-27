using System.Drawing;

namespace Laba_6
{
    public abstract class IImpactPoint
    {
        public float X, Y;
        public abstract void ImpactParticle(Particle particle);
        public virtual void Render(Graphics g)
        {
            g.DrawEllipse(new Pen(Color.White, 2), X - 15, Y - 15, 30, 30);
        }
    }

    public class ColorPoint : IImpactPoint
    {
        public int Radius = 30;
        public Color Color = Color.Red;

        public override void ImpactParticle(Particle particle)
        {
            float dx = X - particle.X;
            float dy = Y - particle.Y;
            float dist = (float)Math.Sqrt(dx * dx + dy * dy);
            if (dist < Radius && particle is ParticleColorful pc)
            {
                pc.Color = this.Color;
            }
        }

        public override void Render(Graphics g)
        {
            using (var pen = new Pen(Color, 3))
                g.DrawEllipse(pen, X - Radius, Y - Radius, Radius * 2, Radius * 2);
        }
    }
}
