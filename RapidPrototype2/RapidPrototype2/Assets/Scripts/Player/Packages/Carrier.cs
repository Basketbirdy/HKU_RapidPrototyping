using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carrier : MonoBehaviour, ICarrier
{
    [SerializeField] private string dropKey = "q";
    [Space]
    [SerializeField] private float pickupSpeed;
    [SerializeField] private float pickupTolerance;
    [SerializeField] private Vector2 offsetRange;
    [Space]
    [SerializeField] private float dropAngle = 22.5f;
    [SerializeField] private Vector2 dropRange;
    [SerializeField] private LayerMask dropMask;
    private int dropAttempts;
    private int maxDropAttempts = 160;

    public Transform CarryPoint => transform;

    private List<ICarriable> carriables = new List<ICarriable>();
    public List<ICarriable> Carriables { get { return carriables; } set { carriables = value; } }

    private Queue<ICarriable> carriableQueue = new Queue<ICarriable>();
    public Queue<ICarriable> CarriableQueue { get { return carriableQueue; } set { carriableQueue = value; } }

    private Action<ICarriable> awaitMove;
    private bool awaitingMove;

    private Vector2[] directions;

    private void Start()
    {
        directions = new Vector2[maxDropAttempts];
    }

    private void Update()
    {
        if (Input.GetKeyDown(dropKey))
        {
            Drop();
        }
    }

    public void Drop()
    {
        if (awaitingMove) { Debug.LogWarning($"Currently awaiting other move! cancelling drop"); return; }
        Debug.Log("Dropping carriable");

        if(carriables.Count == 0) { Debug.Log($"No carriables to drop found! Returning"); return; }

        Vector2 dropDirection = Vector2.zero;
        bool foundDirection = false;
        // find position to drop
        while(!foundDirection)
        {
            float angle = dropAngle * -dropAttempts;
            Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.one;
            Debug.Log($"direction: {direction}; magnitude: {direction.magnitude}");
            //directions[i] = direction;

            bool obstacle = Physics2D.Raycast(transform.position, direction, dropRange.y, dropMask);
            if (!obstacle) 
            {
                dropDirection = direction.normalized;
                foundDirection = true;
                dropAttempts = 0;
                break;
            }

            if(dropAttempts >= maxDropAttempts && !foundDirection) 
            {
                Debug.LogWarning("No valid drop direction found! cancelling drop");
                foundDirection = true;
                return;
            }
            dropAttempts++;
        }

        // check for drop direction
        if(dropDirection == Vector2.zero) { Debug.Log("No drop direction found! Returning"); return; }
        // find random distance to drop item
        float distance = UnityEngine.Random.Range(dropRange.x, dropRange.y);
        // find position to drop to
        Vector2 transformPosV2 = new Vector2(transform.position.x, transform.position.y);
        Vector2 dropPosition = transformPosV2 + (dropDirection * distance);
        // find what carriable to drop
        ICarriable carriableToDrop = carriableQueue.Peek();

        awaitMove += EndDrop;

        StartCoroutine(MoveTowardsTarget(carriableToDrop, dropPosition, pickupSpeed, pickupTolerance));
    }

    public void Drop(Vector3 target, Action action)
    {
        if (awaitingMove) { Debug.LogWarning($"Currently awaiting other move! cancelling drop"); return; }
        Debug.Log("Dropping carriable");

        if (carriables.Count == 0) { Debug.Log($"No carriables to drop found! Returning"); return; }

        Vector2 dropPosition = target;
        // find what carriable to drop
        ICarriable carriableToDrop = carriableQueue.Peek();

        awaitMove += EndDrop;

        StartCoroutine(MoveTowardsTarget(carriableToDrop, dropPosition, pickupSpeed, pickupTolerance, action));
    }

    public void PickUp(ICarriable carriable)
    {
        if (awaitingMove) { Debug.LogWarning($"Currently awaiting other move! cancelling pickup"); return; }

        Debug.Log($"Picking up carriable");

        if (!carriables.Contains(carriable)) { carriables.Add(carriable); }
        if(!carriableQueue.Contains(carriable)) { carriableQueue.Enqueue(carriable); }

        awaitMove += EndPickUp;

        StartCoroutine(MoveTowardsTarget(carriable, CarryPoint, pickupSpeed, pickupTolerance));
    }

    private void EndPickUp(ICarriable carriable)
    {
        awaitMove -= EndPickUp;
        awaitingMove = false;
        carriable.IsCarried = true;

        carriable.CarriableTransform.parent = transform;
    }

    private void EndDrop(ICarriable carriable)
    {
        awaitMove -= EndDrop;
        awaitingMove = false;
        carriable.IsCarried = false;

        carriable.CarriableTransform.parent = null;

        if (carriables.Contains(carriable))
        {
            carriables.Remove(carriable);
            carriableQueue.Dequeue();
        }
        else
        {
            Debug.LogError("Something went wrong! throwing error", gameObject);
        }

        EventHandler<float>.InvokeEvent(EventStrings.PLAYER_WEIGHT_REMOVE, carriable.Weight);
    }

    private IEnumerator MoveTowardsTarget(ICarriable carriable, Transform target, float speed, float tolerance)
    {
        bool movingItem = true;

        while (movingItem)
        {
            if (carriable == null) { break; }

            carriable.CarriableTransform.position = Vector3.MoveTowards(carriable.CarriableTransform.position, target.position, speed * Time.deltaTime);

            if (carriable.CarriableTransform.position.x <= target.position.x + tolerance && carriable.CarriableTransform.position.x > target.position.x - tolerance &&
                carriable.CarriableTransform.position.y <= target.position.y + tolerance && carriable.CarriableTransform.position.y > target.position.y - tolerance)
            {
                movingItem = false;
                carriable.CarriableTransform.position = target.position + new Vector3(UnityEngine.Random.Range(-offsetRange.x, offsetRange.x), 
                                                                                        UnityEngine.Random.Range(-offsetRange.y, offsetRange.y));
            }

            yield return null;
        }

        awaitMove?.Invoke(carriable);
    }

    private IEnumerator MoveTowardsTarget(ICarriable carriable, Vector3 target, float speed, float tolerance, Action action = null)
    {
        bool movingItem = true;

        while (movingItem)
        {
            if (carriable == null) { break; }

            carriable.CarriableTransform.position = Vector3.MoveTowards(carriable.CarriableTransform.position, target, speed * Time.deltaTime);

            if (carriable.CarriableTransform.position.x <= target.x + tolerance && carriable.CarriableTransform.position.x > target.x - tolerance &&
                carriable.CarriableTransform.position.y <= target.y + tolerance && carriable.CarriableTransform.position.y > target.y - tolerance)
            {
                movingItem = false;
                carriable.CarriableTransform.position = target + new Vector3(UnityEngine.Random.Range(-offsetRange.x, offsetRange.x),
                                                                                        UnityEngine.Random.Range(-offsetRange.y, offsetRange.y));
            }

            yield return null;
        }

        if (action != null) { action?.Invoke(); }
        awaitMove?.Invoke(carriable);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    foreach(Vector2 dir in directions)
    //    {
    //        Gizmos.DrawRay(transform.position, dir * dropRange.y);
    //    }
    //}
}
