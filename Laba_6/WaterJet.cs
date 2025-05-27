using System;
using System.Collections.Generic;
using System.Drawing;

namespace Laba_6
{
    public class WaterJet
    {
        public float X, Y; // позиция источника (мышь)
        public int ParticleCount = 120; // сколько частиц максимум
        private List<ParticleColorful> particles = new List<ParticleColorful>();

        public void UpdatePosition(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void Update()
        {
            // Старим и двигаем все частицы
            foreach (var p in particles)
            {
                p.X += p.SpeedX;
                p.Y += p.SpeedY;
                p.Life -= 2;
                // Немного "размазываем" по X для естественности
                p.SpeedX *= 0.97f;
                p.SpeedY += 0.12f; // "гравитация"
            }

            // Удаляем умершие частицы
            particles.RemoveAll(p => p.Life <= 0);

            // Добавляем новые частицы к мышке
            for (int i = 0; i < 4; i++) // 4 новых частицы за тик для плотности
            {
                if (particles.Count < ParticleCount)
                {
                    double angle = -Math.PI / 2 + (Particle.rand.NextDouble() - 0.5) * 0.5; // чуть в стороны
                    float speed = 2f + (float)Particle.rand.NextDouble() * 1.5f;
                    float sx = (float)(Math.Cos(angle) * speed) + (float)(Particle.rand.NextDouble() - 0.5f) * 0.5f;
                    float sy = (float)(Math.Sin(angle) * speed) + 1.5f;

                    particles.Add(new ParticleColorful
                    {
                        X = X,
                        Y = Y,
                        Radius = 8,
                        Color = Color.FromArgb(140, 180, 255, 255), // нежно-голубой
                        Life = 100 + Particle.rand.Next(30),
                        SpeedX = sx,
                        SpeedY = sy
                    });
                }
            }
        }

        public void Render(Graphics g)
        {
            foreach (var p in particles)
                p.Draw(g);
        }

        // Для взаимодействия с цветами: возвращаем копию списка
        public List<ParticleColorful> GetParticles() => new List<ParticleColorful>(particles);
    }
}
