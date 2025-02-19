using UnityEngine;

public class EndGameBell : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        // TODO - Play bell sound

        EventHandler.InvokeEvent(EventTypes.GAME_END_TRIGGER);
    }
}
