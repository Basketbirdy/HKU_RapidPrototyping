using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Package : MonoBehaviour, IInteractable, ICarriable
{
    [Header("Settings")]
    [SerializeField] private float weight;
    [SerializeField] private float colliderRadius;

    private GameObject lastInteractor;
    private ConveyorSlot slot;

    private bool isCarried;
    [HideInInspector] public bool IsCarried { get => isCarried; set => isCarried = value; }
    [HideInInspector] public Transform CarriableTransform => transform;



    private void Awake()
    {
        CircleCollider2D coll = GetComponent<CircleCollider2D>();
        coll.radius = colliderRadius;
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log($"Interacting with {gameObject.name}");
        lastInteractor = interactor;

        ICarrier carrier = interactor.GetComponentInChildren<ICarrier>();
        if(carrier == null) { return; }

        EventHandler<float>.InvokeEvent(EventStrings.PLAYER_WEIGHT_ADD, weight);
        if(slot != null ) { slot.EmptySlot(); }

        carrier.PickUp(GetComponent<ICarriable>());
    }

    public void AssignSlot(ConveyorSlot slot)
    {
        this.slot = slot;
    }
}
