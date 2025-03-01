using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour, ICarrier
{
    [SerializeField] private float pickupSpeed;
    [SerializeField] private float pickupTolerance;
    [Space]
    [SerializeField] private Vector2 offsetRange;

    public Transform CarryPoint => transform;

    private List<ICarriable> carriables = new List<ICarriable>();
    public List<ICarriable> Carriables { get { return carriables; } set { carriables = value; } }

    private Queue<ICarriable> carriableQueue = new Queue<ICarriable>();
    public Queue<ICarriable> CarriableQueue { get { return carriableQueue; } set { carriableQueue = value; } }

    private Action<ICarriable> awaitMove;

    public void Drop(ICarriable carriable)
    {

    }

    public void PickUp(ICarriable carriable)
    {
        Debug.Log($"Picking up carriable");

        if (!carriables.Contains(carriable)) { carriables.Add(carriable); }
        if(!carriableQueue.Contains(carriable)) { carriableQueue.Enqueue(carriable); }

        awaitMove += EndPickUp;
        StartCoroutine(MoveTowardsTarget(carriable, CarryPoint, pickupSpeed, pickupTolerance));
    }

    private void EndPickUp(ICarriable carriable)
    {
        awaitMove -= EndPickUp;
        carriable.IsCarried = true;

        carriable.CarriableTransform.parent = transform;
        // reorder pickup items
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
}
