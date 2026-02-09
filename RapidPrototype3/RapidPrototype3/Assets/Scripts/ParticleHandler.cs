using UnityEngine;
using System.Collections.Generic;

public class ParticleHandler : MonoBehaviour
{
    public static ParticleHandler instance;

    public ParticleObject[] effects;

    private void Awake()
    {
        if(instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    public ParticleObject FindEffect(string name)
    {
        ParticleObject effect = new ParticleObject();
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].name == name) { effect = effects[i]; }
        }
        if (effect.name == "") { return new ParticleObject(); }
        return effect;
    }

    public void PlayEffect(string name)
    {
        ParticleObject effect = FindEffect(name);
        if(effect.name == "") { return; }
        PlayEffect(effect);
    }
    public void PlayEffect(ParticleObject effect)
    {
        for (int i = 0; i < effect.particleInstances.Length; i++)
        {
            effect.particleInstances[i].particleSystem.Stop();
            effect.particleInstances[i].particleSystem.Play();
        }
    }

    public void PlayEffectAtPosition(string name, Vector3 position)
    {
        ParticleObject effect = FindEffect(name);
        if(effect.name == "") { return; }

        effect.transform.position = position;
        PlayEffect(effect);
    }

    public void AlterEmissionRadius(string name, float radius, params InstanceChange[] changes)
    {
        ParticleObject effect = FindEffect(name);
        if (effect.name == "") { return; }

        foreach(ParticleInstance instance in effect.particleInstances)
        {
            if(changes.Length <= 0)
            {
                var shapeModule = instance.particleSystem.shape;
                shapeModule.radius = radius;
                continue;
            }

            for (int i = 0; i < changes.Length; i++)
            {
                if(instance.name == changes[i].name)
                {
                    var shapeModule = instance.particleSystem.shape;
                    shapeModule.radius = radius;
                }
                else { continue; }
            }

        }
    }
}

[System.Serializable]
public struct ParticleObject
{
    public string name;
    public Transform transform;
    [Space]
    public ParticleInstance[] particleInstances;
}

[System.Serializable]
public struct ParticleInstance
{
    public string name;
    public ParticleSystem particleSystem;
}

public struct InstanceChange
{
    public string name;
    public float value;

    public InstanceChange(string name, float value)
    {
        this.name = name;
        this.value = value;
    }
}
