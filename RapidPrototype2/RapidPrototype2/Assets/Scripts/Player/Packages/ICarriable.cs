using UnityEngine;

public interface ICarriable
{

    public float Weight { get; }
    public bool IsCarried { get; set; }
    public Transform CarriableTransform { get; }
}
