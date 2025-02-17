using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Creature
{
    public Creature(string name, float x, float y, GameObject prefab)
    {
        this.name = name;
        this.x = x;
        this.y = y;
        this.prefab = prefab;
    }

    public string name;
    public float x;
    public float y;
    public GameObject prefab;
}
