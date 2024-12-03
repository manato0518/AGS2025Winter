using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public class Enemy : MonoBehaviour
{

    public enum EnemyState
    {
        ROAMING,
        CHASE,
        ATTACK,
        COOLTIME,
        DAMAGE,
        STUN,
        GATHER,
        DEATH
    };

    //�L�����̏��
    public EnemyState state;

    //�{�XHP
    public float eHP = 100.0f;

    //����X�s�[�h
    public float runSpeed_ = 13.0f;

    //�����X�s�[�h
    public float walkSpeed_ = 7.0f;

    //�^�[�Q�b�g�̏��
    private Transform targetTransform;

    //NavMeshAgent�R���|�[�l���g
    private NavMeshAgent navMeshAgent_;

    //Animator�R���|�[�l���g
    public Animator animator;

    //�ړI�n�̈ʒu�����i�[���邽�߂̃p�����[�^
    private Vector3 destination;

    //����n�_�̃I�u�W�F�N�g��
    private int destPoint_ = 0;

    //����n�_�I�u�W�F�N�g���i�[����z��
    public Transform[] points;

    //�v���C���[
    public Player player;

    //MiniEnemy
    public GameObject minienemy;

    //�U���̓����蔻��I�u�W�F�N�g
    public GameObject attackCollisionObj;

    //�U���̃R���C�_�[
    private  Collider attackCollider;

    //�X�^���̎�������
    private float stunTime = 3.0f;

    //�X�^�����Ԃ̃J�E���g
    private float stunCun = 0.0f;

    //�X�^����Ԃ����ʂ���t���O
    private bool isStun = false;

    private Vector3 gatherGoalPos_;

    // Start is called before the first frame update
    void Start()
    {

        navMeshAgent_ = GetComponent<NavMeshAgent>();

        //����n�_�Ԃ̈ړ����p�������邽�߂Ɏ����u���[�L��false��
        navMeshAgent_.autoBraking = false;

        animator = this.gameObject.GetComponent<Animator>();

        //������Ԃ�Roaming��Ԃɐݒ肷��
        SetState(EnemyState.ROAMING);

        //�U������̎擾
        attackCollider = attackCollisionObj.GetComponent<Collider>();

        //�U������̔�\��
        attackCollider.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(this.state);

        switch (state)
        {
            case EnemyState.ROAMING:
                UpdateRoaming();
                break;
            case EnemyState.CHASE:
                UpdateChase();
                break;
            case EnemyState.ATTACK:
                UpdateAttack();
                break;
            case EnemyState.COOLTIME:
                UpdateCoolTime();
                break;
            case EnemyState.DAMAGE:
                UpdateDamage();
                break;
            case EnemyState.STUN:
                UpdateStun();
                break;
            case EnemyState.GATHER:
                UpdateGather();
                break;
            case EnemyState.DEATH:
                UpdateDeath();
                break;
            default:
                break;
        }

        //�X�^������
        if(isStun)
        {
            stunCun += Time.deltaTime;
            //Debug.Log(stunCun);
            if (stunCun > stunTime)
            {
                isStun = false;
                stunCun = 0.0f;
                animator.SetBool("Stun", false);
                SetState(EnemyState.CHASE);
            }
        }

        //���S����
        //Debug.Log(eHP);
        if(eHP <= 0)
        {
            SetState(EnemyState.DEATH);
        }

    }

    public void SetState(EnemyState tempState, Transform targetObject = null)
    {
        Debug.Log(tempState);

        switch (tempState)
        {
            case EnemyState.CHASE:
                break;
            case EnemyState.ROAMING:
                break;
            case EnemyState.ATTACK:
                break;
            case EnemyState.COOLTIME:
                break;
            case EnemyState.DAMAGE:
                break;
            case EnemyState.STUN:
                break;
            case EnemyState.GATHER:
                break;
            case EnemyState.DEATH:
                break;
            default:
                break;
        }
        state = tempState;
    }

    private void UpdateChase()
    {

        navMeshAgent_.isStopped = false;

        //�v���C���[�̈ʒu�擾
        var playerPos_ = player.transform.position;

        //�ړI�n���^�[�Q�b�g�̈ʒu�ɐݒ�
        navMeshAgent_.SetDestination(playerPos_);

        //�L�����𓮂���悤�ɂ���
        navMeshAgent_.isStopped = false;

        //�A�j���[�V�����R���g���[���[�̃t���O�ؑ�
        animator.SetBool("Chase", true);

        //�L�����̃X�s�[�h�ύX
        navMeshAgent_.speed = runSpeed_;


        if (targetTransform == null)
        {
            //SetState(EnemyState.ROAMING);
        }
        else
        {
            SetDestination(targetTransform.position);
            navMeshAgent_.SetDestination(GetDestination());
        }

        //�G�̌������v���C���[�̕����ɏ����Âς���
        var pPos = player.transform.position;
        pPos.y = 0.0f;
        var mPos = transform.position;
        mPos.y = 0.0f;
        var dir = (pPos - mPos).normalized;
        Quaternion setRotation = Quaternion.LookRotation(dir);

        //�Z�o���������̊p�x��G�̊p�x�ɐݒ�
        transform.rotation = Quaternion.Slerp(transform.rotation, setRotation, navMeshAgent_.angularSpeed * 0.1f * Time.deltaTime);
        
    }

    private void UpdateRoaming()
    {

        //�����_���l
        var rand = Random.Range(0, 3);

        //�A�j���[�V�����R���g���[���[�̃t���O�ؑ�
        animator.SetBool("Roaming", true);

        if (!(navMeshAgent_.remainingDistance < 0.1))
        {
            return;
        }

        //�n�_�ɉ����ݒ肳��ĂȂ��Ƃ��ɕԂ�
        if (points.Length == 0)
        {
            return;
        }

        //Enemy���ݒ肳�ꂽ�ړI�n�Ɉړ�����
        navMeshAgent_.destination = points[destPoint_].position;

        destPoint_ =destPoint_+1;

        //Debug.Log(this.destPoint_);

        //�z����̎��̈ʒu��ړI�n�ɐݒ�
        destPoint_ = (destPoint_) % points.Length;

        //�L�����̃X�s�[�h�ύX
        navMeshAgent_.speed = walkSpeed_;
    }

    private void UpdateAttack()
    {
        //�L�����̈ړ����~�߂�
        navMeshAgent_.isStopped = true;

        // ���x�����Z�b�g���邱�ƂŊ����h��
        navMeshAgent_.velocity = Vector3.zero;

        animator.SetBool("Attack", true);
    }

    private void UpdateCoolTime()
    {
        //�L�����̈ړ����~�߂�
        navMeshAgent_.isStopped = true;

        animator.SetBool("CoolTime", true);
        animator.SetBool("Attack", false);

    }

    private void UpdateDamage()
    {

        //�L�����̈ړ����~�߂�
        navMeshAgent_.isStopped = true;

        // ���x�����Z�b�g���邱�ƂŊ����h��
        navMeshAgent_.velocity = Vector3.zero;

        animator.SetBool("Damage",true);

        attackCollider.enabled = false;

    }

    private void UpdateStun()
    {
        // ���ɃX�^�����Ȃ牽�����Ȃ�
        if (isStun) return;

        animator.SetBool("Stun", true);

        //�L�����̈ړ����~�߂�
        navMeshAgent_.isStopped = true;

        // ���x�����Z�b�g���邱�ƂŊ����h��
        navMeshAgent_.velocity = Vector3.zero;

        isStun = true;

        attackCollider.enabled = false;
    }

    private void UpdateDeath() 
    {
        animator.SetBool("Death", true);

        //�G�̃R���C�_�[�폜
        var enemyCollider = this.GetComponent<CapsuleCollider>();
        enemyCollider.enabled = false;

        //�L�����̈ړ����~�߂�
        navMeshAgent_.isStopped = true;

        // ���x�����Z�b�g���邱�ƂŊ����h��
        navMeshAgent_.velocity = Vector3.zero;
    }

    public void ChangeGather(Vector3 pos)
    {
        gatherGoalPos_ = pos;

        if (this.state == EnemyState.ROAMING)
        {
            this.navMeshAgent_.SetDestination(gatherGoalPos_);

            SetState(Enemy.EnemyState.GATHER);
        }
    }
     
    private void UpdateGather()
    {
        animator.SetBool("Chase", true);

        float dis = Vector3.Distance(gatherGoalPos_, transform.position);

        var minienemyanim = minienemy.GetComponent<Animator>();

        //�L�����̃X�s�[�h�ύX
        navMeshAgent_.speed = runSpeed_;

        if (dis < 10.0f)
        {
            SetState(EnemyState.ROAMING);

            minienemyanim.SetBool("Walk", true);
        }
    }

    //�@�G�L�����N�^�[�̏�Ԏ擾���\�b�h
    public EnemyState GetState()
    {
        return state;
    }

    //�@�ړI�n��ݒ肷��
    private void SetDestination(Vector3 position)
    {
        destination = position;
    }

    //�@�ړI�n���擾����
    private Vector3 GetDestination()
    {
        return destination;
    }

    public void CoolTimeEnd()
    {
        animator.SetBool("CoolTime", false);
        SetState(EnemyState.CHASE);
    }

    public void AttackStart()
    {
        attackCollider.enabled = true;
    }

    public void AttackEnd()
    {
        attackCollider.enabled = false;
        animator.SetBool("Attack", false);
    }

    public void DamgeEnd()
    {
        SetState(EnemyState.CHASE);
        animator.SetBool("Damage", false);
    }

}

