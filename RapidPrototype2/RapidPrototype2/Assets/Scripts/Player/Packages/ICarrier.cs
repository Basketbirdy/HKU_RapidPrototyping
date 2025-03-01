using UnityEngine;
using System.Collections.Generic;

public interface ICarrier
{
    public Transform CarryPoint { get; }
    public List<ICarriable> Carriables { get; }
    public Queue<ICarriable> CarriableQueue { get; }

    public void PickUp(ICarriable carriable);
    public void Drop(ICarriable carriable);
}
