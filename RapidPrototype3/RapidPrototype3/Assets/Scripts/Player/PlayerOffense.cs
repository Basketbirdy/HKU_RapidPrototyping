using UnityEngine;

public class PlayerOffense : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private PlayerStats stats;

    private void Awake()
    {
        EventHandler<PlayerStats>.AddListener(EventStrings.PLAYER_STATS_ASSIGNREFERENCE, AssignStats);
        EventHandler.AddListener(EventStrings.PLAYER_STATS_BIND, OnBind);
        EventHandler.AddListener(EventStrings.PLAYER_STATS_INITIALIZE, OnInitialize);
    }

    private void AssignStats(PlayerStats stats)
    {
        this.stats = stats;
        EventHandler<PlayerStats>.RemoveListener(EventStrings.PLAYER_STATS_ASSIGNREFERENCE, AssignStats);
    }

    private void OnBind()
    {
        FloatStat damageStat = new FloatStat(nameof(damage), damage);
        stats.BindStats(damageStat);
    }

    private void OnInitialize()
    {

    }
}
