using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float friction;

    // dynamic variables
    Vector2 direction;

    // references
    private Rigidbody2D rb;

    private PhysicsMovement physicsMovement;
    private PlayerStats stats;

    private void Awake()
    {
        EventHandler<PlayerStats>.AddListener(EventStrings.PLAYER_STATS_ASSIGNREFERENCE, AssignStats);
        EventHandler.AddListener(EventStrings.PLAYER_STATS_BIND, OnBind);
        EventHandler.AddListener(EventStrings.PLAYER_STATS_INITIALIZE, OnInitialize);

        rb = GetComponent<Rigidbody2D>();
        physicsMovement = new PhysicsMovement(rb);
    }

    private void Update()
    {
        GetInput();
        physicsMovement.Move(direction, stats.GetFloatStat(nameof(acceleration)), stats.GetFloatStat(nameof(speed)));
    }

    private void FixedUpdate()
    {
        physicsMovement.UpdatePhysics(stats.GetFloatStat(nameof(friction)), stats.GetFloatStat(nameof(deceleration)));
    }

    private void GetInput()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirY = Input.GetAxisRaw("Vertical");
        direction = new Vector2(dirX, dirY);
        direction = direction.normalized;
    }

    private void AssignStats(PlayerStats stats)
    {
        this.stats = stats;
        EventHandler<PlayerStats>.RemoveListener(EventStrings.PLAYER_STATS_ASSIGNREFERENCE, AssignStats);
    }

    private void OnBind()
    {
        FloatStat speedStat = new FloatStat(nameof(speed), speed);
        FloatStat accelerationStat = new FloatStat(nameof(acceleration), acceleration);
        FloatStat decelerationStat = new FloatStat(nameof(deceleration), deceleration);
        FloatStat frictionStat = new FloatStat(nameof(friction), friction);
        stats.BindStats(speedStat, accelerationStat, decelerationStat, frictionStat);

        EventHandler.RemoveListener(EventStrings.PLAYER_STATS_BIND, OnBind);
    }

    private void OnInitialize()
    {
        // initialisation of stats at start
    }

    public void ResetStats()
    {
        stats.SetFloatStat(nameof(speed), speed);
        stats.SetFloatStat(nameof(acceleration), acceleration);
        stats.SetFloatStat(nameof(deceleration), deceleration);
        stats.SetFloatStat(nameof(friction), friction);
    } 

    public void PrintStats()
    {
        Debug.Log("------- Stats --------");
        Debug.Log($"Speed: {stats.GetFloatStat(nameof(speed))}");
        Debug.Log($"Acceleration: {stats.GetFloatStat(nameof(acceleration))}");
        Debug.Log($"Deceleration: {stats.GetFloatStat(nameof(deceleration))}");
        Debug.Log($"Friction: {stats.GetFloatStat(nameof(friction))}");
        Debug.Log("------- Stats --------");
    }
}
