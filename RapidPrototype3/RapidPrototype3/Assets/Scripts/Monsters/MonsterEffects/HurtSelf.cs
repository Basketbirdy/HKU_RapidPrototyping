using Unity.VisualScripting;
using UnityEngine;

public class HurtSelf : BaseMonsterEffect
{
    [SerializeField] private string effectName;
    [SerializeField] private float damage;

    public HurtSelf(EffectMoment moment, float damage) : base(null, moment) 
    {
        effectName = "HurtSelf";
        this.damage = damage; 
    }

    public override void ApplyEffect()
    {
        IDamagable damagable = self.GetComponent<IDamagable>();
        if(damagable == null ) { return; }

        damagable.TakeDamage(damage);
    }

    public override void RemoveEffect()
    {
    }
}
