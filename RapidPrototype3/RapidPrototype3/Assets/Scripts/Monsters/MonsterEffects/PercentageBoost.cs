using UnityEngine;

[System.Serializable]
public class PercentageBoost : BaseMonsterEffect
{
    [SerializeField] private string effectName;
    [SerializeField] private string statIdentifier;
    [SerializeField] private float boost;

    public PercentageBoost(EffectMoment moment, string statIdentifier, float boost) : base(null, moment)
    {
        effectName = "PercentageBoost";
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
