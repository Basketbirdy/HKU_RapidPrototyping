using UnityEngine;
using UnityEngine.Rendering;

public class CreatureDetection : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private LayerMask visibilityMask;
    [Space]
    [SerializeField] private float detectionRadius;

    [Header("References")]
    [SerializeField] private GameObject spriteMask;

    private Vector3 cursorWorldPosition;
    private Ray ray;

    private void Awake()
    {
        
    }

    private void Start()
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
            Debug.Log($"Creature is visible");
        }
    }

    private bool TryFindCreature()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(cursorWorldPosition, detectionRadius, transform.forward, Mathf.Infinity, detectionMask);

        if (hits.Length == 0) { return false; }

        //Debug.Log($"Creatures hit count: {hits.Length}");
        for(int i = 0; i < hits.Length; i++)
        {
            bool covered = Physics.Raycast(hits[i].transform.position, -transform.forward, Mathf.Infinity);

            if (covered) { continue; }

            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(cursorWorldPosition, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(ray);
    }
}
