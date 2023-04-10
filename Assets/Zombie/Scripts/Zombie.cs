using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Zombie : MonoBehaviour, IDamageable
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected AudioSource audioSource;

    [SerializeField] protected Transform target;

    [SerializeField] private GameObject[] attackTriggers;

    [Tooltip("Animator")]
    public enum STATE_TYPE {
        Attack,
        Bark,
        Damage,
        Die,
        Idle,
        Movement,
        Walk,
    }
    protected STATE_TYPE currentState;
    protected bool alreadyAttacked = false;
    private float enableAttackOffset = 0.95f;

    [Header("Parameter")]
    [SerializeField] private int health = 50;
    [SerializeField] private int damageReactionOffset = 25;
    [SerializeField] protected int attackPower = 30;
    [SerializeField] private int headShotBuff = 2;

    [SerializeField] private bool activate = false;
    private bool isDie = false;
    
    protected void Start()
    {
        if (!target) {
            target = GameManager.Instance.player;
        }
    }

    protected virtual void Update()
    {
        Movement();

        UpdateCurrentState();

        ControlMovementByAnimationState();

        PreventDuplicateAttack();

        CheckDamageAnimationFinish();
    }

    public virtual void Activate() {
        activate = true;
        HideAttackTriggers();
    }

    protected virtual void Movement() {
        if (activate) {
            Chase();
        }
        else {
            Patrol();
        }
    }

    protected virtual void Chase() {
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

    protected virtual void Patrol() {}

    public void UpdateCurrentState() {
        foreach(STATE_TYPE state in Enum.GetValues(typeof(STATE_TYPE))) {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(state.ToString()) || stateInfo.IsTag(state.ToString())) {
                currentState = state;
                print($"currentState : {currentState}");
            }
        }
    }
    private void ControlMovementByAnimationState() {
        if (currentState == STATE_TYPE.Movement && activate) {
            agent.enabled = true;
        }
        else {
            agent.enabled = false;
        }
    }

    public void Damage(int damage) {
        if (isDie) return;

        health -= damage;
        if (health <= 0) {
            Die();
        }
        else if (health <= damageReactionOffset) {
            animator.SetInteger("DamageType", 0);
            animator.SetBool(STATE_TYPE.Damage.ToString(), true);
            damageReactionOffset = 0; // 再びリアクションしないようにする
        }
    }

    public void HeadShot(int damage) {
        if (isDie) return;

        health -= damage * headShotBuff;

        ScoreManager.OnHeadShot();

        if (health <= 0) {
            Die();
            return;
        }

        if (currentState == STATE_TYPE.Damage) return;

        animator.SetInteger("DamageType", 1);
        animator.SetBool(STATE_TYPE.Damage.ToString(), true);
    }

    private void CheckDamageAnimationFinish() {
        if (currentState != STATE_TYPE.Damage) return;

        animator.SetBool(STATE_TYPE.Damage.ToString(), false);
    }

    //TODO 死ぬアニメーションが再生され続けるバグの修正
    private void Die() {
        if (isDie) return;

        isDie = true;

        ScoreManager.OnKill();

        animator.SetBool(STATE_TYPE.Die.ToString(), true);
        Destroy(this.gameObject, 3.0f);
    }

    public void ModeChangeAttack(bool mode) {
        animator.SetBool(STATE_TYPE.Attack.ToString(), mode);
    }

    public virtual void Attack(Collider other) {
        //Debug.Log($"Attack other : {other}");
        if (currentState == STATE_TYPE.Attack && !alreadyAttacked) {
            //Debug.Log("Attack");
            alreadyAttacked = true;

            if (other.TryGetComponent(out IDamageable target)) {
                target.Damage(attackPower);
            }
        }
    }

    private void PreventDuplicateAttack() {
        if (currentState == STATE_TYPE.Attack && !alreadyAttacked) return;

        var stateTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        // ループしている場合はstateTimeはどんどん増えていく。よって正規化をする
        var normalizedTime = stateTime - Mathf.Floor(stateTime);
        if (enableAttackOffset < normalizedTime) {
            alreadyAttacked = false;
            Debug.Log("refresh attack flag");
        }
    }

    public void ShowAttackTriggers() {
        if (attackTriggers.Length == 0) return;
        foreach(var attackTrigger in attackTriggers) {
            if (!attackTrigger) continue;
            attackTrigger.SetActive(true);
        }
    }

    public void HideAttackTriggers() {
        if (attackTriggers.Length == 0) return;
        foreach (var attackTrigger in attackTriggers) {
            attackTrigger.SetActive(false);
        }
    }

    public bool IsDie { get { return currentState == STATE_TYPE.Die; } }

    [Header("Sound")]
    [SerializeField] protected AudioClip footStepAudioClip;
    [SerializeField] protected float footStepVolume = 1.0f;
    [SerializeField] protected AudioClip damageAudioClip;
    [SerializeField] protected float damageVolume = 0.4f;
    [SerializeField] protected AudioClip dieAudioClip;
    [SerializeField] protected float dieVolume = 0.4f;
    [SerializeField] protected AudioClip barkClip;
    [SerializeField] protected float barkVolume = 0.4f;

    protected string footStepState = "Left";

    protected void FootStep() {
        if (!audioSource || !footStepAudioClip) {
            Debug.LogError("audio source or audio clip are null");
            return;
        }

        audioSource.volume = footStepVolume;
        audioSource.PlayOneShot(footStepAudioClip, volumeScale: footStepVolume);
    }

    public void RightFootStep() {
        if (footStepState == "Right") return;
        Debug.Log("right");
        footStepState = "Right";
        FootStep();
    }

    public void LeftFootStep() {
        if (footStepState == "Left") return;
        Debug.Log("left");
        footStepState = "Left";
        FootStep();
    }

    public void DamageSound() {
        if (damageAudioClip) {
            audioSource.PlayOneShot(damageAudioClip, volumeScale: damageVolume);
        }
    }

    public void DieSound() {
        if (dieAudioClip) {
            audioSource.PlayOneShot(dieAudioClip, volumeScale: dieVolume);
        }
    }

    public void Bark() {
        if (barkClip) {
            audioSource.PlayOneShot(barkClip, volumeScale: barkVolume);
        }
    }

    public string getTargetTag { get { return target.tag; } }
}
