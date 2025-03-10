using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]
public class SpawnHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 area;
    [SerializeField] private float areaThickness;
    [SerializeField] private Vector2 groupSizeRange = new Vector2(1, 6);
    [SerializeField] private float spawnInterval = 6.0f;
    [SerializeField] private Vector2 groupSpawnRange = new Vector2(2, 6);

    [Header("Monsters")]
    [SerializeField] private MonsterObject[] monsters;

    private Transform player;
    private bool hasStopped = false;

    // timer
    private float timer;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerManager>().transform;

        EventHandler.AddListener(EventStrings.PLAYER_UI_ONDEATH, OnPlayerDeath);
    }

    private void OnDisable()
    {
        EventHandler.RemoveListener(EventStrings.PLAYER_UI_ONDEATH, OnPlayerDeath);
    }

    private void Start()
    {
        timer = Time.time + spawnInterval;
    }

    private void Update()
    {
        if(timer > Time.time || hasStopped) { return; }

        // form group
        int size = (int)Random.Range(groupSizeRange.x, groupSizeRange.y);
        MonsterObject[] group = new MonsterObject[size];

        for(int i = 0; i < size; i++)
        {
            int randInt = Random.Range(0, monsters.Length);
            MonsterObject selected = monsters[randInt];
            group[i] = selected;
        }

        SpawnGroup(group);
        timer  = Time.time + spawnInterval;
    }

    private void SpawnGroup(MonsterObject[] group)
    {
        int randValue = Random.Range(1, 5);
        float groupPosX = 0;
        float groupPosY = 0;  
        switch (randValue)
        {
            case 1: // up
                groupPosX = Random.Range(-area.x / 2, area.x / 2);
                groupPosY = Random.Range(area.y / 2, area.y / 2 + areaThickness);
                break;
            case 2: // right
                groupPosX = Random.Range(area.x / 2, area.x / 2 + areaThickness);
                groupPosY = Random.Range(-area.y / 2, area.y / 2);
                break;
            case 3: // down
                groupPosX = Random.Range(-area.x / 2, area.x / 2);
                groupPosY = Random.Range(-area.y / 2, -area.y / 2 - areaThickness);
                break;
            case 4: // left
                groupPosX = Random.Range(-area.x / 2, -area.x / 2 - areaThickness);
                groupPosY = Random.Range(-area.y / 2, area.y / 2);
                break;
        }
        Vector2 groupPos = new Vector2(groupPosX, groupPosY);

        foreach(MonsterObject monster in group)
        {
            float offsetX = Random.Range(-groupSpawnRange.x/2, groupSpawnRange.x/2);
            float offsetY = Random.Range(-groupSpawnRange.y/2, groupSpawnRange.y/2);
            Vector2 offset = new Vector2(offsetX, offsetY);
            Vector2 individualPos = groupPos + offset;

            Monster monsterScript = Instantiate(monster.prefab, individualPos, Quaternion.identity).GetComponent<Monster>();
            monsterScript.SetTarget(player);
        }
    }

    private void OnPlayerDeath()
    {
        hasStopped = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, area);
    }
}

[System.Serializable]
public struct MonsterObject
{
    public GameObject prefab;
    [Range(0, 100)] public int weight;
}
