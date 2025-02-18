using System;
using System.Collections.Generic;
using UnityEngine;

public class PageGenerator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int pageAmount = 10;
    [SerializeField] private int corruptedAmount;
    [SerializeField] private Color[] pageColors;

    [Header("Objects")]
    [SerializeField] private GameObject pagePrefab;
    [SerializeField] private Creature[] creatures;

    [Header("Runtime")]
    [SerializeField] private Dictionary<Page, int> pages = new Dictionary<Page, int>();
    private int currentCorrupted;
    private Creature selectedCreature;

    private void OnEnable()
    {
         GameManager.onStartGame += OnStartGame;
    }

    private void OnDisable()
    {
        GameManager.onStartGame -= OnStartGame;
    }

    private void Start()
    {
        if(corruptedAmount > pageAmount) { Debug.LogWarning($"Trying to assign more monsters than there are pages, defaulting to all pages"); }
    }

    private void OnStartGame()
    {
        GeneratePages();
    }

    private void GeneratePages()
    {
        float zOffset = 0;

        for(int i = 0;  i < pageAmount; i++) 
        {

            // randomize corruption
            if(currentCorrupted < corruptedAmount)
            {
                if(i < pageAmount - (corruptedAmount - currentCorrupted))
                {
                    float rand = UnityEngine.Random.value;
                    if(rand > .5f) 
                    {
                        selectedCreature = SelectCreature(); 
                        currentCorrupted++;
                    }
                }
                else
                {
                    selectedCreature = SelectCreature();
                    currentCorrupted++;
                }
            }

            Vector3 pagePos = new Vector3(transform.position.x, transform.position.y, transform.position.z + zOffset);
            zOffset += 1f;
            Page page = Instantiate(pagePrefab, pagePos, Quaternion.identity).GetComponent<Page>();
            page.SetupPage(selectedCreature, SelectColor());
            selectedCreature = null;
        }

        currentCorrupted = 0;
    }

    private Creature SelectCreature()
    {
        if(creatures.Length == 0) 
        {
            Debug.LogWarning($"Could not find creature");
            return null;
        }

        int randIndex = UnityEngine.Random.Range(0, creatures.Length);
        return creatures[randIndex];
    }

    private Color SelectColor()
    {
        if (pageColors.Length == 0)
        {
            Debug.LogWarning($"Could not find creature");
            return Color.white;
        }

        int randIndex = UnityEngine.Random.Range(0, pageColors.Length);
        return pageColors[randIndex];
    }
}
