using System;
using System.Collections.Generic;
using System.Drawing;

namespace Laba_6
{
    public class Emitter
    {
        public int Width, Height;
        public List<Particle> particles = new List<Particle>();

        public virtual void GenerateFlowers(int count)
        {
            // переопределяется в TopEmitter
        }

        public void Render(Graphics g)
        {
            foreach (var p in particles)
                p.Draw(g);
        }
    }

    public class TopEmitter : Emitter
    {
        private List<Flower> flowers = new List<Flower>();
        private List<(float X, float Y, int Radius)> flowerCenters = new List<(float, float, int)>();

        public override void GenerateFlowers(int count)
        {
            particles.Clear();
            flowers.Clear();
            flowerCenters.Clear();

            int attemptsLimit = 1000;

            for (int i = 0; i < count; i++)
            {
                int flowerSize = Particle.rand.Next(15, 30);
                int petalCount = Particle.rand.Next(5, 13);

                int attempt = 0;
                bool positionFound = false;
                int centerX = 0, centerY = 0;

                while (attempt < attemptsLimit && !positionFound)
                {
                    centerX = Particle.rand.Next(60, Width - 60);
                    centerY = Particle.rand.Next(Height / 2, Height - 60);

                    positionFound = true;
                    foreach (var fc in flowerCenters)
                    {
                        float dx = centerX - fc.X;
                        float dy = centerY - fc.Y;
                        float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                        if (dist < flowerSize + fc.Radius + 20)
                        {
                            positionFound = false;
                            break;
                        }
                    }
                    attempt++;
                }

                if (!positionFound)
                    continue;

                flowerCenters.Add((centerX, centerY, flowerSize));
                Flower flower = new Flower(centerX, centerY, flowerSize, petalCount);
                flowers.Add(flower);

                GenerateFlowerParticles(flower);
            }

            UpdateParticlesList();
        }

        private void GenerateFlowerParticles(Flower flower)
        {
            flower.Particles.Clear();

            int centerRadius = (int)(flower.CurrentRadius / 2);
            int petalLength = (int)flower.CurrentRadius;

            int particlesPerPetal = 300 * (int)flower.CurrentRadius / 30;
            int centerParticleCount = 360 * (int)flower.CurrentRadius / 30;

            int particleRadius = Math.Max(1, (int)flower.CurrentRadius / 10);
            double maxWidth = flower.CurrentRadius * 0.5;
            double petalOffset = centerRadius + flower.CurrentRadius * 0.33;

            // Сердцевина
            for (int i = 0; i < centerParticleCount; i++)
            {
                double t = 2 * Math.PI * Particle.rand.NextDouble();
                double r = centerRadius * Math.Sqrt(Particle.rand.NextDouble());
                float x = flower.CenterX + (float)(r * Math.Cos(t));
                float y = flower.CenterY + (float)(r * Math.Sin(t));

                flower.Particles.Add(new ParticleColorful
                {
                    X = x,
                    Y = y,
                    Radius = particleRadius,
                    Color = Color.Yellow,
                    Life = 100,
                    Scale = 1f
                });
            }

            // Лепестки
            for (int p = 0; p < flower.PetalCount; p++)
            {
                double angle = 2 * Math.PI * p / flower.PetalCount;
                Color petalColor = GetRandomPetalColor();

                for (int i = 0; i < particlesPerPetal; i++)
                {
                    double t = Particle.rand.NextDouble();
                    double distance = t * petalLength;
                    double widthFactor = Math.Sin(Math.PI * t);
                    double offset = (Particle.rand.NextDouble() - 0.5) * maxWidth * widthFactor;

                    double localX = distance;
                    double localY = offset;

                    float rotatedX = (float)(localX * Math.Cos(angle) - localY * Math.Sin(angle));
                    float rotatedY = (float)(localX * Math.Sin(angle) + localY * Math.Cos(angle));

                    float x = flower.CenterX + (float)(Math.Cos(angle) * petalOffset) + rotatedX;
                    float y = flower.CenterY + (float)(Math.Sin(angle) * petalOffset) + rotatedY;

                    flower.Particles.Add(new ParticleColorful
                    {
                        X = x,
                        Y = y,
                        Radius = particleRadius,
                        Color = petalColor,
                        Life = 100,
                        Scale = 1f
                    });
                }
            }
        }

        private void UpdateParticlesList()
        {
            particles.Clear();
            foreach (var flower in flowers)
                particles.AddRange(flower.Particles);
        }

        private Color GetRandomPetalColor()
        {
            Color[] palette = { Color.Red, Color.Pink, Color.Magenta, Color.Orange, Color.Violet, Color.DeepPink };
            return palette[Particle.rand.Next(palette.Length)];
        }

        public void UpdateGrowth(List<ParticleColorful> waterParticles)
        {
            List<Flower> flowersToRemove = new List<Flower>();

            foreach (var flower in flowers)
            {
                bool touched = false;

                // Проверяем, касается ли цветок вода
                foreach (var wp in waterParticles)
                {
                    float dx = wp.X - flower.CenterX;
                    float dy = wp.Y - flower.CenterY;
                    float dist = (float)Math.Sqrt(dx * dx + dy * dy);

                    if (dist < flower.CurrentRadius + 10)
                    {
                        touched = true;
                        break;
                    }
                }

                flower.Growing = touched;

                if (flower.Growing)
                {
                    // Растём с шагом 0.15, не превышая MaxRadius
                    flower.CurrentRadius = Math.Min(flower.CurrentRadius + 0.40f, flower.MaxRadius);

                    // Если достигли максимального размера — цветок исчезает и создаётся новый
                    if (flower.CurrentRadius >= flower.MaxRadius)
                    {
                        flowersToRemove.Add(flower);
                        continue; // переходим к следующему цветку
                    }

                    GenerateFlowerParticles(flower);
                }
                else
                {
                    // Уменьшаемся с шагом 0.05, не меньше половины базового размера
                    flower.CurrentRadius -= 0.05f;

                    if (flower.CurrentRadius <= flower.BaseRadius * 0.5f)
                    {
                        // Если слишком маленький — удаляем и создаём новый цветок
                        flowersToRemove.Add(flower);
                        continue;
                    }

                    GenerateFlowerParticles(flower);
                }
            }

            // Удаляем отмеченные цветы и создаём новые
            foreach (var flower in flowersToRemove)
            {
                flowers.Remove(flower);
                flowerCenters.RemoveAll(fc => fc.X == flower.CenterX && fc.Y == flower.CenterY);

                int newSize = Particle.rand.Next(15, 30);
                int newPetalCount = Particle.rand.Next(5, 13);

                int attemptsLimit = 1000;
                int attempt = 0;
                bool positionFound = false;
                int centerX = 0, centerY = 0;

                while (attempt < attemptsLimit && !positionFound)
                {
                    centerX = Particle.rand.Next(60, Width - 60);
                    centerY = Particle.rand.Next(Height / 2, Height - 60);

                    positionFound = true;
                    foreach (var fc in flowerCenters)
                    {
                        float dx = centerX - fc.X;
                        float dy = centerY - fc.Y;
                        float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                        if (dist < newSize + fc.Radius + 20)
                        {
                            positionFound = false;
                            break;
                        }
                    }
                    attempt++;
                }

                if (positionFound)
                {
                    flowerCenters.Add((centerX, centerY, newSize));
                    Flower newFlower = new Flower(centerX, centerY, newSize, newPetalCount);
                    flowers.Add(newFlower);
                    GenerateFlowerParticles(newFlower);
                }
            }

            UpdateParticlesList();
        }


    }
}
