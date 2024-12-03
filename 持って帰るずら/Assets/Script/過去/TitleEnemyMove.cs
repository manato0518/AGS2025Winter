using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleEnemyMove : MonoBehaviour
{
    //移動速度
    private float moveSpeed = 5.0f;


    enum MoveState
    {
        Wait,
        Run
    }
    MoveState moveState = MoveState.Run;

    // ※ 1～9の範囲でランダムな整数値が返る
    float rnd;
    
    // Start is called before the first frame update
    void Start()
    {

        moveState = MoveState.Run;

    }

    // Update is called once per frame
    void Update()
    {
        switch (moveState)
        {
            case MoveState.Wait:

                // 指定の待つ時間がたったかどうかを見る

                // 指定時間がたっていれば状態Runに変える

                // 何秒まつかを決定する


                //秒数に変換
                rnd-=Time.deltaTime;
                if (rnd <= 0.0f)
                {
                    moveState=MoveState.Run;

                    // スピードを戻す
                    moveSpeed = 5.0f;
                }
                else
                {
                    // 移動を止める
                    moveSpeed = 0.0f;

                }

                break;

            case MoveState.Run:
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                break;
        }

    }

    // ゲームオブジェクト同士が接触したタイミングで実行
    void OnTriggerEnter(Collider other)
    {
        Vector3 posi = this.transform.position;

        // もし衝突した相手オブジェクトの名前が"Cube"ならば
        if (other.tag == "Replay")
        {

            posi = new Vector3(-32.473f, 0f, -8.75f);
            this.transform.position = posi;

            // 状態を待ちの状態に変える
            moveState = MoveState.Wait;

            //ランダムな数字の取得
            rnd = UnityEngine.Random.Range(3,7);

        }
    }
}
