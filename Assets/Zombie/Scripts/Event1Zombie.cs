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

        //�ړ����x�̌���B���̋����ȓ��ł���Α��x�𗎂Ƃ�
        if (maxSpeedOffset < distance) {
            speed = maxSpeed;
        }
        else {
            speed = maxSpeed * (distance / maxSpeedOffset);
        }

        // ���̕����Ɍ����Đ��񂷂�(120�x/�b)
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 120f * Time.deltaTime);

        // �����̌����Ǝ��̈ʒu�̊p�x����30�x�ȏ�̏ꍇ�A���̏�Ő���
        float angle = Vector3.Angle(targetDir, transform.forward);
        if (angle < 30f) {
            transform.position += transform.forward * speed * Time.deltaTime;
            // �������̏ꍇ�̕␳
            //if (Vector3.Distance(nextPoint, transform.position) < 0.5f)�@transform.position = nextPoint;
        }

        var axis = Vector3.Cross(transform.forward, targetDir);
        var horizontalValue = angle * (axis.y < 0 ? -1 : 1);
        var forwardValue = speed / maxSpeed;

        animator.SetFloat("Forward", forwardValue);
        animator.SetFloat("Horizontal", horizontalValue);

        //Debug.Log($"next Point : {nextPoint}, targetDir : {targetDir}, targetRotation : {targetRotation}");
        //Debug.Log($"angle : {angle}");

        // target�Ɍ������Ĉړ����܂��B
        agent.SetDestination(target.position);
        agent.nextPosition = transform.position;
    }

    public override void Attack(Collider other) {
        base.Attack(other);
    }
}