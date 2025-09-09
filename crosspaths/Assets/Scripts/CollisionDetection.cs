using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
     public float slowMotionDuration = 2f;
    public string gameOverScreenTag = "GameOverScreen";
    public string gameOverScreenChildName = "GameOverScreenChild";
    private int layer1Crashes = 0;
    private float gracePeriod = 10f;
    private bool inGracePeriod = false;
    private float gracePeriodEndTime = 0f;
    private float gameOverCountdownTime = 33f;
    private float gameStartTime;
    private bool gameOverTriggered = false;
    private ScoreManager scoreManager;
    public bool crashMode = false;
     public AudioClip soundEffect;
    public AudioSource audioSource;

    private void Start()
    {
        audioSource.playOnAwake = false;  
        gameStartTime = Time.time;
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
    }

    private void Update()
    {
        if (crashMode && !gameOverTriggered && Time.time - gameStartTime >= gameOverCountdownTime)
        {
            TriggerGameOver();
            gameOverTriggered = true;
        }

        if (inGracePeriod && Time.time >= gracePeriodEndTime)
        {
            inGracePeriod = false;
            layer1Crashes = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (scoreManager != null && scoreManager.isGameOver)
        {
            return;
        }

        if (crashMode)
        {
            if (collision.gameObject.CompareTag("car"))
            {
                 if (audioSource != null && soundEffect != null)
        {
            audioSource.Play();
        }   
                if (scoreManager != null)
                {
                    scoreManager.AddScore(100);
                }
            }

            if (collision.gameObject.CompareTag("pedestrian"))
            {
                 if (audioSource != null && soundEffect != null)
        {
            audioSource.Play();
        }
                if (scoreManager != null)
                {
                    scoreManager.AddScore(200);
                }
            }
        }

        if (collision.gameObject.CompareTag("car"))
        {
            int thisLayer = gameObject.layer;
            int collidedLayer = collision.gameObject.layer;

            if (thisLayer != collidedLayer)
            {
                if (!inGracePeriod)
                {
                    layer1Crashes++;
                    Debug.Log($"Collision detected between cars on different layers: {LayerMask.LayerToName(thisLayer)} and {LayerMask.LayerToName(collidedLayer)}");

                    if (!crashMode && layer1Crashes >= 3)
                    {
                        TriggerGameOver();
                    }
                }
            }
        }

        if (!crashMode && collision.gameObject.CompareTag("pedestrian"))
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        Time.timeScale = 1.5f;
        Invoke("StopTimeScale", slowMotionDuration);

        GameObject gameOverScreen = GameObject.FindWithTag(gameOverScreenTag);
        if (gameOverScreen != null)
        {
            Transform child = gameOverScreen.transform.Find(gameOverScreenChildName);
            if (child != null)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Game over screen child not found! Ensure that the child object exists and has the correct name.");
            }
        }
        else
        {
            Debug.LogError("Game over screen not found! Ensure that an object with the correct tag exists.");
        }

        if (scoreManager != null)
        {
            scoreManager.SetGameOver();
        }
    }    
}
