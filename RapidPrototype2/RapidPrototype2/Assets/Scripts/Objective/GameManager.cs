using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Settings")]
    [SerializeField] private float startTime = 60f;
    [SerializeField] private float restoreTime;
    [Space]
    [SerializeField] private float currentTime;
    [SerializeField] private float totalTimeSpent;

    private bool isRunning;
    private Coroutine gameTimer;

    [Header("UI")]
    [SerializeField] private UIDocument document;
    private MenuUI menuUI;

    // main menu
    [SerializeField] private string mainMenu = "MainMenu_Backdrop";
    [SerializeField] private string startButton = "MainMenu_Buttons_Start";
    [SerializeField] private string quitButton = "MainMenu_Buttons_Quit";
    [Space]
    // hud
    [SerializeField] private string hud = "HUD_Backdrop";
    [SerializeField] private string timer = "HUD_Timer_Label";
    [Space]
    // game over menu
    [SerializeField] private string gameOverMenu = "GameOverMenu_Backdrop";
    [SerializeField] private string results = "GameOverMenu_Stats_Label";
    [SerializeField] private string goMainMenuButton = "GameOverMenu_Buttons_MainMenu";
    [SerializeField] private string goQuitButton = "GameOverMenu_Buttons_Quit";

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this; 
        }
        else 
        { 
            Destroy(this.gameObject);
        }

        menuUI = new MenuUI(document);

        // main menu
        menuUI.AddVisualElement(mainMenu);
        menuUI.AddButton(startButton);
        menuUI.AddButtonListener(startButton, StartGame);
        menuUI.AddButton(quitButton);
        menuUI.AddButtonListener(quitButton, QuitGame);

        // hud
        menuUI.AddVisualElement(hud);
        menuUI.AddLabel(timer);

        // game over menu
        menuUI.AddVisualElement("GameOverMenu_Backdrop");
        menuUI.AddLabel(results);
        menuUI.AddButton(goMainMenuButton);
        menuUI.AddButtonListener(goMainMenuButton, RestartGame);
        menuUI.AddButton(goQuitButton);
        menuUI.AddButtonListener(goQuitButton, QuitGame);
    }

    private void Start()
    {
        menuUI.ShowVisualElement(mainMenu);
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        menuUI.RemoveButtonListener(startButton, StartGame);
        menuUI.RemoveButtonListener(quitButton, QuitGame);       
        
        menuUI.RemoveButtonListener(goMainMenuButton, RestartGame);
        menuUI.RemoveButtonListener(goQuitButton, QuitGame);
    }

    private void StartGame()
    {
        Time.timeScale = 1.0f;
        menuUI.HideVisualElement(mainMenu);
        menuUI.ShowVisualElement(hud);
        gameTimer = StartCoroutine(GameTimer());
    }

    private void RestartGame()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    private void QuitGame()
    {
        Debug.LogWarning("Application quit!", gameObject);
        Application.Quit();
    }

    private void EndGame()
    {
        menuUI.HideVisualElement(hud);
        menuUI.ShowVisualElement(gameOverMenu);

        string resultsString = $"You survived for {totalTimeSpent.ToString("F2")} seconds";
        menuUI.ChangeLabel(results, resultsString);
    }

    private IEnumerator GameTimer()
    {
        currentTime = startTime;

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            totalTimeSpent += Time.deltaTime;
            menuUI.ChangeLabel(timer, currentTime.ToString("F0"));
            yield return null;
        }

        currentTime = 0;
        EndGame();
    }

    public void AddTime()
    {
        currentTime += restoreTime;
    }
}
