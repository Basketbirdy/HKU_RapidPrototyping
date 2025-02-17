using UnityEngine;

public class Page : MonoBehaviour
{
    [Header("Dimensions")]
    public float pageX = 6f;
    public float pageY = 8f;

    [Header("References")]
    [SerializeField] private GameObject pageGFX;
    private BoxCollider2D pageCollider;

    private void Awake()
    {
        pageCollider = GetComponent<BoxCollider2D>();
    }

    public void SetupPage(Creature creature, Color pageColor)
    {
        pageGFX.transform.localScale = new Vector3(pageX, pageY, 0);
        pageCollider.size = new Vector2(pageX, pageY);
        pageGFX.GetComponent<SpriteRenderer>().color = pageColor;

        // setup creature
        if(creature == null) { return; }
        float rangeX = (pageX / 2) - (creature.x / 2);
        float rangeY = (pageY / 2) - (creature.y / 2);
        Vector3 position = new Vector3(Random.Range(-rangeX, rangeX), Random.Range(-rangeY, rangeX), transform.position.z - 0.01f);
        Debug.Log($"Position: {position}");
        GameObject creatureObj = Instantiate(creature.prefab, position, Quaternion.identity, transform);
    }
}
