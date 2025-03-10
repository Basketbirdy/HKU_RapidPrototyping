using UnityEngine;

[System.Serializable]
public class SpawnHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 area;
    [SerializeField] private Vector2 groupSizeRange = new Vector2(1, 6);
    [SerializeField] private float spawnInterval = 6.0f;
    [SerializeField] private Vector2 groupSpawnRange = new Vector2(2, 6);

    [Header("Monsters")]
    [SerializeField] private MonsterObject[] monsters;

    private Transform player;

    // timer
    private float timer;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerManager>().transform;
    }

    private void Start()
    {
        timer = Time.time + spawnInterval;
    }

    private void Update()
    {
        if(timer > Time.time) { return; }

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
        float groupPosX = Random.Range(-area.x, area.x); 
        float groupPosY = Random.Range(-area.y, area.y); 
        Vector2 groupPos = new Vector2(groupPosX, groupPosY);

        foreach(MonsterObject monster in group)
        {
            float offsetX = Random.Range(-groupSpawnRange.x, groupSpawnRange.x);
            float offsetY = Random.Range(-groupSpawnRange.y, groupSpawnRange.y);
            Vector2 offset = new Vector2(offsetX, offsetY);
            Vector2 individualPos = groupPos + offset;

            Monster monsterScript = Instantiate(monster.prefab, individualPos, Quaternion.identity).GetComponent<Monster>();
            monsterScript.SetTarget(player);
        }
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
