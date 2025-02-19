using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Action onStartGame;

    [Header("Creature")]
    [SerializeField] private int maxCreatureStages;

    [Header("Coin")]

    [Header("Ending")]
    [SerializeField] private float endDelay;
    [Space]
    [SerializeField] private EndingData[] endingData;

    [Header("Dragable instantiation")]
    [SerializeField] private GameObject tutorialPage;
    [SerializeField] private Vector3 tutorialPageSpawn;
    [SerializeField] private GameObject scratchingCoin;
    [SerializeField] private Vector2 coinSpawn;

    [Header("Debug")]
    [SerializeField] private bool showDarkness;
    [SerializeField] private GameObject darkness;
    [Space]
    [SerializeField] private List<GameObject> creatures = new List<GameObject>();

    [Header("Win conditions")]
    [SerializeField] private int currentCreatureStage;
    [SerializeField] private bool creaturesCleared = false;

    private void OnEnable()
    {
        EventHandler.AddListener(EventTypes.CREATURE_STAREDAT, AdvanceStage);
        EventHandler<GameObject>.AddListener(EventTypes.CREATURE_ADD, AddCreature);
        EventHandler<GameObject>.AddListener(EventTypes.CREATURE_REMOVE, RemoveCreature);

        // game events
        EventHandler.AddListener(EventTypes.GAME_END_TRIGGER, TriggerEndGame);
    }

    private void OnDisable()
    {
        EventHandler.RemoveListener(EventTypes.CREATURE_STAREDAT, AdvanceStage);
        EventHandler<GameObject>.RemoveListener(EventTypes.CREATURE_ADD, AddCreature);
        EventHandler<GameObject>.RemoveListener(EventTypes.CREATURE_REMOVE, RemoveCreature);

        // game events
        EventHandler.RemoveListener(EventTypes.GAME_END_TRIGGER, TriggerEndGame);
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
        Instantiate(tutorialPage, tutorialPageSpawn, Quaternion.identity);

        OnStartGame();
    }

    private void OnStartGame()
    {
        onStartGame?.Invoke();
    }

    public void AdvanceStage()
    {
        currentCreatureStage += 1;  
        
        if(currentCreatureStage > maxCreatureStages) 
        {
            // TODO - trigger jumpscare
            EventHandler.InvokeEvent(EventTypes.GAME_END_TRIGGER);
        }
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

        if(creatures.Count <= 0) 
        {
            creaturesCleared = true;
        }
    }

    private void TriggerEndGame()
    {
        StartCoroutine(Timer(endDelay, EndGame));
    }

    private void EndGame()
    {
        Debug.LogWarning("ENDING GAME");
    }

    public IEnumerator Timer(float duration, Action action)
    {
        float elapsedTime = 0f;

        while(elapsedTime < duration) 
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        action?.Invoke();
    }

}

[System.Serializable]
struct EndingData
{
    public string name;
    [TextArea(5, 10)] public string flavorText;
    [TextArea(5, 10)] public string explanationText;
}
