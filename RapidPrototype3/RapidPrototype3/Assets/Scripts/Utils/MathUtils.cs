using UnityEngine;

public static class MathUtils
{
    public static float Remap(float value, float minValue, float maxValue)
    {
        return (value - minValue) / (maxValue - minValue);
    }

    public static float Remap(float value, float minValue, float maxValue, float newMin, float newMax)
    {
        return newMin + (value - minValue) * (newMax - newMin) / (maxValue - minValue);
    }
}
