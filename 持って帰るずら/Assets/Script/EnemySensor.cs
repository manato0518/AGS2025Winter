using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemySensor : MonoBehaviour
{

    //感知範囲
    [SerializeField]
    private SphereCollider searchArea = default;

    //感知範囲の角度
    [SerializeField]
    private float searchAngle = 45f;

    private Enemy enemy = default;


    private void Start()
    {
        enemy = transform.parent.GetComponent<Enemy>();
    }

    private void OnTriggerStay(Collider target)
    {

        if (target.tag != "Player")
        {
            return;
        }

        // Enemy位置
        var ePos = enemy.transform.position;
        ePos.y = 0.0f;

        // Enemy前方
        var eForward = enemy.transform.forward;

        // プレイヤー位置
        var pPos = target.transform.position;
        pPos.y = 0.0f;

        var playerDirection = pPos - ePos;
        var angle = Vector3.Angle(eForward, playerDirection);
        var distnce = Vector3.Distance(pPos, ePos);
        var state = enemy.GetState();
        var radius = searchArea.radius * enemy.transform.localScale.x;
        var isVeryNear = distnce < radius * 0.1f;
        var isNear = distnce < radius;
        var isSee = angle <= searchAngle;

        switch (state)
        {
            case Enemy.EnemyState.ROAMING:
                if(isNear && isSee)
                {
                    enemy.SetState(Enemy.EnemyState.CHASE, target.transform);
                }
                break;
            case Enemy.EnemyState.CHASE:
                if (isVeryNear)
                {
                    enemy.SetState(Enemy.EnemyState.ATTACK);
                }
                break;
            case Enemy.EnemyState.ATTACK:
                if (!isVeryNear)
                {
                    enemy.SetState(Enemy.EnemyState.CHASE, target.transform);
                }
                break;
            default:
                break;
        }

    }

#if UNITY_EDITOR
    //探索角度の表示
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0.0f, -searchAngle, 0.0f)*transform.forward, searchAngle*2.0f, searchArea.radius);
    }
#endif

}
