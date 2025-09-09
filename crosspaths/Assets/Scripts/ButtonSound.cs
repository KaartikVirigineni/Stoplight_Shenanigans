using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioClip soundEffect;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = soundEffect;
        Button button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(PlaySoundEffect);
        }
        else
        {
            Debug.LogWarning("no button");
        }
    }

    void PlaySoundEffect()
    {
        if (audioSource != null && soundEffect != null){
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("audiosource not set.");
        }
    }
}
