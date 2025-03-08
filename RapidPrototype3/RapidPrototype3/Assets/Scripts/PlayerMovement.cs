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
    public PlayerStats stats;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        physicsMovement = new PhysicsMovement(rb);

        FloatStat speedStat = new FloatStat(nameof(speed), speed);
        FloatStat accelerationStat = new FloatStat(nameof(acceleration), acceleration);
        FloatStat decelerationStat = new FloatStat(nameof(deceleration), deceleration);
        FloatStat frictionStat = new FloatStat(nameof(friction), friction);
        stats = new PlayerStats(speedStat, accelerationStat, decelerationStat, frictionStat);
    }

    private void Start()
    {
        
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
