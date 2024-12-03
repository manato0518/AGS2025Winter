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

    //キャラの状態
    public EnemyState state;

    //ボスHP
    public float eHP = 100.0f;

    //走るスピード
    public float runSpeed_ = 13.0f;

    //歩くスピード
    public float walkSpeed_ = 7.0f;

    //ターゲットの情報
    private Transform targetTransform;

    //NavMeshAgentコンポーネント
    private NavMeshAgent navMeshAgent_;

    //Animatorコンポーネント
    public Animator animator;

    //目的地の位置情報を格納するためのパラメータ
    private Vector3 destination;

    //巡回地点のオブジェクト数
    private int destPoint_ = 0;

    //巡回地点オブジェクトを格納する配列
    public Transform[] points;

    //プレイヤー
    public Player player;

    //MiniEnemy
    public GameObject minienemy;

    //攻撃の当たり判定オブジェクト
    public GameObject attackCollisionObj;

    //攻撃のコライダー
    private  Collider attackCollider;

    //スタンの持続時間
    private float stunTime = 3.0f;

    //スタン時間のカウント
    private float stunCun = 0.0f;

    //スタン状態か判別するフラグ
    private bool isStun = false;

    private Vector3 gatherGoalPos_;

    // Start is called before the first frame update
    void Start()
    {

        navMeshAgent_ = GetComponent<NavMeshAgent>();

        //巡回地点間の移動を継続させるために自動ブレーキをfalseに
        navMeshAgent_.autoBraking = false;

        animator = this.gameObject.GetComponent<Animator>();

        //初期状態をRoaming状態に設定する
        SetState(EnemyState.ROAMING);

        //攻撃判定の取得
        attackCollider = attackCollisionObj.GetComponent<Collider>();

        //攻撃判定の非表示
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

        //スタン時間
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

        //死亡判定
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

        //プレイヤーの位置取得
        var playerPos_ = player.transform.position;

        //目的地をターゲットの位置に設定
        navMeshAgent_.SetDestination(playerPos_);

        //キャラを動けるようにする
        navMeshAgent_.isStopped = false;

        //アニメーションコントローラーのフラグ切替
        animator.SetBool("Chase", true);

        //キャラのスピード変更
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

        //敵の向きをプレイヤーの方向に少しづつ変える
        var pPos = player.transform.position;
        pPos.y = 0.0f;
        var mPos = transform.position;
        mPos.y = 0.0f;
        var dir = (pPos - mPos).normalized;
        Quaternion setRotation = Quaternion.LookRotation(dir);

        //算出した方向の角度を敵の角度に設定
        transform.rotation = Quaternion.Slerp(transform.rotation, setRotation, navMeshAgent_.angularSpeed * 0.1f * Time.deltaTime);
        
    }

    private void UpdateRoaming()
    {

        //ランダム値
        var rand = Random.Range(0, 3);

        //アニメーションコントローラーのフラグ切替
        animator.SetBool("Roaming", true);

        if (!(navMeshAgent_.remainingDistance < 0.1))
        {
            return;
        }

        //地点に何も設定されてないときに返す
        if (points.Length == 0)
        {
            return;
        }

        //Enemyが設定された目的地に移動する
        navMeshAgent_.destination = points[destPoint_].position;

        destPoint_ =destPoint_+1;

        //Debug.Log(this.destPoint_);

        //配列内の次の位置を目的地に設定
        destPoint_ = (destPoint_) % points.Length;

        //キャラのスピード変更
        navMeshAgent_.speed = walkSpeed_;
    }

    private void UpdateAttack()
    {
        //キャラの移動を止める
        navMeshAgent_.isStopped = true;

        // 速度をリセットすることで滑りを防ぐ
        navMeshAgent_.velocity = Vector3.zero;

        animator.SetBool("Attack", true);
    }

    private void UpdateCoolTime()
    {
        //キャラの移動を止める
        navMeshAgent_.isStopped = true;

        animator.SetBool("CoolTime", true);
        animator.SetBool("Attack", false);

    }

    private void UpdateDamage()
    {

        //キャラの移動を止める
        navMeshAgent_.isStopped = true;

        // 速度をリセットすることで滑りを防ぐ
        navMeshAgent_.velocity = Vector3.zero;

        animator.SetBool("Damage",true);

        attackCollider.enabled = false;

    }

    private void UpdateStun()
    {
        // 既にスタン中なら何もしない
        if (isStun) return;

        animator.SetBool("Stun", true);

        //キャラの移動を止める
        navMeshAgent_.isStopped = true;

        // 速度をリセットすることで滑りを防ぐ
        navMeshAgent_.velocity = Vector3.zero;

        isStun = true;

        attackCollider.enabled = false;
    }

    private void UpdateDeath() 
    {
        animator.SetBool("Death", true);

        //敵のコライダー削除
        var enemyCollider = this.GetComponent<CapsuleCollider>();
        enemyCollider.enabled = false;

        //キャラの移動を止める
        navMeshAgent_.isStopped = true;

        // 速度をリセットすることで滑りを防ぐ
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

        //キャラのスピード変更
        navMeshAgent_.speed = runSpeed_;

        if (dis < 10.0f)
        {
            SetState(EnemyState.ROAMING);

            minienemyanim.SetBool("Walk", true);
        }
    }

    //　敵キャラクターの状態取得メソッド
    public EnemyState GetState()
    {
        return state;
    }

    //　目的地を設定する
    private void SetDestination(Vector3 position)
    {
        destination = position;
    }

    //　目的地を取得する
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

