using Laba_6;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Laba_6
{
    public class WaterJet
    {
        public float X, Y;
        public int ParticleCount = 120;
        public float SpeedMultiplier = 0.6f;

        private List<ParticleColorful> particles = new List<ParticleColorful>();

        public WaterJet()
        {
            for (int i = 0; i < ParticleCount; i++)
                particles.Add(CreateParticleNear(0, 0));
        }

        private ParticleColorful CreateParticleNear(float baseX, float baseY)
        {
            double angle = -Math.PI / 2 + (Particle.rand.NextDouble() - 0.5) * 0.5;
            float baseSpeed = 2f + (float)Particle.rand.NextDouble() * 1.5f;
            float sx = (float)(Math.Cos(angle) * baseSpeed) + (float)(Particle.rand.NextDouble() - 0.5f) * 0.5f;
            float sy = (float)(Math.Sin(angle) * baseSpeed) + 1.5f;

            sx *= SpeedMultiplier;
            sy *= SpeedMultiplier;

            return new ParticleColorful
            {
                X = baseX,
                Y = baseY,
                Radius = 8,
                Color = Color.FromArgb(140, 180, 255, 255),
                Life = 150 + Particle.rand.Next(50),
                SpeedX = sx,
                SpeedY = sy
            };
        }

        public void UpdatePosition(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void Update()
        {
            foreach (var p in particles)
            {
                p.X += p.SpeedX;
                p.Y += p.SpeedY;
                p.Life -= 2;
                p.SpeedX *= 0.97f;
                p.SpeedY += 0.12f * SpeedMultiplier;
            }

            particles.RemoveAll(p => p.Life <= 0);

            while (particles.Count < ParticleCount)
                particles.Add(CreateParticleNear(X, Y));
        }

        public void Render(Graphics g)
        {
            foreach (var p in particles)
                p.Draw(g);
        }

        public List<ParticleColorful> GetParticles() => new List<ParticleColorful>(particles);
    }
}
