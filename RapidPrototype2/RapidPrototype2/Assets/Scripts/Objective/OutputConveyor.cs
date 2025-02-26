using UnityEngine;
using System.Collections.Generic;

public class OutputConveyor : MonoBehaviour
{
    [SerializeField] private int conveyorCount = 6;
    [SerializeField] private float conveyorDistance = 6;
    [SerializeField] private ConveyorSlot[] conveyorSlots;

    [SerializeField] private GameObject objectPrefab;

    [SerializeField] private float spawnInterval = 3f;
    private float spawnTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        conveyorSlots = new ConveyorSlot[conveyorCount];
        for(int i = 0; i < conveyorCount; i++)
        {
            conveyorSlots[i] = new ConveyorSlot(i, Direction.LEFT, conveyorDistance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnOnTimer();
        UpdateConveyors();
    }

    public void SpawnOnTimer()
    {
        if (Time.time < spawnTimer) { return; }

        // spawn object and assign it to conveyor slot 1
        if (!conveyorSlots[0].CheckSlot()) 
        {
            GameObject newObject = Instantiate(objectPrefab);
            conveyorSlots[0].AssignSlot(newObject);
        }

        spawnTimer = Time.time + spawnInterval; // reset timer

    }

    public void UpdateConveyors()
    {
        for(int i = conveyorCount - 1; i >= 0; i--)
        {
            if(i == 0) { continue; }

            // check if current slot is filled
            if (conveyorSlots[i].CheckSlot()) { continue; }

            // check if conveyor before is filled
            if (!conveyorSlots[i - 1].CheckSlot()) { continue; }

            // if yes, move that item to this conveyor
            GameObject itemToMove = conveyorSlots[i - 1].GetObject();
            conveyorSlots[i - 1].EmptySlot();
            conveyorSlots[i].AssignSlot(itemToMove);
        }
    }
}
