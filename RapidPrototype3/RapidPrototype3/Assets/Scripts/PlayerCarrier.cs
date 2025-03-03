using System;
using System.Collections;
using UnityEngine;

public class PlayerCarrier : MonoBehaviour, ICarrier
{
    [Header("Settings")]
    [Header("Pickup")]
    [SerializeField] private float pickupSpeed = 100f;
    [SerializeField] private float pickupTolerance = .1f;

    [Header("Throw")]
    [SerializeField] private string throwKey = "q";
    [SerializeField] private AnimationCurve throwCurve;
    [SerializeField] private float throwChargeSpeed;
    [SerializeField] private float maxThrowCharge;
    [SerializeField] private float minThrowCharge;
    [Space]
    [SerializeField] private float currentThrowCharge;
    [SerializeField] private float throwChargeRatio;
    private bool isCharging = false;
    private Coroutine chargeCoroutine;

    [HideInInspector] public Transform CarryPoint => transform;

    private ICarriable currentCarriable;
    public ICarriable Carriable { get => currentCarriable; set => currentCarriable = value; }

    private Action<ICarriable> awaitMove;
    private bool awaitingMove = false;

    private void Update()
    {
        if(currentCarriable == null) { return; }

        if (Input.GetKeyDown(throwKey))
        {
            ChargeThrow();
        }

        if(Input.GetKeyUp(throwKey))
        {
            ReleaseThrow();
        }
    }

    public void Drop()
    {
    }

    public void PickUp(ICarriable carriable)
    {
        if (awaitingMove) { return; }
        if (currentCarriable != null) { return; } // check if not already holding a carriable

        //Debug.Log($"Picking up carriable: {carriable.CarriableTransform.gameObject.name}", gameObject);
        currentCarriable = carriable;
        carriable.IsCarried = true;

        awaitMove += EndPickup;
        awaitingMove = true;
        StartCoroutine(MoveTowardsTarget(currentCarriable, CarryPoint, Vector3.zero, pickupSpeed, pickupTolerance));
    }

    private void EndPickup(ICarriable carriable)
    {
        awaitMove -= EndPickup;
        awaitingMove = false;

        carriable.CarriableTransform.parent = CarryPoint;

        carriable.OnCarry();
    }

    public void Throw()
    {
        // TODO - throwing
        // TODO - Remap currentThrowCharge to a value between 1 and 0
        throwChargeRatio = MathUtils.Remap(currentThrowCharge, minThrowCharge, maxThrowCharge);
        Debug.Log($"Throwing with a charge of {currentThrowCharge} and ratio of: {throwChargeRatio}");
    }

    public void ChargeThrow()
    {
        chargeCoroutine = StartCoroutine(StartCharging());
    }

    public void ReleaseThrow()
    {
        if(chargeCoroutine != null) 
        { 
            StopCoroutine(chargeCoroutine); 
            chargeCoroutine = null;
        }

        if(currentThrowCharge < minThrowCharge) { return; }
        Throw();
    }

    private IEnumerator StartCharging()
    {
        currentThrowCharge = 0;

        while (currentThrowCharge < maxThrowCharge)
        {
            currentThrowCharge += throwChargeSpeed * Time.deltaTime;
            yield return null;
        }

        currentThrowCharge = maxThrowCharge;
        isCharging = false;
    }

    private IEnumerator MoveTowardsTarget(ICarriable carriable, Transform target, Vector3 offset, float speed, float tolerance)
    {
        bool movingItem = true;

        while (movingItem)
        {
            if (carriable == null) { break; }

            carriable.CarriableTransform.position = Vector2.MoveTowards(carriable.CarriableTransform.position, target.position, speed * Time.deltaTime);

            if (carriable.CarriableTransform.position.x <= target.position.x + tolerance && carriable.CarriableTransform.position.x > target.position.x - tolerance &&
                carriable.CarriableTransform.position.y <= target.position.y + tolerance && carriable.CarriableTransform.position.y > target.position.y - tolerance)
            {
                movingItem = false;
                carriable.CarriableTransform.position = target.position + offset;
            }

            yield return null;
        }

        awaitMove?.Invoke(carriable);
    }
}
