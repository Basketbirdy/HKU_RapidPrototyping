using UnityEngine;

public class PhysicsMovement
{
    public System.Action onMovingChange;

    private bool isDisabled;
    private bool isMoving;

    private Rigidbody2D rb;

    public PhysicsMovement(Rigidbody2D rb)
    {
        this.rb = rb;
    }

    public virtual void Move(Vector2 direction, float acceleration, float speed)
    {
        if (isDisabled) { return; }

        if(direction != Vector2.zero) { ChangeMoving(true); }
        else { ChangeMoving(false); }

        if (!isMoving) { return; }

        rb.AddForce(direction * acceleration, ForceMode2D.Force);
        if (rb.linearVelocity.magnitude >= speed) { rb.linearVelocity = rb.linearVelocity.normalized * speed; } // limit velocity
    }

    public virtual void UpdatePhysics(float friction, float deceleration)
    {
        if (isMoving) { rb.linearDamping = friction; }
        else { rb.linearDamping = deceleration; }
    }

    public void EnableMovement() { isDisabled = true; }
    public void DisableMovement() { isDisabled = false; }

    public bool GetMoving()
    {
        return isMoving;
    }

    private void ChangeMoving(bool state)
    {
        isMoving = state;
        onMovingChange?.Invoke();
    }
}
