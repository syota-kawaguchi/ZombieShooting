using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineActivator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayableDirector director;
    [SerializeField] private bool isOnce = true;
    [SerializeField] private Zombie bindingZombie;
    private bool alreadyPlayed = false;

    public void Activate() {
        if (!director) return;
        if (isOnce && alreadyPlayed) return;

        alreadyPlayed = true;
        if (bindingZombie && bindingZombie.IsDie) return;
        director.Play();
    }
}
