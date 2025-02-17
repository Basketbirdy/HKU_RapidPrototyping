using UnityEngine;

public class CreatureDetection : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask detectionMask;
    [Space]
    [SerializeField] private float detectionRadius;

    [Header("References")]
    [SerializeField] private GameObject spriteMask;

    private Vector3 cursorWorldPosition;

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cursorWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 maskPos = new Vector3(cursorWorldPosition.x, cursorWorldPosition.y, transform.position.z);
        spriteMask.transform.localPosition = maskPos;

        if (TryFindCreature())
        {
            //Debug.Log("FoundCreature");
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
