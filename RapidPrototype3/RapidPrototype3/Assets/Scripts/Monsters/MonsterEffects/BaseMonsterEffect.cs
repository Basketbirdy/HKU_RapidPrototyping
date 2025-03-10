using UnityEngine;
using System;

public enum EffectMoment { ONCARRY, ONLANDING }

[System.Serializable]
public abstract class BaseMonsterEffect
{
    public BaseMonsterEffect(PlayerStats playerStats, EffectMoment moment) 
    { 
        stats = playerStats;
        effectMoment = moment;
        //EffectMoment = moment;
        Debug.Log($"EffectMoment; {effectMoment}");
    }

    protected PlayerStats stats;
    protected GameObject self;

    public EffectMoment effectMoment;

    public void Setup(PlayerStats stats, GameObject self) 
    { 
        this.stats = stats; 
        this.self = self;
        Debug.Log($"BaseMonsterEffect; effectmoment: {effectMoment}");
    }
    public abstract void ApplyEffect();
    public abstract void RemoveEffect();

    public virtual BaseMonsterEffect Clone()
    {
        return (BaseMonsterEffect)this.MemberwiseClone(); // Creates a shallow copy
    }
}
