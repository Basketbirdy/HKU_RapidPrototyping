using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CreatureDetection : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private LayerMask visibilityMask;
    [Space]
    [SerializeField] private float detectionRadius;
    [Space]
    [SerializeField] private float creatureGracePeriod = 3f;

    [Header("References")]
    [SerializeField] private GameObject spriteMask;

    private Vector3 cursorWorldPosition;
    private Ray ray;

    private Coroutine creatureTimer;

    [Header("Debug")]
    [SerializeField] private float elapsedTime;

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
            //Debug.Log($"Creature is visible");
            if(creatureTimer == null)
            {
                creatureTimer = StartCoroutine(CreatureTimer(creatureGracePeriod));
            }
        }
        else
        {
            if(creatureTimer != null)
            {
                StopCoroutine(creatureTimer);
                creatureTimer = null;
            }
        }
    }

    private bool TryFindCreature()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(cursorWorldPosition, detectionRadius, transform.forward, Mathf.Infinity, detectionMask);

        if (hits.Length == 0) { return false; }

        //Debug.Log($"Creatures hit count: {hits.Length}");
        for(int i = 0; i < hits.Length; i++)
        {
            IScratchable scratchable = hits[i].collider.GetComponent<IScratchable>();
            if(scratchable != null) 
            {
                if (scratchable.IsScratched) { continue; }
            }

            bool covered = Physics.Raycast(hits[i].transform.position, -transform.forward, Mathf.Infinity);

            if (covered) { continue; }

            return true;
        }

        return false;
    }

    private IEnumerator CreatureTimer(float duration)
    {
        elapsedTime = 0;

        while(elapsedTime < duration) 
        { 
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // advance creature state
        EventHandler.InvokeEvent(EventTypes.CREATURE_STAREDAT);
        creatureTimer = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(cursorWorldPosition, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(ray);
    }
}
