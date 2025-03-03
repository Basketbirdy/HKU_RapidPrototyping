using UnityEngine;

[System.Serializable]
public class PercentageBoost : BaseMonsterEffect
{
    [SerializeField][Range(-200, 200)] private float boost;
    [SerializeField] private string statIdentifier;

    public PercentageBoost() { }

    public override void ApplyEffect()
    {
        
    }

    public override void RemoveEffect()
    {

    }
}
