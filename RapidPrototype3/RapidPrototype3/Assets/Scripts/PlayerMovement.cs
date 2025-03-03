using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [Header("Movement")]
    [SerializeField] private float baseSpeed;
    [SerializeField] private float baseAcceleration;
    [SerializeField] private float baseDeceleration;
    [SerializeField] private float baseFriction;


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
        //Stat speedStat = new Stat("Speed", baseSpeed);
        //Stat accelerationStat = new Stat("Acceleration", baseAcceleration);
        //Stat decelerationStat = new Stat("Deceleration", baseDeceleration);
        //Stat frictionStat = new Stat("Friction", baseFriction);
        stats = new PlayerStats(baseSpeed, baseAcceleration, baseDeceleration, baseFriction);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        GetInput();
        physicsMovement.Move(direction, stats.GetAcceleration(), stats.GetSpeed());
    }

    private void FixedUpdate()
    {
        physicsMovement.UpdatePhysics(stats.GetFriction(), stats.GetDeceleration());
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
        stats.SetSpeed(baseSpeed);
        stats.SetAcceleration(baseAcceleration);
        stats.SetDeceleration(baseDeceleration);
        stats.SetFriction(baseFriction);
    } 
}
