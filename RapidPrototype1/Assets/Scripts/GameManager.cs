using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Action onStartGame;


    [Header("Debug")]
    [SerializeField] private bool showDarkness;
    [SerializeField] private GameObject darkness;

    private void Awake()
    {
        if(instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        if (showDarkness) { darkness.SetActive(true); }
        else {  darkness.SetActive(false); }

        OnStartGame();
    }

    private void OnStartGame()
    {
        onStartGame?.Invoke();
    }
}
