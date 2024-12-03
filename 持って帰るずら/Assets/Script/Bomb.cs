using UnityEngine;

public class Bomb : MonoBehaviour
{
    //パーティクル
    public GameObject particleObj;

    //Enemeyオブジェクト
    Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {

        //敵の場合
        if (collision.gameObject.tag == "Enemy")
        {
            //Enemyの取得
            enemy = GameObject.Find("Enemy").GetComponent<Enemy>();

            //ダメージ
            //enemy.eHP -= damage;

            //パーティクル用ゲームオブジェクト生成
            Instantiate(particleObj, this.transform.position, Quaternion.identity);

            //あったったボムの削除
            Destroy(this.gameObject);

            //ステート変更
            enemy.SetState(Enemy.EnemyState.DAMAGE);
        }

        //ステージの場合
        if (collision.gameObject.tag == "Stage")
        {
            //パーティクル用ゲームオブジェクト生成
            Instantiate(particleObj, this.transform.position, Quaternion.identity);

            //あったったボムの削除
            Destroy(this.gameObject);

        }

    }

}
