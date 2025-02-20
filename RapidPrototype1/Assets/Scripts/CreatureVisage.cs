using UnityEngine;

public class CreatureVisage : MonoBehaviour, IScratchable, ICoverable
{
    [SerializeField] private GameObject scratchMark;
    [HideInInspector] public GameObject ScratchMark { get => scratchMark; set => scratchMark = value; }

    [SerializeField] private bool isScratched;
    [HideInInspector] public bool IsScratched { get => isScratched; set => isScratched = value; }

    private void Awake()
    {
        scratchMark = transform.GetChild(0).gameObject;
    }

    public void OnScratch()
    {
        IsScratched = true;
        if(scratchMark != null) { scratchMark.SetActive(true); }
        EventHandler<GameObject>.InvokeEvent(EventTypes.CREATURE_REMOVE, gameObject);
        AudioManager.instance.Play("CoinScratch");

        Debug.Log($"Scratched {gameObject.name}");
    }

    public bool CheckIfCovered()
    {
        bool covered = Physics.Raycast(transform.position, -transform.forward, Mathf.Infinity);

        if(covered) { return true; }

        return false;
    }
}
