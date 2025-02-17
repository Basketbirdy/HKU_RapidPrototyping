using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DragableHandler : MonoBehaviour
{
    [Header("Data")]
    private List<IDragable> dragables = new List<IDragable>();
    private string[] test;

    private void OnEnable()
    {
        EventHandler<IDragable>.AddListener(EventTypes.DRAGABLE_ADD, AddDragable);
        EventHandler<IDragable>.AddListener(EventTypes.DRAGABLE_REMOVE, RemoveDragable);
        EventHandler<IDragable>.AddListener(EventTypes.DRAGABLE_REORDER, ReOrderDragable);
    }

    private void OnDisable()
    {
        EventHandler<IDragable>.RemoveListener(EventTypes.DRAGABLE_ADD, AddDragable);
        EventHandler<IDragable>.RemoveListener(EventTypes.DRAGABLE_REMOVE, RemoveDragable);
        EventHandler<IDragable>.RemoveListener(EventTypes.DRAGABLE_REORDER, ReOrderDragable);
    }

    private void Start()
    {
        //test = new string[] { "Hello", "Giant", "Duck", "Bingus", "Ralph" };

        //test = CollectionUtils.ReOrderArray<string>(test, 3);

        //for(int i = test.Length - 1; i >= 0; i--)
        //{
        //    Debug.Log($"Index: {i}; String: {test[i]}");
        //}
    }

    private void AddDragable(IDragable dragable)
    {
        if (dragables.Contains(dragable)) { return; }
        dragables.Add(dragable);
    }

    private void RemoveDragable(IDragable dragable)
    {
        if(!dragables.Contains(dragable)) { return; }
        dragables.Add(dragable);
    }

    private void ReOrderDragable(IDragable dragable)
    {
        if(dragables.Contains(dragable))
        {
            for(int i = 0; i < dragables.Count; i++)
            {
                if (dragables[i] != dragable) { continue; }
                ReOrder(i);
            }
        }
    }

    private void ReOrder(int targetIndex)
    {
        dragables = CollectionUtils.ReOrderList<IDragable>(dragables, targetIndex);

        for (int i = 0; i < dragables.Count; i++)
        {
            Debug.Log($"New order: {i}; dragable object: {dragables[i].ToString()}");
            dragables[i].UpdateOrder(i);
        }
    }
}
