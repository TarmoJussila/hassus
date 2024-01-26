float invLerp(float a, float b, float v)
{
    return clamp((v - a) / (b - a), 0, 1);
}
