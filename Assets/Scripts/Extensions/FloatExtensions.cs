public static class FloatExtensions
{
    public static void Clamp(this float f, float min, float max)
    {
        if (f < min) f = min;
        if (f > max) f = max;
    }

}