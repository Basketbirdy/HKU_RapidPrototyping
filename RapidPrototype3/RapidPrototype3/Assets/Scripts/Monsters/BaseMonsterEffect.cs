using UnityEngine;

public enum EffectMoments { ONCARRY, ONLANDING }

[System.Serializable]
public abstract class BaseMonsterEffect
{
    public BaseMonsterEffect(PlayerStats playerStats) 
    { 
        stats = playerStats;
    }

    protected PlayerStats stats;

    public EffectMoments EffectMoments { get; protected set; }
    public void SetupStats(PlayerStats stats) { this.stats = stats; }
    public abstract void ApplyEffect();
    public abstract void RemoveEffect();
}
