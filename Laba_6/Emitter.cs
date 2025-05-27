using Laba_6;
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
        private int scoreDelta = 0;

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

            // Параметр t: 0 — базовый размер (молодой цветок), 1 — максимальный размер (расцвет)
            float t = (flower.CurrentRadius - flower.BaseRadius) / (flower.MaxRadius - flower.BaseRadius);
            t = Math.Clamp(t, 0f, 1f);

            // Цвета сердцевины: при увядании — оранжевый, при расцветании — ярко-жёлтый
            Color aliveCenterColor = Color.Yellow;
            Color deadCenterColor = Color.Orange;
            Color centerColor = InterpolateColor(deadCenterColor, aliveCenterColor, t);

            // Палитры лепестков — массивы цветовых гамм
            Color[][] palettes = new Color[][]
            {
        new Color[] { Color.Pink, Color.White, Color.Orange },
        new Color[] { Color.LightBlue, Color.Blue, Color.White },
        new Color[] { Color.Green, Color.LightGreen, Color.Yellow },
        new Color[] { Color.Red, Color.DarkRed, Color.MediumSeaGreen },
        new Color[] { Color.DarkRed, Color.Orange, Color.Beige }
            };

            // Коричнево-оранжевый цвет для увядающих лепестков
            Color deadPetalColor = Color.FromArgb(180, 139, 69, 19);

            // Выбираем палитру для цветка по индексу (фиксируем на время жизни)
            int paletteIndex = flower.PetalCount % palettes.Length;
            Color[] palette = palettes[paletteIndex];

            // Функция интерполяции цвета
            Color InterpolateColor(Color c1, Color c2, float factor)
            {
                int r = (int)(c1.R + (c2.R - c1.R) * factor);
                int g = (int)(c1.G + (c2.G - c1.G) * factor);
                int b = (int)(c1.B + (c2.B - c1.B) * factor);
                int a = (int)(c1.A + (c2.A - c1.A) * factor);
                return Color.FromArgb(a, r, g, b);
            }

            // ===== Сердцевина =====
            for (int i = 0; i < centerParticleCount; i++)
            {
                double angle = 2 * Math.PI * Particle.rand.NextDouble();
                double r = centerRadius * Math.Sqrt(Particle.rand.NextDouble());
                float x = flower.CenterX + (float)(r * Math.Cos(angle));
                float y = flower.CenterY + (float)(r * Math.Sin(angle));

                flower.Particles.Add(new ParticleColorful
                {
                    X = x,
                    Y = y,
                    Radius = particleRadius,
                    Color = centerColor,
                    Life = 100,
                    Scale = 1f
                });
            }

            // ===== Лепестки =====
            for (int p = 0; p < flower.PetalCount; p++)
            {
                double angle = 2 * Math.PI * p / flower.PetalCount;

                // Выбираем цвет лепестка из палитры циклически
                Color brightPetalColor = palette[p % palette.Length];
                // Интерполируем цвет лепестка от коричнево-оранжевого (увядание) к яркому (расцвет)
                Color petalColor = InterpolateColor(deadPetalColor, brightPetalColor, t);

                for (int i = 0; i < particlesPerPetal; i++)
                {
                    double tParam = Particle.rand.NextDouble();
                    double distance = tParam * petalLength;
                    double widthFactor = Math.Sin(Math.PI * tParam);
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


        private void CreateExplosionParticles(Flower flower)
        {
            int explosionParticleCount = 50; // количество частиц взрыва
            Random rand = Particle.rand;

            // Цвета лепестков цветка (как в генерации лепестков)
            Color[][] palettes = new Color[][]
            {
        new Color[] { Color.Pink, Color.White, Color.Orange },
        new Color[] { Color.LightBlue, Color.Blue, Color.White },
        new Color[] { Color.Green, Color.LightGreen, Color.Yellow },
        new Color[] { Color.Red, Color.DarkRed, Color.MediumSeaGreen },
        new Color[] { Color.DarkRed, Color.Orange, Color.Beige }
            };
            int paletteIndex = flower.PetalCount % palettes.Length;
            Color[] palette = palettes[paletteIndex];

            for (int i = 0; i < explosionParticleCount; i++)
            {
                double angle = 2 * Math.PI * rand.NextDouble();
                float speed = 3f + (float)rand.NextDouble() * 3f;

                float sx = (float)(Math.Cos(angle) * speed);
                float sy = (float)(Math.Sin(angle) * speed);

                // Выбираем случайный цвет из палитры лепестков
                Color color = palette[rand.Next(palette.Length)];

                // Добавляем частицу в общий список particles (нужно, чтобы класс TopEmitter имел доступ к нему)
                particles.Add(new ParticleColorful
                {
                    X = flower.CenterX,
                    Y = flower.CenterY,
                    Radius = Math.Max(2, (int)flower.CurrentRadius / 10),
                    Color = color,
                    Life = 80 + rand.Next(40),
                    SpeedX = sx,
                    SpeedY = sy
                });
            }
        }


        private Color GetFlowerColor(Flower flower, bool isCenter)
        {
            // Отношение роста: 0 = базовый размер, 1 = максимальный
            float t = (flower.CurrentRadius - flower.BaseRadius) / (flower.MaxRadius - flower.BaseRadius);
            t = Math.Clamp(t, 0f, 1f);

            if (isCenter)
            {
                // Центр: от ярко-жёлтого до тусклого коричневого
                Color aliveColor = Color.Yellow;
                Color deadColor = Color.FromArgb(150, 100, 50, 0); // коричнево-оранжевый тусклый

                // Чем ближе к смерти (t близко к 0), тем ближе к deadColor
                // Чем ближе к расцветанию (t близко к 1), тем ближе к aliveColor
                return InterpolateColor(deadColor, aliveColor, t);
            }
            else
            {
                // Лепестки: от ярких цветов к тусклым коричневым
                Color[] brightColors = { Color.Pink, Color.Orange, Color.White };
                Color deadColor = Color.FromArgb(150, 100, 50, 0);

                // Выбираем яркий цвет случайно (можно фиксировать для цветка)
                Color brightColor = brightColors[flower.PetalCount % brightColors.Length];

                return InterpolateColor(deadColor, brightColor, t);
            }
        }

        // Линейная интерполяция цвета
        private Color InterpolateColor(Color c1, Color c2, float t)
        {
            int r = (int)(c1.R + (c2.R - c1.R) * t);
            int g = (int)(c1.G + (c2.G - c1.G) * t);
            int b = (int)(c1.B + (c2.B - c1.B) * t);
            int a = (int)(c1.A + (c2.A - c1.A) * t);
            return Color.FromArgb(a, r, g, b);
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
                // Счётчик частиц воды, попавших на цветок
                int hitCount = 0;

                foreach (var wp in waterParticles)
                {
                    float dx = wp.X - flower.CenterX;
                    float dy = wp.Y - flower.CenterY;
                    float dist = (float)Math.Sqrt(dx * dx + dy * dy);

                    if (dist < flower.CurrentRadius + 10)
                    {
                        hitCount++;
                    }
                }

                if (hitCount > 0)
                {
                    flower.Growing = true;

                    // Увеличиваем размер пропорционально количеству частиц,
                    // например, каждая частица даёт прирост 0.02f
                    float growthAmount = 0.02f * hitCount;

                    flower.CurrentRadius = Math.Min(flower.CurrentRadius + growthAmount, flower.MaxRadius);

                    if (flower.CurrentRadius >= flower.MaxRadius)
                    {
                        flowersToRemove.Add(flower);
                        continue;
                    }
                }

                else
                {
                    flower.Growing = false;

                    // Уменьшаем размер, если нет попаданий воды
                    flower.CurrentRadius -= 0.05f;

                    if (flower.CurrentRadius <= flower.BaseRadius * 0.5f)
                    {
                        flowersToRemove.Add(flower);
                        continue;
                    }
                }

                GenerateFlowerParticles(flower);
            }

            // Удаляем выросшие и уменьшившиеся цветы, создаём новые
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
