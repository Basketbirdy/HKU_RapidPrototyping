using UnityEngine;

public class Machine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something entered machine trigger", gameObject);
        ICarriable carriable = collision.gameObject.GetComponent<ICarriable>();
        if(carriable != null) 
        {
            Destroy(carriable.CarriableTransform.gameObject);
            GameManager.instance.AddTime();
        }
    }
}
