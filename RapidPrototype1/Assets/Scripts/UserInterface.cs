using TMPro;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private Canvas gameEndScreen;
    [SerializeField] private TextMeshProUGUI endingFlavorText;
    [SerializeField] private TextMeshProUGUI endingExplanationText;

    private void OnEnable()
    {
        EventHandler<EndingData>.AddListener(EventTypes.GAME_END, ShowEndingUI);
    }

    private void OnDisable()
    {
        EventHandler<EndingData>.RemoveListener(EventTypes.GAME_END, ShowEndingUI);
    }

    private void Start()
    {
        gameEndScreen.gameObject.SetActive(false);
    }

    private void ShowEndingUI(EndingData data)
    {
        gameEndScreen.gameObject.SetActive(true);

        endingFlavorText.text = data.flavorText;
        endingExplanationText.text = data.explanationText;
    }
}
