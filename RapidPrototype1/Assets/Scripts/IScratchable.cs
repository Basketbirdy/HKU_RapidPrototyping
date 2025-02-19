using UnityEngine;

public interface IScratchable
{
    public GameObject ScratchMark { get; set; }
    public bool IsScratched { get; set; }
    public void OnScratch();
}
