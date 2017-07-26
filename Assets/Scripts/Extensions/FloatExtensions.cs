public static class FloatExtensions
{
    public static float Clamp(this float f, float min, float max)
    {
        if (f < min)
        {
            return min;
        }
        if (f > max)
        {
            return max;
        }
        return f;
    }
}