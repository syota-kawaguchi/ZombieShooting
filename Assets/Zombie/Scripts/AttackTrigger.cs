using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField] private Zombie zombie;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == zombie.getTargetTag) {
            zombie.Attack(other);
        }
    }
}
