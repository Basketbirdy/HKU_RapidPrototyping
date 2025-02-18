using UnityEngine;

public class CreatureVisage : MonoBehaviour, IScratchable
{
    [SerializeField] private bool isScratched;
    [HideInInspector] public bool IsScratched { get => isScratched; set => isScratched = value; }

    public void OnScratch()
    {
        Debug.Log($"Scratched {gameObject.name}");
    }
}
