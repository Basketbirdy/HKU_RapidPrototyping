using UnityEngine;
using System.Collections.Generic;

public class AOEDamage : BaseMonsterEffect
{
    [SerializeField] private string effectName;
    [SerializeField] private float damage;
    [SerializeField] private float radius;

    public AOEDamage(EffectMoment moment, float damage) : base(null, moment)
    {
        this.effectName = "AOEDamage";
        this.damage = damage;
        this.radius = 1;
    }

    public override void ApplyEffect()
    {
        IDamagable[] damagables = HitCheck();
        if(damagables.Length <= 0) { return; }

        float finalDamage = damage + stats.GetFloatStat("damage");
        foreach (IDamagable damagable in damagables)
        {
            if (damagable.IsInvincible) { continue; }
            damagable.TakeDamage(finalDamage);
        }
    }

    public override void RemoveEffect()
    {

    }

    private IDamagable[] HitCheck()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(self.transform.position, radius);
        if(hits.Length <= 0 ) { return null; }

        List<IDamagable> damagables = new List<IDamagable>();
        foreach(Collider2D hit in hits)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if(damagable == null) { continue; }
            if(damagable == self.GetComponent<IDamagable>()) { continue; }

            if (!damagables.Contains(damagable))
            {
                damagables.Add(damagable);
            }
        }
        return damagables.ToArray();
    }
}
