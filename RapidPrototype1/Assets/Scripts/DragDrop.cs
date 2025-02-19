using System;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    [Header("States")]
    [SerializeField] private bool isHolding;

    [Header("Data")]
    private Vector2 mouseWorldPos;
    private IDragable heldDragable;

    public static Action onClick;
    public static Action onRelease;

    private Vector2 moveOffset;

    private void OnEnable()
    {
        onClick += OnClick;
        onRelease += OnRelease;
    }

    private void OnDisable()
    {
        onClick -= OnClick;
        onRelease -= OnRelease;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        UpdateDragable();
    }

    private void GetInput()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Mouse0)) { onClick?.Invoke(); }
        if (Input.GetKeyUp(KeyCode.Mouse0)) { onRelease?.Invoke(); }
    }

    private void UpdateDragable()
    {
        if (!isHolding) { return; }

        if(heldDragable == null)
        {
            Debug.LogWarning("Not holding anything, missing held dragable");
            return;
        }

        heldDragable.OnDrag(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void OnClick()
    {
        Debug.Log("Clicked");

        if (isHolding) { return; }

        // shoot raycast to find draggable
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

        if(hit == null) { return; }

        IClickable clickable = hit.GetComponent<IClickable>();
        if (clickable != null)
        {
            clickable.OnClick();
        }

        IDragable dragable = hit.GetComponent<IDragable>();
        if (dragable != null)
        {
            heldDragable = dragable;
            dragable.OnClick(mouseWorldPos);

            isHolding = true;
        }
    }

    private void OnRelease()
    {
        Debug.Log("Released");

        if(!isHolding) { return; }

        heldDragable.OnRelease();
        heldDragable = null;
        isHolding = false;
    }

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(mouseWorldPos, transform.forward * 100);
        Gizmos.DrawRay(ray);
    }
}
