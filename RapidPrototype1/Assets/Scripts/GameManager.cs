using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Action onStartGame;

    [Header("Creature")]
    [SerializeField] private int maxCreatureStages;
    [SerializeField] private float baseCreatureBashRate = 0;
    [SerializeField] private float creatureBashRateIncrement = 25;
    [SerializeField] private float baseCreatureBashDelay = .5f;
    private Coroutine bashTimer;

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
    [SerializeField] private float currentCreatureBashRate;

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
        currentCreatureBashRate = baseCreatureBashRate;

        Instantiate(scratchingCoin, coinSpawn, Quaternion.identity);
        Page page = Instantiate(tutorialPage, tutorialPageSpawn, Quaternion.identity).GetComponent<Page>();
        page.SetupPage(null, Color.white);

        AudioManager.instance.Play("Ambience");
        OnStartGame();
    }

    private void OnStartGame()
    {
        onStartGame?.Invoke();
        StartBashTimer();
    }

    public void AdvanceStage()
    {
        currentCreatureStage += 1;
        currentCreatureBashRate = baseCreatureBashRate + creatureBashRateIncrement * currentCreatureStage;
        AudioManager.instance.Play("MonsterSignal");
        
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

        string currentEnding = "";

        if(currentCreatureStage > maxCreatureStages)
        {
            currentEnding = "CreatureAttack";
        }
        else
        {
            if (creaturesCleared)
            {
                if(currentCreatureStage == 0)
                {
                    currentEnding = "NoVisage_CreatureUnAlerted";
                }
                else
                {
                    currentEnding = "NoVisage_CreatureAlerted";
                }
            }
            else
            {
                if (currentCreatureStage == 0)
                {
                    currentEnding = "Visage_CreatureUnAlerted";
                }
                else
                {
                    currentEnding = "Visage_CreatureAlerted";
                }
            }
        }


        EndingData currentEndingData = new EndingData();
        foreach(EndingData ending in endingData)
        {
            if(currentEnding == ending.name)
            {
                currentEndingData = ending;
            }
        }

        EventHandler<EndingData>.InvokeEvent(EventTypes.GAME_END, currentEndingData);
    }

    public void RestartGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void StartBashTimer()
    {
        if(bashTimer == null)
        {
            float randDuration = UnityEngine.Random.Range(baseCreatureBashDelay - .3f, baseCreatureBashDelay + .3f);
            bashTimer = StartCoroutine(Timer(randDuration, PlayBashSound));
        }
    }

    private void PlayBashSound()
    {
        float value = UnityEngine.Random.value;
        if(value < (currentCreatureBashRate / 100))
        {
            AudioManager.instance.Play("MonsterBashing");
        }
        bashTimer = null;
        StartBashTimer();
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
