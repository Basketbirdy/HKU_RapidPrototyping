using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Action onStartGame;

    [Header("Creature Stages")]
    [SerializeField] private int maxCreatureStages;

    [Header("Coin")]
    [SerializeField] private GameObject scratchingCoin;
    [SerializeField] private Vector2 coinSpawn;

    [Header("Debug")]
    [SerializeField] private bool showDarkness;
    [SerializeField] private GameObject darkness;
    [Space]
    [SerializeField] private int currentCreatureStage;

    private void OnEnable()
    {
        EventHandler.AddListener(EventTypes.CREATURE_STAREDAT, AdvanceStage);
    }

    private void OnDisable()
    {
        EventHandler.RemoveListener(EventTypes.CREATURE_STAREDAT, AdvanceStage);
    }

    private void Awake()
    {
        if(instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        if (showDarkness) { darkness.SetActive(true); }
        else {  darkness.SetActive(false); }

        currentCreatureStage = 0;

        Instantiate(scratchingCoin, coinSpawn, Quaternion.identity);

        OnStartGame();
    }

    private void OnStartGame()
    {
        onStartGame?.Invoke();
    }

    public void AdvanceStage()
    {
        currentCreatureStage += 1;   
    }

    public int GetCreatureStage()
    {
        return currentCreatureStage;
    }
}
