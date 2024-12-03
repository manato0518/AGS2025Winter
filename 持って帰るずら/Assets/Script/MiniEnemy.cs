using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniEnemy : MonoBehaviour
{

    //探索範囲の半径
    public float seachRadius = 10.0f;

    private Vector3 startPosition;

    //ナビゲーション
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        //ナビゲーションの取得
        agent = GetComponent<NavMeshAgent>();

        //初期位置を基点に
        startPosition = transform.position;

        //目的地の設定
        SetNewDestination();
    }

    // Update is called once per frame
    void Update()
    {
        //目的地に到達したら待機して次の目的地を設定
        if(!agent.pathPending&&agent.remainingDistance<= agent.stoppingDistance)
        {
            SetNewDestination();
        }
    }

    //新しい目的地を設定
    private void SetNewDestination()
    {
        Vector3 randamDirection = Random.insideUnitSphere * seachRadius;

        //基点位置を中心に範囲内でランダムなポイントを生成
        randamDirection += startPosition;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(randamDirection,out hit,seachRadius,1))
        {
            //NavMesh上の有効地点を目的地に設定
            agent.SetDestination(hit.position);
        }

    }
}
