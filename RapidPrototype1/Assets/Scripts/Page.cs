using UnityEngine;

public class Page : MonoBehaviour
{
    [Header("Dimensions")]
    public float pageX = 6f;
    public float pageY = 8f;

    [Header("References")]
    [SerializeField] private SpriteRenderer pageGFX;
    [SerializeField] private BoxCollider visibilityCollider;
    [SerializeField] private BoxCollider2D pageCollider;

    private void Awake()
    {
        pageGFX = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetupPage(Creature creature, Color pageColor)
    {
        pageGFX.size = new Vector3(pageX, pageY, 0);
        pageCollider.size = new Vector3(pageX, pageY);
        visibilityCollider.size = new Vector3 (pageX, pageY, 0);
        pageGFX.GetComponent<SpriteRenderer>().color = pageColor;

        // setup creature
        if(creature == null) { return; }
        float rangeX = (pageX / 2) - (creature.x / 2);
        float rangeY = (pageY / 2) - (creature.y / 2);
        Vector3 position = new Vector3(Random.Range(-rangeX, rangeX), Random.Range(-rangeY, rangeX), transform.position.z - 0.01f);
        GameObject creatureObj = Instantiate(creature.prefab, position, Quaternion.identity, transform);
        EventHandler<GameObject>.InvokeEvent(EventTypes.CREATURE_ADD, creatureObj);
    }
}
