using UnityEngine;

public class Monster : MonoBehaviour, IInteractable, ICarriable
{
    [SerializeField] private float speed;

    [SerializeField] private Transform target;

    public Transform CarriableTransform => transform;

    private bool isCarried;
    public bool IsCarried { get => isCarried; set => isCarried  = value; }

    public void Interact(GameObject interactor)
    {
        Debug.Log($"Interacting with: {gameObject.name}");
        ICarrier carrier = interactor.GetComponentInChildren<ICarrier>();
        if (carrier != null) { carrier.PickUp(this); }
    }

    private void Update()
    {
        if (IsCarried) { return; }
        Move();
    }

    private void Move()
    {
        float step = Time.deltaTime * speed;
        Vector2 newPos = Vector2.MoveTowards(transform.position, target.position, step);
        transform.position = newPos;
        //Debug.Log($"New position: {newPos}: Step: {step}", gameObject);
    }

    public void OnCarry()
    {
        
    }

    public void OnThrow()
    {
    }
}
