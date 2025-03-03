using UnityEditor;
using UnityEngine;

public class PatrolBetweenPoints : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private float tolerance = .1f;

    [SerializeField] private Transform[] patrolPoints;

    [Header("Debugging")]
    [SerializeField] private Color gizmoColor; 
    private int currentIndex;
    private Vector3 currentTarget;

    private void Start()
    {
        currentIndex = 0;
        transform.position = patrolPoints[currentIndex].position;
    }

    private void Update()
    {
        Vector3 target = patrolPoints[currentIndex].position;

        float step = speed * Time.deltaTime;
        Vector3 newPos = Vector3.MoveTowards(transform.position, target, step);
        transform.position = newPos;
        if (transform.position.x <= target.x + tolerance && transform.position.x > target.x - tolerance &&
            transform.position.y <= target.y + tolerance && transform.position.y > target.y - tolerance)
        {
            AdvanceIndex();
        }

        //if(transform.position == currentTarget)
        //{
        //    AdvanceIndex();
        //}

    }

    private void AdvanceIndex()
    {
        if(currentIndex + 1 == patrolPoints.Length)
        {
            currentIndex = 0;
        }
        else { currentIndex++; }

        Debug.Log("New patrol index" + currentIndex);
    }

    private void OnDrawGizmos()
    {
        if (patrolPoints.Length <= 0) { return; }

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Vector3 pos = patrolPoints[i].position;
            pos.z = -5;

            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(pos, .3f);

            pos.x -= 2.5f;
            pos.y += 1;

            Gizmos.color = gizmoColor;
            Handles.Label(pos, gameObject.name + "; " + i.ToString());
        }
    }
}
