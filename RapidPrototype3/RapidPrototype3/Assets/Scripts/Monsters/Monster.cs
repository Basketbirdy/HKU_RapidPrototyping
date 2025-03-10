using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Monster : MonoBehaviour, IInteractable, ICarriable, IDamagable
{
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private MonsterData settings;

    [SerializeField] private Transform target;
    [SerializeField] private LayerMask collisionMask;

    [Header("Graphics")]
    [SerializeField] private MonsterGFX gfx;
    private bool isFlipped = false;

    // collision variables
    private CircleCollider2D coll;
    private bool isCollisionActive = true;

    // carriable properties
    public Transform CarriableTransform => transform;

    private bool isCarried;
    public bool IsCarried { get => isCarried; set => isCarried  = value; }

    // damagable properties
    private float currentHealth;
    public float CurrentHealth { get => currentHealth; }

    private bool isInvincible;
    public bool IsInvincible { get => isInvincible; set => isInvincible = value; }


    private void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
        settings = new MonsterData(monsterData);
        settings.name = gameObject.name;
    }

    private void Start()
    {
        if (!target.gameObject.activeSelf)
        {
            IsCarried = true;
        }
        settings.Setup(Object.FindFirstObjectByType<PlayerManager>().PlayerStats, gameObject);
        currentHealth = settings.health;
    }

    private void Update()
    {
        if (IsCarried) { return; }
        Move();
    }

    private void FixedUpdate()
    {
        if (!isCollisionActive) { return; }
        CollisionCheck();
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log($"Something inside of trigger; {collision.gameObject.name}");

    //    IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
    //    if (damagable == null) { return; }

    //    if (damagable.IsInvincible) { return; }
    //    damagable.TakeDamage(settings.damage);
    //}

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void CollisionCheck()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, coll.radius, collisionMask);
        if(hits.Length <= 0 ) { return; }

        foreach(Collider2D hit in hits)
        {
            IDamagable damagable = hit.gameObject.GetComponent<IDamagable>();
            if (damagable == null) { return; }

            if (damagable.IsInvincible) { return; }
            damagable.TakeDamage(settings.damage);
        }
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log($"Interacting with: {gameObject.name}");
        ICarrier carrier = interactor.GetComponentInChildren<ICarrier>();
        if (carrier != null) 
        {
            isCollisionActive = false;
            carrier.PickUp(this); 
        }
    }

    private void Move()
    {
        if(target == null) { return; }

        float step = Time.deltaTime * settings.speed;
        Vector2 newPos = Vector2.MoveTowards(transform.position, target.position, step);
        transform.position = newPos;
        //Debug.Log($"New position: {newPos}: Step: {step}", gameObject);
        EvaluateDirection();
    }

    private void EvaluateDirection()
    {
        Vector2 direction = target.position - transform.position;

        if(direction.normalized.x > 0 && !isFlipped) { isFlipped = true; gfx.Flip(true); }
        if(direction.normalized.x < 0 && isFlipped) { isFlipped = false;  gfx.Flip(false); }
    }

    public void OnCarry()
    {
        foreach(BaseMonsterEffect effect in settings.holdEffects) 
        {
            if(effect.EffectMoment == EffectMoment.ONCARRY)
            {
                effect.ApplyEffect();
            }
        }
    }

    public void OnThrow()
    {
        foreach (BaseMonsterEffect effect in settings.holdEffects)
        {
            if (effect.EffectMoment == EffectMoment.ONCARRY)
            {
                effect.RemoveEffect();
            }
        }
    }

    public void OnLanding()
    {
        Debug.Log($"{gameObject.name} landed! now executing landing effects");

        foreach (BaseMonsterEffect effect in settings.holdEffects)
        {
            if (effect.EffectMoment == EffectMoment.ONLANDING)
            {
                effect.ApplyEffect();
            }
        }

        isCollisionActive = true;
        IsCarried = false;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log($"Damaging {gameObject.name} for {damage} damage");

        float newHealth = currentHealth - damage;

        Debug.Log($"New health is {newHealth}");

        if (newHealth > 0)
        {
            currentHealth = newHealth;
            return;
        }

        Die();
    }

    public void Die() 
    {
        EventHandler.InvokeEvent(EventStrings.MONSTER_KILLED);
        gameObject.SetActive(false); 
    }
}

[System.Serializable]
public struct MonsterGFX
{
    public GFXPiece[] pieces;

    public void Flip(bool state)
    {
        foreach(GFXPiece piece in pieces)
        {
            piece.renderer.flipX = state;
        }
    }
}

[System.Serializable]
public struct GFXPiece
{
    public string name;
    public SpriteRenderer renderer;
}
