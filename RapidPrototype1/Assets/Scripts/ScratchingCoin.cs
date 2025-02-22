using System;
using System.Collections;
using UnityEngine;

public class ScratchingCoin : MonoBehaviour
{
    [SerializeField] private float scratchTime = .3f;

    [Space]

    [SerializeField] private float elapsedTime;

    private IDragable dragable;
    private Coroutine scratchDelay;
    private bool isCoroutineRunning = false;

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

        if (coverable != null)
        {
            if (coverable.CheckIfCovered()) { return; }
        }

        if (!isCoroutineRunning)
        {
            isCoroutineRunning = true;
            scratchDelay = StartCoroutine(Timer(scratchTime, scratchable.OnScratch));
            Debug.Log($"Coroutine; Started: {scratchDelay}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isCoroutineRunning) { return; }

        IScratchable scratchable = collision.GetComponent<IScratchable>();
        if (scratchable == null) { return; }

        StopCoroutine(scratchDelay);
        Debug.Log($"Coroutine; Stopped: {scratchDelay}");

        scratchDelay = null;
        isCoroutineRunning = false;
    }

    public IEnumerator Timer(float duration, Action action)
    {
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        action?.Invoke();
        isCoroutineRunning = false;
    }
}
