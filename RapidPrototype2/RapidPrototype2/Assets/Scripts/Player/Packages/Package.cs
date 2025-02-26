using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Package : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] private float weight;
    [SerializeField] private float colliderRadius;

    private IInteractor lastInteractor;

    private void Awake()
    {
        CircleCollider2D coll = GetComponent<CircleCollider2D>();
        coll.radius = colliderRadius;
    }

    public void Interact(IInteractor interactor)
    {
        Debug.Log($"Interacting with {gameObject.name}");
        lastInteractor = interactor;

        EventHandler<float>.InvokeEvent(EventStrings.PLAYER_WEIGHT_ADD, weight);
    }
}
