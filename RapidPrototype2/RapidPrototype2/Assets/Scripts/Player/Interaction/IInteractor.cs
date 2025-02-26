using UnityEngine;
using System.Collections.Generic;

public interface IInteractor
{
    public List<IInteractable> Interactables { get; }
    public void AddInteractable(IInteractable interactable);
    public void RemoveInteractable(IInteractable interactable);
    public void ExecuteInteractable(int index = 0);
}
