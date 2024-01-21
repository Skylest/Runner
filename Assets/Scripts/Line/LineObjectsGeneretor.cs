using System;
using UnityEngine;


public class LineObjectsGeneretor : MonoBehaviour
{
    [SerializeField]
    private LineItem[] lineObjects;

    private float x = -3f;
    private float z = 25f;
    private readonly float xStep = 3f;
    private readonly float zStep = 10f;

    // This will be appended to the name of the created entities and increment when each is created.
    private int instanceNumber = 1;
    private int numberOfPrefabsToCreate = 3;
    int a = 40;
    int b = 0;

    private void Update()
    {
        if (b < a)
        {
            SpawnEntities();
            b++;
        }
    }

    private void SpawnEntities()
    {
        x = -3f;
        LineItem[] linePoolToCreate = GetPool();
        for (int i = 0; i < numberOfPrefabsToCreate; i++)
        {
            float y = linePoolToCreate[i].gameObjectPrefab.transform.position.y;
            Vector3 spawnPoint = new Vector3(x, y, z);
            // Creates an instance of the prefab at the current spawn point.
            GameObject currentEntity = Instantiate(linePoolToCreate[i].gameObjectPrefab, spawnPoint, Quaternion.identity);

            // Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number. 
            currentEntity.name = linePoolToCreate[i].name + instanceNumber;

            instanceNumber++;
            x += xStep;
        }
        z += zStep;
    }

    private LineItem[] GetPool()
    {
        LineItem[] itemsPool = new LineItem[numberOfPrefabsToCreate];
        
        int countOfImpassable = 0;
        int prefabsCreated = 0;
        while (prefabsCreated < numberOfPrefabsToCreate)
        {
            int index = UnityEngine.Random.Range(0, lineObjects.Length);

            if (lineObjects[index].type == LineItem.Type.Impassable && countOfImpassable < 2)
            {
                countOfImpassable++;
                itemsPool[prefabsCreated] = lineObjects[index];
                prefabsCreated++;
            }
            else if (lineObjects[index].type == LineItem.Type.Passable)
            {
                itemsPool[prefabsCreated] = lineObjects[index];
                prefabsCreated++;
            }            
        }

        return itemsPool;
    }
}

