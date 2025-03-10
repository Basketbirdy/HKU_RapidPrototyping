using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private int monstersLeft;

    [Header("User Interface Ids")]
    [SerializeField] private string healthBarMaskId = "Element_HealthBarMask";
    [SerializeField] private string counterLabelId = "Label_MonsterCounter";
    [SerializeField] private string resetButtonId = "Button_Reset";

    private void Awake()
    {
        EventHandler<HealthChangeInfo>.AddListener(EventStrings.PLAYER_UI_ONHEALTHCHANGE, OnHealthChange);
        EventHandler.AddListener(EventStrings.MONSTER_KILLED, OnMonsterDefeated);
        EventHandler.AddListener(EventStrings.PLAYER_UI_ONDEATH, OnPlayerDeath);

        UserInterfaceHandler.instance.AddVisualElementRef(healthBarMaskId);

        UserInterfaceHandler.instance.AddLabelRef(counterLabelId);

        UserInterfaceHandler.instance.AddButtonRef(resetButtonId);
        UserInterfaceHandler.instance.AddButtonListener(resetButtonId, OnReset);
    }

    private void OnDisable()
    {
        EventHandler<HealthChangeInfo>.RemoveListener(EventStrings.PLAYER_UI_ONHEALTHCHANGE, OnHealthChange);
        EventHandler.RemoveListener(EventStrings.MONSTER_KILLED, OnMonsterDefeated);
        EventHandler.RemoveListener(EventStrings.PLAYER_UI_ONDEATH, OnPlayerDeath);
    }

    private void Start()
    {
        UserInterfaceHandler.instance.SetLabel(counterLabelId, monstersLeft.ToString());
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
        monstersLeft--;
        UserInterfaceHandler.instance.SetLabel(counterLabelId, monstersLeft.ToString());

        if( monstersLeft <= 0)
        {
            // end game
            UserInterfaceHandler.instance.ShowButton(resetButtonId);
        }
    }

    private void OnReset()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
    private void OnPlayerDeath()
    {
        UserInterfaceHandler.instance.ShowButton(resetButtonId);
    }
}
