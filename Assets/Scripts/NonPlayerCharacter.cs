using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class NonPlayerCharacter : MonoBehaviour
{
    public AudioSource talking;
    public AudioClip talkingsounds;
    public float displayTime = 6.0f;
    public GameObject dialogBox;
    float timerDisplay;

    void Start()
    {
        talking = GetComponent<AudioSource>();
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
    }

    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
                talking.Stop();
            }
        }
    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
        talking.Play();
    }
}
