using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonClick : MonoBehaviour
{
    public AudioSource _audio;
    public AudioClip correctSFX;
    public AudioClip wrongSFX;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void OnButtonClick(bool isCorrect)
    {
        if (isCorrect)
        {
            _audio.PlayOneShot(correctSFX);
        }
        else
        {
            _audio.PlayOneShot(wrongSFX);
        }
    }
}
