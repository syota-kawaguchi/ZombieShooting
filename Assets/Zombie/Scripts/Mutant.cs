using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutant : Zombie
{
    public override void Activate() {
        base.Activate();
        animator.SetBool("Activate", true);
    }
}
