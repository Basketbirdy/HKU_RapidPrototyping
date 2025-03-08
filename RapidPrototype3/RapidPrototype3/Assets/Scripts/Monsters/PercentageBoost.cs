using UnityEngine;

[System.Serializable]
public class PercentageBoost : BaseMonsterEffect
{
    [SerializeField] private float boost;
    [SerializeField] private string statIdentifier;

    public PercentageBoost(float boost, string statIdentifier) : base(null)
    {
        this.boost = boost;
        this.statIdentifier = statIdentifier;
    }

    public override void ApplyEffect()
    {
        Debug.Log($"Applying monster effect; id: {statIdentifier}; boost: {boost}");
        stats.SetFloatModifier(statIdentifier, boost);
    }

    public override void RemoveEffect()
    {
        Debug.Log($"Removing monster effect; id: {statIdentifier}; boost: {-boost}");
        stats.SetFloatModifier(statIdentifier, -boost);
    }
}
