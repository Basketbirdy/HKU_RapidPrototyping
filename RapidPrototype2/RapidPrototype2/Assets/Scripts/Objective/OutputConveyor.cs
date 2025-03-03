using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System.Collections;

public class OutputConveyor : MonoBehaviour
{
    [Header("Conveyor settings")]
    [SerializeField] private Direction conveyorDirection = Direction.LEFT;

    [SerializeField] private int conveyorCount = 6;
    [SerializeField] private float conveyorDistance = 6;

    [SerializeField] private float conveyorMoveTolerance = .1f;
    [SerializeField] private float conveyorMoveSpeed = 5f;
    
    // private 
    private Conveyor conveyor;

    [Header("Spawning")]
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private float spawnInterval = 3f;
    private float spawnTimer;

    private void Awake()
    {
        conveyor = new Conveyor(this, conveyorDirection, conveyorCount, conveyorDistance, conveyorMoveTolerance, conveyorMoveSpeed);
    }

    private void Start()
    {
        //spawnTimer = Time.time + spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnOnTimer();
        conveyor.Execute();
    }

    public void SpawnOnTimer()
    {
        if (Time.time < spawnTimer) { return; }

        // spawn object and assign it to conveyor slot 1
        if (!conveyor.conveyorSlots[0].CheckSlot()) 
        {
            GameObject newObject = Instantiate(objectPrefab, conveyor.conveyorSlots[0].GetPosition(), Quaternion.identity);
            conveyor.conveyorSlots[0].AssignSlot(newObject);
        }

        spawnTimer = Time.time + spawnInterval; // reset timer

    }

    //public void UpdateConveyors()
    //{
    //    for(int i = conveyorCount - 1; i >= 0; i--)
    //    {
    //        if(i == 0) { continue; }

    //        // check if current slot is filled
    //        if (conveyorSlots[i].CheckSlot()) { continue; }

    //        // check if conveyor before is filled
    //        if (!conveyorSlots[i - 1].CheckSlot()) { continue; }

    //        // if yes, move that item to this conveyor
    //        GameObject itemToMove = conveyorSlots[i - 1].GetObject();

    //        StartCoroutine(MoveItem(itemToMove, conveyorMoveSpeed, conveyorMoveTolerance, i));
    //    }
    //}

    //public IEnumerator MoveItem(GameObject itemToMove, float speed, float tolerance, int i)
    //{
    //    bool movingItem = true;

    //    Vector2 target = conveyorSlots[i].GetPosition();

    //    while (movingItem)
    //    {
    //        if(itemToMove == null) { break; }

    //        itemToMove.transform.position = Vector3.MoveTowards(itemToMove.transform.position, target, speed * Time.deltaTime);
            
    //        if(itemToMove.transform.position.x <= target.x + tolerance && itemToMove.transform.position.x > target.x - tolerance &&
    //            itemToMove.transform.position.y <= target.y + tolerance && itemToMove.transform.position.y > target.y - tolerance)
    //        {
    //            movingItem = false;
    //            itemToMove.transform.position = target;
    //        }

    //        yield return null;
    //    }

    //    conveyorSlots[i - 1].EmptySlot();
    //    if(itemToMove != null)
    //    {
    //        conveyorSlots[i].AssignSlot(itemToMove);
    //    }
    //}
}
