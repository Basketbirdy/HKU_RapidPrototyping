using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("User Interface Ids")]
    [SerializeField] private string healthBarMaskId = "Element_HealthBarMask";

    private void Awake()
    {
        EventHandler<HealthChangeInfo>.AddListener(EventStrings.PLAYER_UI_ONHEALTHCHANGE, OnHealthChange);
    }

    private void OnDisable()
    {
        EventHandler<HealthChangeInfo>.RemoveListener(EventStrings.PLAYER_UI_ONHEALTHCHANGE, OnHealthChange);
    }

    private void Start()
    {
        UserInterfaceHandler.instance.AddVisualElementRef(healthBarMaskId);
    }

    private void OnHealthChange(HealthChangeInfo info)
    {
        Debug.Log($"Attempting health bar change");
        Debug.Log($"HealthChangeInfo; min: {info.currentHealth}, max: {info.currentMaxHealth}");
        float min = 8f;
        float max = 88f;

        float ratio = MathUtils.Remap(info.currentHealth, 0, info.currentMaxHealth, min, max);
        Debug.Log($"HealthChange; ratio: {ratio}");
        UserInterfaceHandler.instance.ResizeVisualElement(healthBarMaskId, ratio, 100, true);
    }
}
