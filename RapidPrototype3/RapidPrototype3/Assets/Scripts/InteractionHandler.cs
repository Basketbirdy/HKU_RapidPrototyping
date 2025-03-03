using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private string interactionKey = "e";
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask interactionMask;

    [Header("Debugging")]
    [SerializeField] private bool showRadius;

    private void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            Debug.Log($"Received interaction input; Key: {interactionKey}");
            TryInteract();
        }
    }

    public void TryInteract()
    {
        // check radius for interactables
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactionMask);

        // check for hits
        if(hits.Length == 0) 
        {
            Debug.Log("No hits found"); 
            return; 
        }
        
        // get closest hit
        Collider2D closest = hits[0];
        float closestDistance = 999999;
        foreach (Collider2D hit in hits) 
        {
            float distance = Vector2.Distance(hit.transform.position, transform.position);
            if(distance < closestDistance) 
            {
                closestDistance = distance; 
                closest = hit; 
            }
        }

        IInteractable interactable = closest.gameObject.GetComponent<IInteractable>();
        if(interactable != null) { interactable.Interact(this.gameObject); }
        else { Debug.Log($"Interactable is null?!"); }
    }

    private void OnDrawGizmos()
    {
        if (!showRadius) { return; }
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
