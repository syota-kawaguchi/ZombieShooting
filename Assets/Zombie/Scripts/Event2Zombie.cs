using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event2Zombie : Zombie
{
    [Header("Original")]
    [SerializeField] private bool movable;

    new private void Start() {
        base.Start();
    }

    public void OnStartDirector() {
        animator.SetBool("StandUp", true);
    }

    public override void Activate() {
        base.Activate();
        animator.SetBool("Activate", true);
    }
}
