using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1Zombie : Zombie {
    [SerializeField] private float maxSpeed = 1.0f;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float maxSpeedOffset = 1.0f;

    protected override void Chase() {

        if (!agent.enabled) return;

        var nextPoint = agent.steeringTarget;
        Vector3 targetDir = nextPoint - transform.position;
        var distance = targetDir.magnitude;

        //移動速度の決定。一定の距離以内であれば速度を落とす
        if (maxSpeedOffset < distance) {
            speed = maxSpeed;
        }
        else {
            speed = maxSpeed * (distance / maxSpeedOffset);
        }

        // その方向に向けて旋回する(120度/秒)
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f * Time.deltaTime);

        // 自分の向きと次の位置の角度差が30度以上の場合、その場で旋回
        float angle = Vector3.Angle(targetDir, transform.forward);
        if (angle < 30f) {
            transform.position += transform.forward * speed * Time.deltaTime;
            // もしもの場合の補正
            //if (Vector3.Distance(nextPoint, transform.position) < 0.5f)　transform.position = nextPoint;
        }

        var axis = Vector3.Cross(transform.forward, targetDir);
        var horizontalValue = angle * (axis.y < 0 ? -1 : 1);
        var forwardValue = speed / maxSpeed;

        animator.SetFloat("Forward", forwardValue);
        animator.SetFloat("Horizontal", horizontalValue);

        //Debug.Log($"next Point : {nextPoint}, targetDir : {targetDir}, targetRotation : {targetRotation}");
        //Debug.Log($"angle : {angle}");

        // targetに向かって移動します。
        agent.SetDestination(target.position);
        agent.nextPosition = transform.position;
    }

    public override void Attack(Collider other) {
        base.Attack(other);
    }
}