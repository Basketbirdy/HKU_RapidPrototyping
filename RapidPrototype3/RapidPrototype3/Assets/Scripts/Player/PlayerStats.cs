using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerStats
{
    //private float speed;
    //private float acceleration;
    //private float deceleration;
    //private float friction;

    private Dictionary<string, float> floatStats = new Dictionary<string, float>();
    private Dictionary<string, float> floatModifier = new Dictionary<string, float>();

    public PlayerStats(params BaseStat[] stats)
    {
        //this.speed = speed;
        //this.acceleration = acceleration;
        //this.deceleration = deceleration;
        //this.friction = friction;

        foreach (FloatStat stat in stats)
        {
            floatStats.Add(stat.Identifier, stat.value);
            floatModifier.Add(stat.Identifier, 1);
        }
    }

    public void BindStats(params BaseStat[] stats)
    {
        //this.speed = speed;
        //this.acceleration = acceleration;
        //this.deceleration = deceleration;
        //this.friction = friction;

        foreach (FloatStat stat in stats)
        {
            floatStats.Add(stat.Identifier, stat.value);
            floatModifier.Add(stat.Identifier, 1);
        }
    }

    public float GetFloatStat(string identifier)
    {
        if (!floatStats.ContainsKey(identifier)) { return -1; }
        return floatStats[identifier] * floatModifier[identifier];
    }

    public void SetFloatStat(string identifier, float value)
    {
        if (!floatStats.ContainsKey(identifier)) { return; }
        floatStats[identifier] = value;
    }

    public void SetFloatModifier(string identifier, float value)
    {
        if (!floatStats.ContainsKey(identifier)) { return; }
        floatModifier[identifier] += value / 100;
    }

    //public PlayerStats(params Stat[] stats)
    //{
    //    dynamicStats.AddRange(stats);
    //}

    //public Stat GetStat(string identifier)
    //{
    //    return dynamicStats.Single<Stat>(x => x.identifier == identifier);
    //}

    //public void SetStat(string identifier, object value)
    //{
    //    Stat stat = dynamicStats.Single<Stat>(x => x.identifier == identifier);
    //    stat.value = value;
    //}

    //public List<Stat> GetAllStats()
    //{
    //    return dynamicStats;
    //}

    //public float GetSpeed() { return speed; }
    //public void SetSpeed(float speed) { this.speed = speed; }

    //public float GetAcceleration() { return acceleration; }
    //public void SetAcceleration(float acceleration) { this.acceleration = acceleration; }

    //public float GetDeceleration() { return deceleration; }
    //public void SetDeceleration(float deceleration) { this.deceleration = deceleration; }

    //public float GetFriction() { return friction; }
    //public void SetFriction(float friction) { this.friction = friction; }

}

public abstract class BaseStat
{
    public string Identifier { get; private set; }

    public BaseStat(string identifier)
    {
        Identifier = identifier;
    }
}
