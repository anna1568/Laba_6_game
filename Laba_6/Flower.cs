using Laba_6;

public class Flower
{
    public int CenterX, CenterY;
    public int BaseRadius;
    public float CurrentRadius;
    public float MaxRadius;
    public int PetalCount;
    public bool Growing = false;
    public bool IsTouched = false; // вода касается сейчас
    public List<ParticleColorful> Particles = new List<ParticleColorful>();

    public Flower(int x, int y, int radius, int petalCount)
    {
        CenterX = x;
        CenterY = y;
        BaseRadius = radius;
        CurrentRadius = radius;
        MaxRadius = radius * 2; // ограничим рост в 2 раза, чтобы не занимал полэкрана
        PetalCount = petalCount;
    }
}
