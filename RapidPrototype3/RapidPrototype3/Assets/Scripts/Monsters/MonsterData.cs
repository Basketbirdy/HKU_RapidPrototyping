using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    [Header("Description")]
    public string monsterName;
    public string description;

    [Header("Settings")]
    [Header("Movement")]
    public float speed;

    [Header("Offense")]
    public float damage;
    public LayerMask hitMask; // the layers that will get hit when the monster lands
    
    [Header("Defense")]
    public float health;

    [Header("Other")]
    [SerializeReference] public List<BaseMonsterEffect> holdEffects = new List<BaseMonsterEffect>();

    public MonsterData(MonsterData defaultData)
    {
        monsterName = defaultData.monsterName;
        description = defaultData.description;

        speed = defaultData.speed;
        damage = defaultData.damage;
        hitMask = defaultData.hitMask;

        health = defaultData.health;

        foreach(BaseMonsterEffect effect in defaultData.holdEffects)
        {
            holdEffects.Add(effect.Clone());
        }
    }

    public void Setup(PlayerStats stats, GameObject self)
    {
        foreach (BaseMonsterEffect effect in holdEffects)
        {
            effect.Setup(stats, self);
        }
    }

    // add types of boosts
    [ContextMenu("Stats/Float/PercentageBoost")]
    public void AddPercentageBoost()
    {
        holdEffects.Add(new PercentageBoost(EffectMoment.ONCARRY, "NewPercentageBoost", 0));
    }

    [ContextMenu("Damaging/HurtSelf")]
    public void AddHurtSelf()
    {
        holdEffects.Add(new HurtSelf(EffectMoment.ONLANDING, damage));
    }

    [ContextMenu("Damaging/AOE/AOEDamage/OnLanding")]
    public void AddAOEDamageOnLand()
    {
        holdEffects.Add(new AOEDamage(EffectMoment.ONLANDING, damage, hitMask));
    }

    [ContextMenu("Damaging/AOE/AOEDamage/OnCarry")]
    public void AddAOEDamageOnCarry()
    {
        holdEffects.Add(new AOEDamage(EffectMoment.ONCARRY, damage, hitMask));
    }
}
