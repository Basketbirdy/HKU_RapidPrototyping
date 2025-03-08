using UnityEngine;

public class Monster : MonoBehaviour, IInteractable, ICarriable
{
    [SerializeField] private MonsterData settings;

    [SerializeField] private Transform target;

    public Transform CarriableTransform => transform;

    private bool isCarried;
    public bool IsCarried { get => isCarried; set => isCarried  = value; }

    private void Awake()
    {
        settings.SetupStats(Object.FindFirstObjectByType<PlayerMovement>().stats);
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log($"Interacting with: {gameObject.name}");
        ICarrier carrier = interactor.GetComponentInChildren<ICarrier>();
        if (carrier != null) { carrier.PickUp(this); }
    }

    private void Update()
    {
        if (IsCarried) { return; }
        Move();
    }

    private void Move()
    {
        float step = Time.deltaTime * settings.speed;
        Vector2 newPos = Vector2.MoveTowards(transform.position, target.position, step);
        transform.position = newPos;
        //Debug.Log($"New position: {newPos}: Step: {step}", gameObject);
    }

    public void OnCarry()
    {
        foreach(BaseMonsterEffect effect in settings.holdEffects) 
        {
            if(effect.EffectMoments == EffectMoments.ONCARRY)
            {
                effect.ApplyEffect();
            }
        }
    }

    public void OnThrow()
    {
        foreach (BaseMonsterEffect effect in settings.holdEffects)
        {
            if (effect.EffectMoments == EffectMoments.ONCARRY)
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
            if (effect.EffectMoments == EffectMoments.ONLANDING)
            {
                effect.ApplyEffect();
            }
        }

        IsCarried = false;
    }
}
