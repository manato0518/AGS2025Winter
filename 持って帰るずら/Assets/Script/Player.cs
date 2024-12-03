using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour
{

    //HP
    public float HP = 100.0f;

    //最大HP
    float maxHp = 100.0f;

    //HPの状態
    public HpState hState;

    //Enemyの攻撃力
    private float enemyDamge = 50.0f;

    // 移動速度
    public float speed_;

    //リスポーン地点
    public Vector3 RespawnPoints;

    //ダメージ画面
    [SerializeField] Image DamageImg;

    //画面の血
    public Image bloodImg;

    //ポストエフェクト
    public PostProcessVolume postProcessVolume;

    //Vignette
    private Vignette vignette;

    //ChromaticAberration
    private ChromaticAberration chromaticAberration;

    //DepthOfField
    private DepthOfField depthOfField;

    //ボムオブジェクトの取得
    public GameObject throwBomb;

    //スタンボムオブジェクトの取得
    public GameObject throwStunBomb;

    //カメラの取得
    public Camera camera_;

    //プレイヤーの所持アイテム
    [SerializeField] private ItemManager itemManager_;

    //選択中のアイテム
    public ItemScroll itemScroll;

    //フェードイン
    public Fade fade;

    //回復するまでの時間
    float deltaHealTime = 0.0f;

    void Start()
    {
        //マウスカーソルの非表示
        Cursor.visible = false;

        //ダメージ画面の色の取得
        DamageImg.color = Color.clear;

        //ボムのプレハブをGameObjectで取得
        throwBomb = (GameObject)Resources.Load("Weapon/Bomb");

        //スタンボムのプレハブをGameObjectで取得
        throwStunBomb = (GameObject)Resources.Load("Weapon/StunBomb");

        //Vignetteの値の取得
        postProcessVolume.profile.TryGetSettings(out vignette);

        //ChromaticAberrationの値の取得
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);

        //DepthOfFieldの値の取得
        postProcessVolume.profile.TryGetSettings(out depthOfField);

        //コルーチン
        StartCoroutine(PostEffectBlood());
        StartCoroutine(PostEffectAberration());
        StartCoroutine(PostEffectDepth());

    }

    // Update is called once per frame
    void Update()
    {
        //移動処理
        playerMove();

        //壁ぬけした際のリスポーン処理
        Respawn();

        //ボムの生成処理
        Throw();

        DamageImg.color = Color.Lerp(DamageImg.color,Color.clear,Time.deltaTime);

        //回復処理
        Heal();

    }

    void playerMove()
    {
        //移動処理
        var velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            velocity.z = speed_;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity.x = -speed_;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity.z = -speed_;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x = speed_;
        }
        if (velocity.x != 0 || velocity.z != 0)
        {
            transform.position += transform.rotation * velocity;
        }


        //マウスカーソルの表示
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }

    }

    private void Respawn()
    {
        //リスポーン処理
        if(this.transform.position.y <= -7 ) 
        {
            this.transform.position = RespawnPoints;
        }
    }

    //攻撃の当たり判定
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyAttackCollision"))
        {
            //Debug.Log("あったった");

            Damage();
        }
        
    }

    //ダメージ処理
    private void Damage()
    {
        //HP減少
        HP-=enemyDamge;

        deltaHealTime = 10.0f;

        //Debug.Log(HP);
        DamageImg.color = new Color(0.7f, 0, 0, 0.7f);
        if (HP <= 60.0f)
        {
            //StartCoroutine("sixtyAdditionProcess");
        }
        else if(HP <= 30.0f)
        {

        }
        if (HP <= 0)
        {
            //fade.Instance.ChangeScene("GameOverScene");
        }
    }

    private void Heal()
    {
        //回復する数値
        //float healvalue = 0.05f;
        float healvalue = 0.2f;


        deltaHealTime -= Time.deltaTime;

        if (deltaHealTime <= 0.0f)
        {
            HP += healvalue;
            if(HP>= 100.0f)
            {
                HP = 100;
            }
        }
    }


    private void Throw()
    {

        //Rigidbodyの取得
        Rigidbody bombRb = null;
        if (Input.GetKeyDown(KeyCode.R))
        {

            var serectItem = itemScroll.selectItem_.GetComponentInChildren<HaveItemCountText>();

            //Itemの所持確認
            if (itemManager_.GetItemHave(serectItem.id_) > 0)
            {
                //ボムの生成
                var go = Instantiate(throwBomb, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 5), Quaternion.identity);
                go.transform.position = this.transform.position + new Vector3(0.0f, 0.2f, 1.0f);
                bombRb = go.GetComponent<Rigidbody>();

                //カメラの回転情報
                Quaternion rot = camera_.transform.rotation;
                rot = rot * Quaternion.AngleAxis(-30.0f, Vector3.right);
                //ボムの放出
                bombRb.AddForce(rot * Vector3.forward * 60.0f, ForceMode.Impulse);

                //Itemの減少処理
                itemManager_.CountDownItem(serectItem.id_, 1);

            }

        }

        Rigidbody SbombRb = null;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //スタンボムの生成
            var go = Instantiate(throwStunBomb, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 5), Quaternion.identity);
            go.transform.position = this.transform.position + new Vector3(0.0f, 0.2f, 1.0f);
            SbombRb = go.GetComponent<Rigidbody>();

            //ボムの放出
            Quaternion rot = camera_.transform.rotation;
            rot = rot * Quaternion.AngleAxis(-30.0f, Vector3.right);
            SbombRb.AddForce(rot * Vector3.forward * 60.0f, ForceMode.Impulse);

        }
    }




    public enum HpState
    {
        NOEML,
        DANGER
    }

    public void SetState(HpState tempState, Transform targetObject = null)
    {
        switch (tempState)
        {
            case HpState.NOEML:
                break;
            case HpState.DANGER:
                break;
            default:
                break;
        }

        if(SetState)
    }

    IEnumerator PostEffectBlood()
    {

        //intensityで使用
        //float iIncrement = 0.1f;
        //float iMin = 0.0f;
        //float iMax = 0.3f;

        //bloodで使用
        float imgIncrement = 0.1f;
        float imgMin = 0.0f;
        float imgMax = 0.4f;

        //chromaticAberrationで使用
        float vIncrement = 0.1f;
        float vMin = 0.0f;
        float vMax = 1.0f;

        //depthOfFieldで使用
        float dIncrement = 100.0f;
        float dMin = 0.0f;
        float dMax = 300.0f;

        while (true)
        {

            Color bloodColor = bloodImg.color;

            //線形補完
            float a = Mathf.Lerp(imgMin, imgMax, 1.0f - HP / maxHp);

            bloodColor.a = Mathf.Clamp(a, imgMin, imgMax);

            bloodImg.color = bloodColor;

            yield return new WaitForSeconds(0.1f);

        }

    }


    IEnumerator PostEffectAberration()
    {

        //chromaticAberrationで使用
        float vIncrement = 0.1f;
        float vMin = 0.0f;
        float vMax = 1.0f;
        while (true)
        {

            var tmpHp = HP;
            var tmpMaxHp = maxHp * 0.8f;
            if(tmpHp <= 30)
            {
                tmpHp = 0;
                StartCoroutine(DangerPostEffectAberration());
                SetState(HpState.DANGER);
            }
            else if(tmpHp >= 60)
            {
                tmpHp = tmpMaxHp;
            }
            float b = Mathf.Lerp(vMin, vMax, 1.0f - tmpHp / tmpMaxHp);

            //chromaticAberration.intensity.value = Mathf.Clamp(chromaticAberration.intensity.value + vIncrement, vMin, vMax);
            chromaticAberration.intensity.value = Mathf.Clamp(b, vMin, vMax);

            yield return new WaitForSeconds(0.1f);

        }

    }

    IEnumerator DangerPostEffectAberration()
    {

        //chromaticAberrationで使用
        float vIncrement = 0.1f;
        float vMin = 0.0f;
        float vMax = 1.0f;


        while (true)
        {
            //増減値の作成(-1.0～1.0)
            float sin = Mathf.Sin(Time.time);
            //0.0～2.0
            sin+=1.0f;
            //0.0～0.5
            sin /=4.0f;
            //0.5～1.0
            sin += 0.5f;

            //float b = Mathf.Lerp(vMin, vMax, 1.0f - tmpHp / tmpMaxHp);

            //chromaticAberration.intensity.value = Mathf.Clamp(chromaticAberration.intensity.value + vIncrement, vMin, vMax);
            chromaticAberration.intensity.value = Mathf.Clamp(sin, vMin, vMax);

            yield return new WaitForSeconds(0.1f);

        }

    }


    IEnumerator PostEffectDepth()
    {

        //depthOfFieldで使用
        float dIncrement = 100.0f;
        float dMin = 0.0f;
        float dMax = 300.0f;
        while (true)
        {

            var tmpHp = HP;
            var tmpMaxHp = maxHp * 0.8f;
            if (tmpHp <= 30)
            {
                tmpHp = 0;
            }
            else if (tmpHp >= 60)
            {
                tmpHp = tmpMaxHp;
            }
            float c = Mathf.Lerp(dMin, dMax, 1.0f - tmpHp / tmpMaxHp);

            //chromaticAberration.intensity.value = Mathf.Clamp(chromaticAberration.intensity.value + vIncrement, vMin, vMax);
            depthOfField.focalLength.value = Mathf.Clamp(c, dMin, dMax);

            yield return new WaitForSeconds(0.1f);

        }

    }

    //    IEnumerator sixtyAdditionProcess()
    //    {

    //        PHASE phase = PHASE.ONE;

    //        //intensityで使用
    //        //float iIncrement = 0.1f;
    //        //float iMin = 0.0f;
    //        //float iMax = 0.3f;

    //        //bloodで使用
    //        float imgIncrement = 0.1f;
    //        float imgMin = 0.0f;
    //        float imgMax = 0.4f;

    //        //chromaticAberrationで使用
    //        float vIncrement = 0.1f;
    //        float vMin = 0.0f;
    //        float vMax = 1.0f;

    //        //depthOfFieldで使用
    //        float dIncrement = 100.0f;
    //        float dMin = 0.0f;
    //        float dMax = 300.0f;



    //        while (true)
    //        {

    //            switch(phase)
    //            {
    //                case PHASE.ONE:

    //                    //vignette.intensity.value = Mathf.Clamp(vignette.intensity.value + iIncrement, iMin, iMax);
    //                    //yield return new WaitForSeconds(0.1f);

    //                    //if (vignette.intensity.value >= iMax)
    //                    //{
    //                    //    phase = PHASE.TWO;
    //                    //}
    //                    //break;



    //                    Color bloodColor = bloodImg.color;
    //                    //線形補完
    //                    float a = Mathf.Lerp(imgMin, imgMax, 1.0f-HP/maxHp);

    //                    //bloodColor.a = Mathf.Clamp(bloodColor.a + imgIncrement, imgMin, imgMax);
    //                    bloodColor.a = Mathf.Clamp(a, imgMin, imgMax);

    //                    bloodImg.color = bloodColor;

    //                    yield return new WaitForSeconds(0.1f);

    //                    if(bloodColor.a >= imgMax)
    //                    {
    //                        phase = PHASE.TWO;
    //                    }
    //                    break;



    //                case PHASE.TWO:

    //                    float b = Mathf.Lerp(vMin, vMax, 1.0f - HP / maxHp);

    //                    //chromaticAberration.intensity.value = Mathf.Clamp(chromaticAberration.intensity.value + vIncrement, vMin, vMax);
    //                    chromaticAberration.intensity.value = Mathf.Clamp(b, vMin, vMax);

    //                    yield return new WaitForSeconds(0.1f);

    //                    if(chromaticAberration.intensity.value >= vMax)
    //                    {
    //                        phase = PHASE.THREE;
    //                    }
    //                    break;

    //                case PHASE.THREE:

    //                    float c = Mathf.Lerp(dMin, dMax, 1.0f - HP / maxHp);

    //                    depthOfField.focalLength.value = Mathf.Clamp(c,dMin,dMax);
    //                    yield return new WaitForSeconds(0.1f);

    //                    if(depthOfField.focalLength.value >= dMax)
    //                    {
    //                        break;
    //                    }
    //                    break;
    //            }

    //        }

    //        yield break;

    //    }

    //}

}
