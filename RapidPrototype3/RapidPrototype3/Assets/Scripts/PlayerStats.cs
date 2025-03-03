using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    private float speed;
    private float acceleration;
    private float deceleration;
    private float friction;

    public PlayerStats(float speed, float acceleration, float deceleration, float friction)
    {
        this.speed = speed;
        this.acceleration = acceleration;
        this.deceleration = deceleration;
        this.friction = friction;
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

    public float GetSpeed() { return speed; }
    public void SetSpeed(float speed) { this.speed = speed; }

    public float GetAcceleration() { return acceleration; }
    public void SetAcceleration(float acceleration) { this.acceleration = acceleration; }

    public float GetDeceleration() { return deceleration; }
    public void SetDeceleration(float deceleration) { this.deceleration = deceleration; }

    public float GetFriction() { return friction; }
    public void SetFriction(float friction) { this.friction = friction; }

}

public struct Stat
{
    public Stat(string identifier, object value)
    {
        this.identifier = identifier;
        this.value = value;
    }

    public string identifier;
    public object value;
}
