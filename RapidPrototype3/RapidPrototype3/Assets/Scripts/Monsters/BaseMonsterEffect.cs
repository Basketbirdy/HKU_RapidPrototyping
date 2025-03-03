using UnityEngine;

[System.Serializable]
public abstract class BaseMonsterEffect
{
    protected PlayerStats stats;

    public void SetupStats(PlayerStats stats) { this.stats = stats; }
    public abstract void ApplyEffect();
    public abstract void RemoveEffect();
}
