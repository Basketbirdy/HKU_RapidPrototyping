using UnityEngine;

[System.Serializable]
public class ConveyorSlot
{
    public ConveyorSlot(int index, Direction direction, float distance)
    {
        this.index = index;
        this.distance = distance;

        switch (direction)
        {
            case Direction.UP:
                position = new Vector2(0, index);
                break;
            case Direction.DOWN:
                position = new Vector2(0, -index);
                break;
            case Direction.LEFT:
                position = new Vector2(-index, 0);
                break;
            case Direction.RIGHT:
                position = new Vector2(index, 0);
                break;
        }
    }

    [Header("Identifier")]
    private int index;

    [Header("Data")]
    [SerializeField] private GameObject slot;
    private Vector2 position;
    private float distance;

    public void AssignSlot(GameObject obj)
    {
        slot = obj;

        Package package = obj.GetComponent<Package>();
        if (package != null) { package.AssignSlot(this); }

        slot.transform.position = position * distance;
    }

    public void EmptySlot()
    {
        slot = null;
    }

    public bool CheckSlot()
    {
        if (slot == null) { return false; }
        return true;
    }

    public GameObject GetObject()
    {
        return slot;
    }
}

public enum Direction
{
    UP, RIGHT, DOWN, LEFT
}
