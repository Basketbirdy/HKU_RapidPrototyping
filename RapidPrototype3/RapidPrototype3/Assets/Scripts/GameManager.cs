using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private int monstersToSlay = 16;
    [SerializeField] private int monstersSlain;

    [Header("User Interface Ids")]
    [SerializeField] private string healthBarMaskId = "Element_HealthBarMask";
    [SerializeField] private string counterLabelId = "Label_MonsterCounter";

    [SerializeField] private string startScreenId = "Element_StartScreen";
    [SerializeField] private string startScreenImageId = "Element_StartScreenImage";
    [SerializeField] private string startButtonId = "Button_Start";

    [SerializeField] private string endScreenId = "Element_EndScreen";
    [SerializeField] private string endScreenTextId = "Label_EndText";
    [SerializeField] private string resetButtonId = "Button_Reset";
    
    private string endText = "";
    private bool hasEnded;

    private void Awake()
    {
        EventHandler<HealthChangeInfo>.AddListener(EventStrings.PLAYER_UI_ONHEALTHCHANGE, OnHealthChange);
        EventHandler.AddListener(EventStrings.MONSTER_KILLED, OnMonsterDefeated);
        EventHandler.AddListener(EventStrings.PLAYER_UI_ONDEATH, OnPlayerDeath);

        UserInterfaceHandler.instance.AddVisualElementRef(healthBarMaskId);

        UserInterfaceHandler.instance.AddVisualElementRef(startScreenId);
        UserInterfaceHandler.instance.AddVisualElementRef(startScreenImageId);

        UserInterfaceHandler.instance.AddButtonRef(startButtonId);
        UserInterfaceHandler.instance.AddButtonListener(startButtonId, OnStart);

        UserInterfaceHandler.instance.AddVisualElementRef(endScreenId);
        UserInterfaceHandler.instance.AddLabelRef(endScreenTextId);

        UserInterfaceHandler.instance.AddLabelRef(counterLabelId);

        UserInterfaceHandler.instance.AddButtonRef(resetButtonId);
        UserInterfaceHandler.instance.AddButtonListener(resetButtonId, OnReset);
    }

    private void OnDisable()
    {
        EventHandler<HealthChangeInfo>.RemoveListener(EventStrings.PLAYER_UI_ONHEALTHCHANGE, OnHealthChange);
        EventHandler.RemoveListener(EventStrings.MONSTER_KILLED, OnMonsterDefeated);
        EventHandler.RemoveListener(EventStrings.PLAYER_UI_ONDEATH, OnPlayerDeath);

        UserInterfaceHandler.instance.RemoveButtonListener(resetButtonId, OnReset);
        UserInterfaceHandler.instance.RemoveButtonListener(startButtonId, OnStart);
    }

    private void Start()
    {
        endText = "";
        monstersSlain = 0;
        UserInterfaceHandler.instance.SetLabel(counterLabelId, monstersSlain.ToString() + " / " + monstersToSlay);

        Time.timeScale = 0;
    }

    private void OnHealthChange(HealthChangeInfo info)
    {
        Debug.Log($"Attempting health bar change");
        Debug.Log($"HealthChangeInfo; min: {info.currentHealth}, max: {info.currentMaxHealth}");
        float min = 0f;
        float max = 100f;

        float ratio = MathUtils.Remap(info.currentHealth, 0, info.currentMaxHealth, min, max);
        Debug.Log($"HealthChange; ratio: {ratio}");
        UserInterfaceHandler.instance.ResizeVisualElement(healthBarMaskId, ratio, 100, true);
    }

    private void OnMonsterDefeated()
    {
        monstersSlain++;
        UserInterfaceHandler.instance.SetLabel(counterLabelId, monstersSlain.ToString() + " / " + monstersToSlay);

        if( monstersSlain >= monstersToSlay && !hasEnded)
        {
            // end game
            endText = "I've slain the requested amount of monsters.";
            UserInterfaceHandler.instance.SetLabel(endScreenTextId, endText);
            UserInterfaceHandler.instance.ShowVisualElement(startScreenImageId);
            UserInterfaceHandler.instance.ShowVisualElement(endScreenId);
            hasEnded = true;
        }
    }

    private void OnReset()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    private void OnStart()
    {
        Time.timeScale = 1.0f;
        UserInterfaceHandler.instance.HideVisualElement(startScreenId);
        UserInterfaceHandler.instance.HideVisualElement(startScreenImageId);
    }

    private void OnPlayerDeath()
    {
        endText = "...";
        UserInterfaceHandler.instance.HideVisualElement(startScreenImageId);
        UserInterfaceHandler.instance.SetLabel(endScreenTextId, endText);
        UserInterfaceHandler.instance.ShowVisualElement(endScreenId);
        hasEnded = true;
    }
}
