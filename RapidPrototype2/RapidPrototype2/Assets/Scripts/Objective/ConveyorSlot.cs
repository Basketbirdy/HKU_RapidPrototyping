using UnityEngine;

[System.Serializable]
public class ConveyorSlot
{
    public ConveyorSlot(Vector2 parentPos, int index, Direction direction, float distance)
    {
        this.index = index;
        this.distance = distance;
        this.parentPos = parentPos;

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
    private Vector2 parentPos;
    private float distance;

    public void AssignSlot(GameObject obj)
    {
        slot = obj;

        Package package = obj.GetComponent<Package>();
        if (package != null) { package.AssignSlot(this); }
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

    public Vector2 GetPosition()
    {
        return parentPos + (position * distance);
    }
}

public enum Direction
{
    UP, RIGHT, DOWN, LEFT
}
