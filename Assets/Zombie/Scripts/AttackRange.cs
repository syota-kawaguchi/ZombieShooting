using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private Zombie zombie;
    private void OnTriggerEnter(Collider other) {
        if (other.tag == zombie.getTargetTag) {
            zombie.ModeChangeAttack(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == zombie.getTargetTag) {
            zombie.ModeChangeAttack(false);
        }
    }
}
