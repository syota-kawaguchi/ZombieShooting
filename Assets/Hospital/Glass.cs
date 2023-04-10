using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioManager;

public class Glass : MonoBehaviour, IDamageable {
    public void Damage(int damage) {
        SEManager.Instance.Play(SEPath.BROKEN_GLASS);
        Destroy(this.gameObject);
    }
}
