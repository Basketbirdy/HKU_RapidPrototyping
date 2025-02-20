using UnityEngine;

public class EndGameBell : MonoBehaviour, IClickable
{
    [SerializeField] private bool clicked;

    public void OnClick()
    {
        // TODO - Play bell sound
        AudioManager.instance.Play("Bell");

        if (clicked) { return; }

        clicked = true;
        EventHandler.InvokeEvent(EventTypes.GAME_END_TRIGGER);
    }
}
