using UnityEngine;
using System.Collections.Generic;

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
    
    [Header("Defense")]
    public float health;

    [Header("Other")]
    [SerializeReference] public List<BaseMonsterEffect> holdEffects;

    public void SetupStats(PlayerStats stats)
    {
        foreach (BaseMonsterEffect effect in holdEffects)
        {
            effect.SetupStats(stats);
        }
    }

    // add types of boosts
    [ContextMenu("Add PercentageBoost")]
    public void AddPercentageBoost()
    {
        holdEffects.Add(new PercentageBoost());
    }
}
