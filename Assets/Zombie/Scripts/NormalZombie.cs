using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombie : Zombie
{
    [Header("Original Parameter")]
    [SerializeField] private int walkType = 0;

    new private void Start() {
        base.Start();
        animator.SetInteger("WalkType", walkType);
    }

    protected override void Chase() {
        if (!agent.enabled) return;

        if (!agent.isOnNavMesh) {
            return;
        }

        agent.destination = target.position;

        //�ړI�n�ւ̕������擾
        var direction = target.position - transform.position;
        var normDirection = direction.normalized;

        //�ړI�n�ւ̕����Ǝ��g�̌����̍������擾
        var diff = normDirection - transform.forward;

        //��]����(+ : �E���, - : �����)
        var rotateDirection = (diff.x + diff.z) >= 0 ? 1 : -1;
        var horizontalValue = diff.magnitude * rotateDirection;
        var forwardValue = agent.velocity.magnitude;

        animator.SetFloat("Horizontal", horizontalValue);
        animator.SetFloat("Forward", forwardValue); //z�����͐i�ނ��i�܂Ȃ������肷��B�����0~1�͈̔͂ɂ��Ă���
    }
}
