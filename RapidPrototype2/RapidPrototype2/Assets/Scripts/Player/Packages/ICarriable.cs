using UnityEngine;

public interface ICarriable
{
    public bool IsCarried { get; set; }
    public Transform CarriableTransform { get; }
}
