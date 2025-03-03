using UnityEngine;

public interface ICarrier
{
    public Transform CarryPoint { get; }
    public ICarriable Carriable { get; set; }

    public void PickUp(ICarriable carriable);
    public void Drop();
    public void Throw();
}
