using UnityEngine;

public class DragableObject : MonoBehaviour, IDragable
{
    public void OnDrag(Vector2 pos)
    {
        // transform into relative position

        Vector3 newPos = new Vector3(pos.x, pos.y, transform.position.z);
        transform.position = newPos;
    }


}


