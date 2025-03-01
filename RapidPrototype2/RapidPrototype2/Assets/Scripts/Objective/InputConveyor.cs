using System;
using System.Threading;
using UnityEngine;

public class InputConveyor : MonoBehaviour, IInteractable
{
    [Header("Conveyor settings")]
    [SerializeField] private Direction conveyorDirection = Direction.LEFT;

    [SerializeField] private int conveyorCount = 6;
    [SerializeField] private float conveyorDistance = 6;

    [SerializeField] private float conveyorMoveTolerance = .1f;
    [SerializeField] private float conveyorMoveSpeed = 5f;

    // private 
    private Conveyor conveyor;
    private ConveyorSlot closestConveyor;
    private ICarrier carrier;

    private Action awaitDrop;

    private void Awake()
    {
        conveyor = new Conveyor(this, conveyorDirection, conveyorCount, conveyorDistance, conveyorMoveTolerance, conveyorMoveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        conveyor.Execute();
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log("Input conveyor interacted with");

        carrier = interactor.GetComponentInChildren<ICarrier>();
        if(carrier == null) { return; }
        if(carrier.Carriables.Count == 0) { return; }

        // get drop position
        float closestDistance = 9999;
        foreach(ConveyorSlot slot in conveyor.conveyorSlots)
        {
            float distance = Vector2.Distance(slot.GetPosition(), interactor.transform.position);
            if(distance < closestDistance) 
            { 
                closestDistance = distance;
                closestConveyor = slot;
            }
        }

        if(closestConveyor == null) { return; }
        if (closestConveyor.CheckSlot())
        {
            Debug.LogWarning("Closest slot is already filled! Returning");
            return;
        }

        awaitDrop += EndDrop;
        carrier.Drop(closestConveyor.GetPosition(), awaitDrop);
    }

    private void EndDrop()
    {
        awaitDrop -= EndDrop;

        // this should only be done when drop is complete
        closestConveyor.AssignSlot(carrier.CarriableQueue.Peek().CarriableTransform.gameObject);

        closestConveyor = null;
        carrier = null;
    }
}
