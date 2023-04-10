using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTreeFPS;

public class Anomaly : MonoBehaviour
{
    private Transform player;
    private AudioSource audioSource;
    private Animator headBobAnimator;

    public AudioClip anomalyEnterReaction;

    private void Start()
    {
        player = FindObjectOfType<DarkTreeFPS.FPSController>().transform;
        audioSource = GetComponent<AudioSource>();
        headBobAnimator = Camera.main.GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            headBobAnimator.Play("CameraKick");
            player.GetComponent<IDamageable>().Damage(20);
            audioSource.PlayOneShot(anomalyEnterReaction);
        }

    }
}
