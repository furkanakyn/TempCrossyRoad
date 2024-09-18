using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    [Header("TRANSFORM")]
    public Transform cars;
    public Transform map;
    [Header("VALUE")]
    public int mapXLength;
    public int mapZLength;
    public int safeZoneLength;
    public int lastRowCount;
    public int diffucultyScalingRowCount = 5;
    public float treeSpawnChance;
    public float minCarTravelcarTravelDuration = 3f;
    public float minCarGenerationFreq = 2f;

    [Header("PREFABS")]
    public Block asphaltBlockPrefab;
    public Block grassBlockPrefab;
    public Transform coinPrefab;

    public Tree treePrefab;
    public Car carPrefab;


    public List<Coroutine> carsCoroutines = new List<Coroutine>();

    public void StartMapGenerator()
    {
        AddNewRows(mapZLength);
    }
    public void AddNewRows(int rowCount)
    {
        for (int z = 0; z < rowCount; z++)
        {
            if (z < safeZoneLength)
            {
                GenerateGrassRow();
            }
            else
            {
                if (Random.value < .5f)
                {
                    GenerateAsphaltRow(true);
                    for (int i = 0; i < Random.Range(1, 6); i++)
                    {
                        GenerateAsphaltRow(false);
                    }
                    GenerateGrassRow();
                }
                else
                {
                    GenerateGrassRow();
                }
            }
        }
    }
    public void GenerateAsphaltRow(bool IsStripHidden)
    {
        var newRow = new GameObject("Asphalt Row");
        for (int i = 0; i < mapXLength; i++)
        {
            var newBlock = Instantiate(asphaltBlockPrefab, newRow.transform);
            newBlock.transform.position = new Vector3(i, 0, lastRowCount);
            if (IsStripHidden)
            {
                newBlock.SeritiGizle();
            }
        }
        
        var carTravelcarTravelDuration = Random.Range(8.5f, 14.5f) - lastRowCount / diffucultyScalingRowCount;
        var carGenerationFreq = Random.Range(3f, 10f) - lastRowCount / diffucultyScalingRowCount;

        carTravelcarTravelDuration = Mathf.Max(carTravelcarTravelDuration, minCarTravelcarTravelDuration - Random.Range(0f, 1f));
        carGenerationFreq = Mathf.Max(carGenerationFreq, minCarGenerationFreq - Random.Range(0f, 1f));
        if (lastRowCount > 20 && Random.value < .5f)
        {
            if (Random.value < .5f)
            {
                carTravelcarTravelDuration = Random.Range(10f, 14f);
                carGenerationFreq = Random.Range(2f, 3f);
            }
            else
            {
                carTravelcarTravelDuration = Random.Range(7f, 8f);
                carGenerationFreq = Random.Range(10f, 11f);
            }

        }
        var newCoroutine = StartCoroutine(GenerateCarCoroutine
            (Random.value < .5f, carTravelcarTravelDuration, carGenerationFreq, lastRowCount, newRow));
        
        
        carsCoroutines.Add(newCoroutine);
        newRow.transform.SetParent(map);
        lastRowCount += 1;
    }

    IEnumerator GenerateCarCoroutine(bool toLeft, float carTravelDuration, float carGenerationFreq, int row,GameObject newRow)
    {
        while (true)
        {
            var newCar = Instantiate(carPrefab, newRow.transform);
            newCar.StartCar(row, toLeft, carTravelDuration);
            yield return new WaitForSeconds(carGenerationFreq);
        }

    }

    public void GenerateGrassRow()
    {

        var newRow = new GameObject("Grass Row");
        for (int i = 0; i < mapXLength; i++)
        {
            var newBlock = Instantiate(grassBlockPrefab, newRow.transform);
            newBlock.transform.position = new Vector3(i, 0, lastRowCount);
            if (lastRowCount > safeZoneLength)
            {
                if (Random.value < treeSpawnChance)
                {
                    var newTree = Instantiate(treePrefab, newRow.transform);
                    newTree.gameObject.transform.position = new Vector3(i, 0, lastRowCount);
                }
                else if (Random.value < .2f)
                {
                    var newCoin = Instantiate(coinPrefab, newRow.transform);
                    newCoin.position = new Vector3(i, 0, lastRowCount);
                }
            }
        }
        newRow.transform.SetParent(map);
        lastRowCount += 1;
    }
    public void DeleteMap()
    {
        foreach (Transform t in map)
        {
            Destroy(t.gameObject);
        }
        foreach (Transform s in cars)
        {
            Destroy(s.gameObject);
        }
        foreach (Coroutine c in carsCoroutines)
        {
            StopCoroutine(c);
        }
        carsCoroutines.Clear();
        lastRowCount = 0;
    }
    public void DeleteRow()
    {
        var tempList = new List<GameObject>();
        foreach (Transform row in map)
        {
            tempList.Add(row.gameObject);
        }
        if (tempList[0].name == "Asphalt Row")
        {
            StopCoroutine(carsCoroutines[0]);
            carsCoroutines.RemoveAt(0);
        }
        Destroy(tempList[0]);
    }
}