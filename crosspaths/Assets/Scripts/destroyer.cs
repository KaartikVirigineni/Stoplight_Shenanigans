using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyer : MonoBehaviour
{
  public ScoreManager scoreManager;
  public bool ischaosmode;

    private void OnTriggerEnter(Collider other)
    {
        if (!ischaosmode && other.CompareTag("car"))
        {
            scoreManager.AddScore(100);
        }
        else if (!ischaosmode && other.CompareTag("pedestrian"))
        {
            scoreManager.AddScore(150);
        }

        Destroy(other.gameObject);
    }
}
