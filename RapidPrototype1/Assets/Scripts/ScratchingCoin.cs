using UnityEngine;

public class ScratchingCoin : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Something entered trigger");

        IScratchable scratchable = collision.collider.GetComponent<IScratchable>();

        if (scratchable == null) { return; }

        if (scratchable.IsScratched) { return; }
        scratchable.OnScratch();
    }
}
