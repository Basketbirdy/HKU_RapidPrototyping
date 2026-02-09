using UnityEngine;

[System.Serializable]
public class FloatStat : BaseStat
{
    public FloatStat(string identifier, float value) : base(identifier) { this.value = value; }

    public float value;
}