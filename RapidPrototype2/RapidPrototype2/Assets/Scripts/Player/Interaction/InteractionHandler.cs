using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionHandler : MonoBehaviour, IInteractor
{
    [Header("Settings")]
    [SerializeField] private string interactKey = "e";
    [SerializeField] private float interactionRadius;

    private CircleCollider2D interactionCollider;

    [SerializeField] private List<int> debugList = new List<int>(); 
    private List<IInteractable> interactables = new List<IInteractable>();
    public List<IInteractable> Interactables { get { return interactables; } set { interactables = value; } }

    private void Awake()
    {
        interactionCollider = GetComponent<CircleCollider2D>();
        interactionCollider.radius = interactionRadius;
        interactionCollider.isTrigger = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log("Interacting input received");
            ExecuteInteractable();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        AddInteractable(interactable);

        Debug.Log($"interactables entered trigger: {interactables.Count}");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        RemoveInteractable(interactable);

        Debug.Log($"interactables exited trigger: {interactables.Count}");
    }

    public void AddInteractable(IInteractable interactable)
    {
        if(interactable == null) { return; }

        if (interactables.Contains(interactable)) { return; }
        interactables.Add(interactable);
        debugList.Add(1);
    }

    public void RemoveInteractable(IInteractable interactable)
    {
        if(interactable == null) { return; }

        if (!interactables.Contains(interactable)) { return; }
        interactables.Remove(interactable);
        debugList.Remove(1);
    }

    public void ExecuteInteractable(int index = 0)
    {
        if(interactables.Count <= 0) { return; }
        IInteractable target = interactables[index];

        if (target == null) { return; }
        target.Interact(transform.parent.gameObject);
    }
}
