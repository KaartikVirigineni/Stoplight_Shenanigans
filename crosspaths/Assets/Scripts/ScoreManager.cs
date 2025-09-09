using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public Text scoreText;
    public bool isGameOver = false;

   public Animator animator;
    public string animationName = "YourAnimation";

    public void Start(){

         if (animator != null && !string.IsNullOrEmpty(animationName))
        {
            animator.Play(animationName);
        }
    }
    public void AddScore(int amount)
    {
        if (!isGameOver)
        {
            score += amount;
            UpdateScoreDisplay();
        }
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
    public void SetGameOver()
    {
        isGameOver = true;
    }
}
