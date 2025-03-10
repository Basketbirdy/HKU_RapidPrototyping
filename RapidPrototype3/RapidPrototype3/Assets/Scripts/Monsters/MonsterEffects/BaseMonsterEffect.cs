using UnityEngine;

public enum EffectMoment { ONCARRY, ONLANDING }

[System.Serializable]
public abstract class BaseMonsterEffect
{
    public BaseMonsterEffect(PlayerStats playerStats, EffectMoment moment) 
    { 
        stats = playerStats;
        EffectMoment = moment;
    }

    protected PlayerStats stats;
    protected GameObject self;

    public EffectMoment EffectMoment { get; private set; }
    public void Setup(PlayerStats stats, GameObject self) 
    { 
        this.stats = stats; 
        this.self = self;
    }
    public abstract void ApplyEffect();
    public abstract void RemoveEffect();
    public virtual BaseMonsterEffect Clone()
    {
        return (BaseMonsterEffect)this.MemberwiseClone(); // Creates a shallow copy
    }
}
