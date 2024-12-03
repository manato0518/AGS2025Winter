using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniEnemySensor : MonoBehaviour
{
    public GameObject enemy;

    public GameObject minienemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider target)
    {
        var enemy = this.enemy.GetComponent<Enemy>();
        var anim = minienemy.GetComponent<Animator>();
        var mEnav = minienemy.GetComponent<NavMeshAgent>();
        var mEPos = minienemy.transform.position;

        if(target.gameObject.tag == "Player")
        {

            //enemy.SetState(Enemy.EnemyState.GATHER);
            enemy.ChangeGather(mEPos);

            //キャラの移動を止める
            mEnav.isStopped = true;

            // 速度をリセットすることで滑りを防ぐ
            mEnav.velocity = Vector3.zero;

            anim.SetBool("Cool",true);
        }

    }

}
