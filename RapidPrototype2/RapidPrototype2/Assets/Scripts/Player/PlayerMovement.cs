using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float friction;

    [Header("Weight")]
    [SerializeField] private float currentWeight;
    [SerializeField] private float maxWeight;
    [SerializeField] private AnimationCurve speedDropoff;
    [SerializeField] private AnimationCurve accelerationDropoff;
    [SerializeField] private AnimationCurve decelerationDropoff;
    [SerializeField] private AnimationCurve frictionDropoff;

    [Header("States")]
    [SerializeField] private bool isDisabled;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    private PhysicsMovement physicsMovement;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem particles;

    // movement
    private float xInput;
    private float yInput;
    private Vector2 direction;

    // weight
    private float weightRatio = 1f;

    [Space(30)]
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float accelerationMultiplier = 1f;
    [SerializeField] private float decelerationMultiplier = 1f;
    [SerializeField] private float frictionMultiplier = 1f;

    private Action onWeightChange;

    private void Awake()
    {
        physicsMovement = new PhysicsMovement(GetComponent<Rigidbody2D>());
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = animator.gameObject.GetComponent<SpriteRenderer>();
        particles = GetComponentInChildren<ParticleSystem>();

        EventHandler<float>.AddListener(EventStrings.PLAYER_WEIGHT_ADD, AddWeight);
        EventHandler<float>.AddListener(EventStrings.PLAYER_WEIGHT_REMOVE, RemoveWeight);
        onWeightChange += UpdateMultipliers;
        physicsMovement.onMovingChange += UpdateAnimations;
    }

    private void OnDisable()
    {
        onWeightChange -= UpdateMultipliers;
        physicsMovement.onMovingChange -= UpdateAnimations;
        EventHandler<float>.RemoveListener(EventStrings.PLAYER_WEIGHT_ADD, AddWeight);
        EventHandler<float>.RemoveListener(EventStrings.PLAYER_WEIGHT_REMOVE, RemoveWeight);
    }

    private void Start()
    {
        currentWeight = 0f;
        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverDistance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        //if (Input.GetKeyDown(KeyCode.O)) { AddWeight(20); }
        //if (Input.GetKeyDown(KeyCode.P)) { RemoveWeight(20); }
    }

    private void FixedUpdate()
    {
        physicsMovement.UpdatePhysics(friction * frictionMultiplier, deceleration * decelerationMultiplier);
        physicsMovement.Move(direction, acceleration * accelerationMultiplier, speed * speedMultiplier);

        if(direction.x > 0 && spriteRenderer.flipX) { spriteRenderer.flipX = false; }
        else if(direction.x < 0 && !spriteRenderer.flipX) { spriteRenderer.flipX = true; }
    }

    public void AddWeight(float weight)
    {
        if(currentWeight + weight > maxWeight) { return; }

        currentWeight = currentWeight + weight;
        onWeightChange?.Invoke();
    }

    public void RemoveWeight(float weight)
    {
        currentWeight -= weight;

        if(currentWeight < 0f) { currentWeight = 0f;}
        onWeightChange?.Invoke();
    }

    private void UpdateMultipliers()
    {
        weightRatio = MathUtils.Remap(currentWeight, 0f, maxWeight);

        speedMultiplier = 1f - speedDropoff.Evaluate(weightRatio);
        accelerationMultiplier = 1f - accelerationDropoff.Evaluate(weightRatio);
        decelerationMultiplier = 1f - decelerationDropoff.Evaluate(weightRatio);
        frictionMultiplier = 1f - frictionDropoff.Evaluate(weightRatio);

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverDistance = weightRatio * 1.2f;
    }

    private void GetInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        direction = new Vector2(xInput, yInput).normalized;
    }

    private void UpdateAnimations()
    {
        animator.SetBool("isMoving", physicsMovement.GetMoving());
    }
}
