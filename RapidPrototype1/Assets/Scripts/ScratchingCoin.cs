using UnityEngine;

public class ScratchingCoin : MonoBehaviour
{
    [SerializeField]

    private IDragable dragable;

    private void Awake()
    {
        dragable = gameObject.GetComponent<IDragable>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something entered trigger");

        IScratchable scratchable = collision.GetComponent<IScratchable>();
        ICoverable coverable = collision.GetComponent<ICoverable>();

        // early returns
        if (scratchable == null) { return; }
        if (!dragable.IsDragged) { return; }
        if (scratchable.IsScratched) { return; }
        
        if(coverable != null)
        {
            if (coverable.CheckIfCovered()) { return; }
        }

        if(scratchable.IsScratched) { return; }
        scratchable.OnScratch();


    }
}
