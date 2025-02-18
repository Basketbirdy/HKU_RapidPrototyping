using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Action onStartGame;

    [Header("Creature")]
    [SerializeField] private int maxCreatureStages;

    [Header("Coin")]
    [SerializeField] private GameObject scratchingCoin;
    [SerializeField] private Vector2 coinSpawn;

    [Header("Debug")]
    [SerializeField] private bool showDarkness;
    [SerializeField] private GameObject darkness;
    [Space]
    [SerializeField] private int currentCreatureStage;
    [SerializeField] private List<GameObject> creatures = new List<GameObject>();
    [Space]
    [SerializeField] private bool creaturesCleared = false;

    private void OnEnable()
    {
        EventHandler.AddListener(EventTypes.CREATURE_STAREDAT, AdvanceStage);
        EventHandler<GameObject>.AddListener(EventTypes.CREATURE_ADD, AddCreature);
        EventHandler<GameObject>.AddListener(EventTypes.CREATURE_REMOVE, RemoveCreature);
    }

    private void OnDisable()
    {
        EventHandler.RemoveListener(EventTypes.CREATURE_STAREDAT, AdvanceStage);
        EventHandler<GameObject>.RemoveListener(EventTypes.CREATURE_ADD, AddCreature);
        EventHandler<GameObject>.RemoveListener(EventTypes.CREATURE_REMOVE, RemoveCreature);
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

    private void AddCreature(GameObject creature)
    {
        if(!creatures.Contains(creature))
        {
            creatures.Add(creature);
        }
    }

    private void RemoveCreature(GameObject creature)
    {
        if(creatures.Contains(creature))
        {
            creatures.Remove(creature);
        }

        if(creatures.Count >= 0) { creaturesCleared = true; }
    }
}
