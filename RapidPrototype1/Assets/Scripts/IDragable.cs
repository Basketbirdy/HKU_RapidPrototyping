using UnityEngine;

public interface IDragable
{
    public bool IsDragged { get; set; }
    public Vector3 Offset { get; set; }

    public void OnDrag(Vector2 pos);
    public void OnClick(Vector2 pos);
    public void OnRelease();

    public void AddDragable();
    public void RemoveDragable();

    public void UpdateOrder(int index);
}
