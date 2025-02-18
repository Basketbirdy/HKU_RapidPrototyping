using UnityEngine;

public interface IScratchable
{
    public bool IsScratched { get; set; }
    public void OnScratch();
}
