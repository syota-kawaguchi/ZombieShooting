using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip clampDoorClip;

    void Start()
    {
        if (!audioSource) {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        
    }

    public void ClampDoorSound() {
        if (!clampDoorClip) return;

        audioSource.PlayOneShot(clampDoorClip);
    }
}
