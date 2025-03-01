using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;

public class Conveyor
{
    public MonoBehaviour owner;
    private Direction conveyorDirection = Direction.LEFT;

    private int conveyorCount = 6;
    private float conveyorDistance = 6;

    private float conveyorMoveTolerance = .1f;
    private float conveyorMoveSpeed = 5f;

    public ConveyorSlot[] conveyorSlots { get; private set; }

    public Conveyor(MonoBehaviour transform, Direction direction, int count, float distance, float tolerance, float speed)
    {
        this.owner = transform;
        conveyorDirection = direction;
        conveyorCount = count;
        conveyorDistance = distance;
        conveyorMoveTolerance = tolerance;  
        conveyorMoveSpeed = speed;

        conveyorSlots = new ConveyorSlot[conveyorCount];
        for (int i = 0; i < conveyorCount; i++)
        {
            conveyorSlots[i] = new ConveyorSlot(this, owner.transform.position, i, conveyorDirection, conveyorDistance);
        }
    }

    public void Execute()
    {
        UpdateConveyors();
    }

    private void UpdateConveyors()
    {
        for (int i = conveyorCount - 1; i >= 0; i--)
        {
            if (i == 0) { continue; }

            // check if current slot is filled
            if (conveyorSlots[i].CheckSlot()) { continue; }

            // check if conveyor before is filled
            if (!conveyorSlots[i - 1].CheckSlot()) { continue; }

            // if yes, move that item to this conveyor
            GameObject itemToMove = conveyorSlots[i - 1].GetObject();

            owner.StartCoroutine(MoveItem(itemToMove, conveyorMoveSpeed, conveyorMoveTolerance, i));
        }
    }

    private IEnumerator MoveItem(GameObject itemToMove, float speed, float tolerance, int i)
    {
        bool movingItem = true;

        Vector2 target = conveyorSlots[i].GetPosition();

        while (movingItem)
        {
            if (itemToMove == null) { break; }

            itemToMove.transform.position = Vector3.MoveTowards(itemToMove.transform.position, target, speed * Time.deltaTime);

            if (itemToMove.transform.position.x <= target.x + tolerance && itemToMove.transform.position.x > target.x - tolerance &&
                itemToMove.transform.position.y <= target.y + tolerance && itemToMove.transform.position.y > target.y - tolerance)
            {
                movingItem = false;
                itemToMove.transform.position = target;
            }

            yield return null;
        }

        conveyorSlots[i - 1].EmptySlot();
        if (itemToMove != null)
        {
            conveyorSlots[i].AssignSlot(itemToMove);
        }
    }
}
