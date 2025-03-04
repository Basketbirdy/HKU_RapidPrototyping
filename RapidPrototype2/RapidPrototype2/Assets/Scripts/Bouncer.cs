using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField] private float force;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Entered collision with: {collision.gameObject.name}");

        IPhysicsMover2D physicsMover = collision.gameObject.GetComponent<IPhysicsMover2D>();
        if(physicsMover == null) { return; }

        Debug.Log($"Found physicsMover");

        Vector2 actor = physicsMover.PhysicsTransform.position;
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = actor - pos;

        Debug.Log($"Adding force of: {direction.normalized * force}, to {collision.gameObject.name}");
        physicsMover.AddForce(direction.normalized, force, ForceMode2D.Impulse);
    }
}
