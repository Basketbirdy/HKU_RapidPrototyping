using UnityEngine;
using System.Collections.Generic;
using System;

public interface ICarrier
{
    public bool AwaitingMove { get; }
    public Transform CarryPoint { get; }
    public List<ICarriable> Carriables { get; }
    public Queue<ICarriable> CarriableQueue { get; }

    public void PickUp(ICarriable carriable);
    public void Drop();
    public void Drop(Vector3 target, Action action);
}
