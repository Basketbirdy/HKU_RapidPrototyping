using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private float health;
    [SerializeField] private float invincibility = 3f;

    private PlayerStats stats;

    private float currentHealth;
    public float CurrentHealth { get => currentHealth; }

    [SerializeField] private bool isInvincible;
    public bool IsInvincible { get => isInvincible; set => isInvincible = value; }

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

    public void TakeDamage(float damage)
    {
        Debug.Log($"Damaging {gameObject.name} for {damage} damage");

        float newHealth = currentHealth - damage;
        EventHandler<HealthChangeInfo>.InvokeEvent(EventStrings.PLAYER_UI_ONHEALTHCHANGE, new HealthChangeInfo(newHealth, stats.GetFloatStat(nameof(health))));

        Debug.Log($"New health is {newHealth}");

        if(newHealth > 0) 
        { 
            currentHealth = newHealth;
            StartCoroutine(InvincibilityTimer(stats.GetFloatStat(nameof(invincibility))));
            return; 
        }

        Die();
    }

    public void Die() 
    {
        EventHandler.InvokeEvent(EventStrings.PLAYER_UI_ONDEATH);
        gameObject.SetActive(false); 
    }

    private void OnBind()
    {
        FloatStat healthStat = new FloatStat(nameof(health), health);
        FloatStat invincibilityStat = new FloatStat(nameof(invincibility), invincibility);

        if (stats == null) { Debug.Log($"Stats is null"); }
        stats.BindStats(healthStat, invincibilityStat);

        EventHandler.RemoveListener(EventStrings.PLAYER_STATS_BIND, OnBind);
    }

    private void OnInitialize()
    {
        // initialisation of stats at start
        currentHealth = stats.GetFloatStat(nameof(health));
    }

    private IEnumerator InvincibilityTimer(float duration)
    {
        Debug.Log($"{gameObject.name} IS invincible");

        bool isRunning = true;
        float elapsedTime = 0;
        
        IsInvincible = true;

        while (isRunning == true)
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime >= duration)
            {
                isRunning = false;
            }

            yield return null;
        }

        IsInvincible = false;
        Debug.Log($"{gameObject.name} is NO LONGER invincible");
    }
}

public struct HealthChangeInfo
{
    public float currentHealth;
    public float currentMaxHealth;

    public HealthChangeInfo(float currentHealth, float currentMaxHealth)
    {
        this.currentHealth = currentHealth;
        this.currentMaxHealth = currentMaxHealth;
    }
}
