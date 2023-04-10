using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Zombie
{
    public override void Attack(Collider other) {
        if (currentState == STATE_TYPE.Attack) {
            if (other.TryGetComponent(out IDamageable target)) {
                target.Damage(attackPower);
            }
        }
    }

    protected override void Update() {
        base.Update();

        var distance = transform.position - target.position;
        animator.SetFloat("Distance", distance.magnitude);
    }
}
