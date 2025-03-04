using UnityEngine;

public interface IPhysicsMover2D
{
    public Transform PhysicsTransform { get; }
    public void AddForce(Vector2 direction, float strength, ForceMode2D mode = ForceMode2D.Force);
}
