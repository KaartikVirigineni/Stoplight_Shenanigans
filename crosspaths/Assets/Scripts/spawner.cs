using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnDirection
{
    PositiveX,
    NegativeX,
    PositiveZ,
    NegativeZ
}

public class spawner : MonoBehaviour
{
     public List<GameObject> prefabsToSpawn;
    public float spawnInterval = 2f;
    public Transform spawnPoint;
    public float gapBetweenObjects = 1.0f;
    public SpawnDirection spawnDirection = SpawnDirection.PositiveX;
    public Vector3 spawnRotationEuler = Vector3.zero;
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;
    public float minGap = 0.5f;
    public float maxGap = 2.0f;
    public GameObject warningObject;
    public float warningDisplayTime = 5f;
    private Vector3 nextSpawnPosition;
    private float timeInTrigger = 0f;
    private Coroutine warningCoroutine;   

    private void Start()
    {
        if (prefabsToSpawn.Count == 0)
        {
            Debug.LogError("No prefabs assigned to the prefabsToSpawn list.");
            return;
        }
        nextSpawnPosition = spawnPoint.position;

        if (warningObject != null)
        {
            warningObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Warning object not assigned.");
        }

        StartCoroutine(SpawnPrefab());
    }

    private IEnumerator SpawnPrefab()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2f, Quaternion.identity);
            int carCount = 0;
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("car"))
                {
                    carCount++;
                }
            }

            Debug.Log($"Number of cars in trigger zone: {carCount}");

            if (carCount < 4)
            {
                if (prefabsToSpawn.Count > 0)
                {
                    GameObject prefabToSpawn = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Count)];
                    float randomGap = Random.Range(minGap, maxGap);
                    GameObject spawnedObject = Instantiate(prefabToSpawn, nextSpawnPosition, Quaternion.Euler(spawnRotationEuler));
                    Debug.Log($"Spawned {prefabToSpawn.name} at {nextSpawnPosition}");

                    switch (spawnDirection)
                    {
                        case SpawnDirection.PositiveX:
                            nextSpawnPosition += new Vector3(prefabToSpawn.transform.localScale.x + randomGap, 0f, 0f);
                            break;
                        case SpawnDirection.NegativeX:
                            nextSpawnPosition -= new Vector3(prefabToSpawn.transform.localScale.x + randomGap, 0f, 0f);
                            break;
                        case SpawnDirection.PositiveZ:
                            nextSpawnPosition += new Vector3(0f, 0f, prefabToSpawn.transform.localScale.z + randomGap);
                            break;
                        case SpawnDirection.NegativeZ:
                            nextSpawnPosition -= new Vector3(0f, 0f, prefabToSpawn.transform.localScale.z + randomGap);
                            break;
                    }
                }
                else
                {
                    Debug.LogWarning("Prefab list is empty, no prefab to spawn.");
                }
            }

            float randomSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomSpawnInterval);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("car"))
        {
            timeInTrigger = 0f;
           
            if (warningCoroutine != null)
            {
                StopCoroutine(warningCoroutine);
            }
            warningCoroutine = StartCoroutine(HandleWarningDisplay());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("car"))
        {
            if (warningCoroutine != null)
            {
                StopCoroutine(warningCoroutine);
                warningCoroutine = null;
            }
            if (warningObject != null)
            {
                warningObject.SetActive(false);
            }
        }
    }

    private IEnumerator HandleWarningDisplay()
    {
        while (true)
        {
            timeInTrigger += Time.deltaTime;

            if (timeInTrigger >= warningDisplayTime - 5f)
            {
                if (warningObject != null)
                {
                    warningObject.SetActive(true);
                }
            }

            if (timeInTrigger >= warningDisplayTime - 0.01f)
            {
                if (warningObject != null)
                {
                    warningObject.SetActive(false);
                }
            }
            yield return null;
        }
    }
}
