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
    [SerializeField] private bool isMoving;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    private PhysicsMovement physicsMovement;

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
        onWeightChange += UpdateMultipliers;
        physicsMovement = new PhysicsMovement(GetComponent<Rigidbody2D>());
    }

    private void OnDisable()
    {
        onWeightChange -= UpdateMultipliers;
    }

    private void Start()
    {
        currentWeight = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        if (Input.GetKeyDown(KeyCode.O)) { AddWeight(20); }
        if (Input.GetKeyDown(KeyCode.P)) { RemoveWeight(20); }
    }

    private void FixedUpdate()
    {
        physicsMovement.UpdatePhysics(friction * frictionMultiplier, deceleration * decelerationMultiplier);
        physicsMovement.Move(direction, acceleration * accelerationMultiplier, speed * speedMultiplier);
    }

    public bool AddWeight(float weight)
    {
        if(currentWeight + weight <= maxWeight) { currentWeight = currentWeight + weight; }
        else { return false; }

        onWeightChange?.Invoke();
        return true;
    }

    public bool RemoveWeight(float weight)
    {
        currentWeight -= weight;
        if(currentWeight < 0f) {  currentWeight = 0f; return false; }

        onWeightChange?.Invoke();
        return true;
    }

    private void UpdateMultipliers()
    {
        weightRatio = MathUtils.Remap(currentWeight, 0f, maxWeight);

        speedMultiplier = 1f - speedDropoff.Evaluate(weightRatio);
        accelerationMultiplier = 1f - accelerationDropoff.Evaluate(weightRatio);
        decelerationMultiplier = 1f - decelerationDropoff.Evaluate(weightRatio);
        frictionMultiplier = 1f - frictionDropoff.Evaluate(weightRatio);
    }

    private void GetInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        direction = new Vector2(xInput, yInput).normalized;
    }
}
