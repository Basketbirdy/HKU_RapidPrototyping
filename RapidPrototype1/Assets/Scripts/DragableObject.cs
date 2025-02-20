using Unity.VisualScripting;
using UnityEngine;

public class DragableObject : MonoBehaviour, IDragable
{
    [SerializeField] private string pickupSound = null;

    [SerializeField] private bool isDragged;
    [HideInInspector] public bool IsDragged { get => isDragged; set => isDragged = value; }

    private Vector3 offset;
    [HideInInspector] public Vector3 Offset { get => offset; set => offset = value; }

    private void OnEnable()
    {
        AddDragable();
    }

    private void OnDisable()
    {
        RemoveDragable();
    }

    public void OnClick(Vector2 pos)
    {
        Vector3 posV3 = new Vector3(pos.x, pos.y, transform.position.z);
        Vector3 relativeVector = transform.position - posV3;

        Offset = relativeVector;

        EventHandler<IDragable>.InvokeEvent(EventTypes.DRAGABLE_REORDER, this.GetComponent<IDragable>());
        isDragged = true;
        Debug.Log($"IsDragged set to true: {isDragged}");

        if(pickupSound != null)
        {
            AudioManager.instance.Play(pickupSound);
        }
    }

    public void OnDrag(Vector2 pos)
    {

        // transform into relative position
        Vector3 posV3 = new Vector3(pos.x, pos.y, transform.position.z);

        transform.position = posV3 + Offset;
    }

    public void OnRelease()
    {
        Offset = Vector3.zero;
        isDragged = false;
        Debug.Log($"IsDragged set to false: {isDragged}");
    }

    public void AddDragable()
    {
        EventHandler<IDragable>.InvokeEvent(EventTypes.DRAGABLE_ADD, this.GetComponent<IDragable>());
    }

    public void RemoveDragable()
    {
        EventHandler<IDragable>.InvokeEvent(EventTypes.DRAGABLE_REMOVE, this.GetComponent<IDragable>());
    }

    public void UpdateOrder(int index)
    {
        Debug.Log($"Updating order to: {index}");
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, index);
        transform.position = newPos;
    }
}


