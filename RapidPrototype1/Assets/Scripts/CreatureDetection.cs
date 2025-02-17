using UnityEngine;

public class CreatureDetection : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask detectionMask;
    [Space]
    [SerializeField] private float detectionRadius;

    private Vector3 cursorWorldPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cursorWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (TryFindCreature())
        {
            Debug.Log("FoundCreature");
        }
    }

    private bool TryFindCreature()
    {
        RaycastHit2D hit = Physics2D.CircleCast(cursorWorldPosition, detectionRadius, transform.forward);

        if (hit.collider == null) { return false; }

        if (hit.collider.CompareTag("Creature"))
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(cursorWorldPosition, detectionRadius);
    }
}
