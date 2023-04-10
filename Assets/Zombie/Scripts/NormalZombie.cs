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

        //目的地への方向を取得
        var direction = target.position - transform.position;
        var normDirection = direction.normalized;

        //目的地への方向と自身の向きの差分を取得
        var diff = normDirection - transform.forward;

        //回転方向(+ : 右回り, - : 左回り)
        var rotateDirection = (diff.x + diff.z) >= 0 ? 1 : -1;
        var horizontalValue = diff.magnitude * rotateDirection;
        var forwardValue = agent.velocity.magnitude;

        animator.SetFloat("Horizontal", horizontalValue);
        animator.SetFloat("Forward", forwardValue); //z方向は進むか進まないか判定する。よって0~1の範囲にしている
    }
}
