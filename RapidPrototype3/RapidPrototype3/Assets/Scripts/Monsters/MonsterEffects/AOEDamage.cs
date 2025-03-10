using UnityEngine;
using System.Collections.Generic;

public class AOEDamage : BaseMonsterEffect
{
    [SerializeField] private string effectName;
    [SerializeField] private float damage;
    [SerializeField] private float radius;
    private LayerMask hitMask;

    public AOEDamage(EffectMoment moment, float damage, LayerMask hitMask) : base(null, moment)
    {
        this.effectName = "AOEDamage";
        this.damage = damage;
        this.radius = 1;
        this.hitMask = hitMask;
    }

    public override void ApplyEffect()
    {
        InstanceChange burstChange = new InstanceChange("Burst", radius / 1.5f);
        InstanceChange outerRingChange = new InstanceChange("OuterRing", radius);
        ParticleHandler.instance.AlterEmissionRadius("LandingParticles", radius, burstChange, outerRingChange);
        ParticleHandler.instance.PlayEffectAtPosition("LandingParticles", self.transform.position);

        IDamagable[] damagables = HitCheck();
        if(damagables.Length <= 0) { return; }

        Debug.Log($"ApplyEffect; damage: {damage}");
        Debug.Log($"ApplyEffect; stats: {stats}");
        Debug.Log($"ApplyEffect; player damage from stats: {stats.GetFloatStat("damage")}");
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
        List<IDamagable> damagables = new List<IDamagable>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(self.transform.position, radius, hitMask);
        Debug.Log($"Damagable Hits length: {hits.Length}");
        if(hits.Length <= 0 ) { return damagables.ToArray(); }

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
