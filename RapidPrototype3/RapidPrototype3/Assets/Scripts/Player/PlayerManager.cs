using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerStats playerStats;
    public PlayerStats PlayerStats { get { return playerStats; } private set { playerStats = value; } }

    private void Awake()
    {
        // events sequence to make sure everything happens in order
        PlayerStats = new PlayerStats();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventHandler<PlayerStats>.InvokeEvent(EventStrings.PLAYER_STATS_ASSIGNREFERENCE, PlayerStats);
        EventHandler.InvokeEvent(EventStrings.PLAYER_STATS_BIND);
        EventHandler.InvokeEvent(EventStrings.PLAYER_STATS_INITIALIZE);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
