using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pederstrians : MonoBehaviour
{
     public List<GameObject> pedestrianPrefabs;
    public Transform spawnPoint;
    public Transform targetPoint1;
    public Transform targetPoint2;
    public Transform targetPoint3;
    public Transform targetPoint4;
    public float spawnInterval = 100f;
    public int minGroupSize = 3;
    public int maxGroupSize = 5;
    public float moveSpeed = 3f;
    public TrafficLightController walkLight;
    public float waitTimeAtSecondPoint = 10f;
    public GameObject warningObject;
    public float warningTimeBeforeCross = 5f;

    private void Start()
    {
        StartCoroutine(SpawnPedestrianGroups());
    }

    private IEnumerator SpawnPedestrianGroups()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            int groupSize = Random.Range(minGroupSize, maxGroupSize + 1);

            for (int i = 0; i < groupSize; i++)
            {
                GameObject pedestrianPrefab = pedestrianPrefabs[Random.Range(0, pedestrianPrefabs.Count)];
                GameObject pedestrian = Instantiate(pedestrianPrefab, spawnPoint.position, Quaternion.identity);
                StartCoroutine(MovePedestrian(pedestrian));
            }
        }
    }

    private IEnumerator MovePedestrian(GameObject pedestrian)
    {
        Transform pedestrianTransform = pedestrian.transform;
        yield return StartCoroutine(MoveToPoint(pedestrianTransform, targetPoint1.position));
        yield return StartCoroutine(MoveToPoint(pedestrianTransform, targetPoint2.position));
        float waitTimer = 0f;
        bool autoCrossing = false;

        while (walkLight.isRed && waitTimer < waitTimeAtSecondPoint)
        {
            if (waitTimeAtSecondPoint - waitTimer <= warningTimeBeforeCross)
            {
                if (warningObject != null)
                {
                    warningObject.SetActive(true);
                }
            }

            waitTimer += Time.deltaTime;
            yield return null;
        }

        if (warningObject != null)
        {
            warningObject.SetActive(false);
        }

        yield return StartCoroutine(MoveToPoint(pedestrianTransform, targetPoint3.position));
        yield return StartCoroutine(MoveToPoint(pedestrianTransform, targetPoint4.position));
        Destroy(pedestrian);
    }

    private IEnumerator MoveToPoint(Transform pedestrianTransform, Vector3 targetPosition)
    {
        while (Vector3.Distance(pedestrianTransform.position, targetPosition) > 0.1f)
        {
            pedestrianTransform.position = Vector3.MoveTowards(pedestrianTransform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
