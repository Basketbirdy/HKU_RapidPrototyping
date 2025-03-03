using System;
using UnityEngine;

public interface ICarriable
{
    public Transform CarriableTransform { get; }
    public bool IsCarried { get; set; }

    public void OnCarry();
    public void OnThrow();
}
