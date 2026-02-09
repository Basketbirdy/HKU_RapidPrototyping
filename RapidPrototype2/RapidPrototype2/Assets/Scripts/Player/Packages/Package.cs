using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Package : MonoBehaviour, IInteractable, ICarriable
{
    [Header("Settings")]
    [SerializeField] private float weight;
    [SerializeField] private float colliderRadius;

    private GameObject lastInteractor;
    private ConveyorSlot slot;

    private CircleCollider2D coll;

    private bool isCarried;
    [HideInInspector] public bool IsCarried { get => isCarried; 
        set 
        { 
            isCarried = value;
            coll.enabled = !value;
        } 
    }

    [HideInInspector] public Transform CarriableTransform => transform;
    [HideInInspector] public float Weight => weight;

    private void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
        coll.radius = colliderRadius;
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log($"Interacting with {gameObject.name}");
        lastInteractor = interactor;

        ICarrier carrier = interactor.GetComponentInChildren<ICarrier>();
        if(carrier == null || carrier.AwaitingMove) { return; }

        EventHandler<float>.InvokeEvent(EventStrings.PLAYER_WEIGHT_ADD, weight);
        if(slot != null ) { slot.EmptySlot(); slot.StopCoroutines(); }

        carrier.PickUp(GetComponent<ICarriable>());
    }

    public void AssignSlot(ConveyorSlot slot)
    {
        this.slot = slot;
    }
}
