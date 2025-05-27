using System;
using System.Collections.Generic;
using System.Drawing;

namespace Laba_6
{
    public class Emitter
    {
        public int X, Y;
        public int ParticlesPerTick = 5;
        public int ParticlesCount = 500;
        public List<IImpactPoint> impactPoints = new List<IImpactPoint>();
        public List<Particle> particles = new List<Particle>();

        public virtual void ResetParticle(Particle particle)
        {
            particle.Life = Particle.rand.Next(80, 120);
            particle.Radius = Particle.rand.Next(5, 10);
            particle.X = X;
            particle.Y = Y;
            double direction = Particle.rand.NextDouble() * Math.PI * 2;
            double speed = 1 + Particle.rand.NextDouble() * 2;
            particle.SpeedY = 1;
            particle.SpeedX = Particle.rand.Next(-2, 2);
            if (particle is ParticleColorful pc)
            {
                pc.Color = Color.White;
            }
        }

        public virtual Particle CreateParticle()
        {
            return new ParticleColorful();
        }

        public void UpdateState()
        {
            // Удаляем погибшие частицы
            particles.RemoveAll(p => p.Life <= 0);

            // Обновляем оставшиеся
            foreach (var particle in particles)
            {
                particle.X += particle.SpeedX;
                particle.Y += particle.SpeedY;
                particle.Life -= 1;

                foreach (var point in impactPoints)
                    point.ImpactParticle(particle);
            }

            // Добавляем только ParticlesPerTick новых частиц за тик
            for (int i = 0; i < ParticlesPerTick && particles.Count < ParticlesCount; i++)
            {
                var p = CreateParticle();
                ResetParticle(p);
                particles.Add(p);
            }
        }

        public void Render(Graphics g)
        {
            foreach (var p in particles)
                p.Draw(g);

            foreach (var point in impactPoints)
                point.Render(g);
        }
    }

    public class TopEmitter : Emitter
    {
        public int Width;

        public override void ResetParticle(Particle particle)
        {
            particle.Life = Particle.rand.Next(80, 120);
            particle.Radius = Particle.rand.Next(5, 10);
            particle.X = Particle.rand.Next(Width);
            particle.Y = 0;
            particle.SpeedY = 2 + (float)Particle.rand.NextDouble() * 2;
            particle.SpeedX = Particle.rand.Next(-2, 3);
            if (particle is ParticleColorful pc)
            {
                pc.Color = Color.White;
            }
        }
    }
}
