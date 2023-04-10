using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivateTrigger : MonoBehaviour
{
    [SerializeField] private Zombie[] zombies;
    [SerializeField] private UnityEvent activateEvent = new UnityEvent();

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            foreach(var zombie in zombies) {
                if (zombie == null) continue;
                if (!zombie.gameObject.activeSelf) zombie.gameObject.SetActive(true);
                zombie.Activate();
            }
            activateEvent.Invoke();
        }
    }
}
