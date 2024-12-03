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

    //�ő�HP
    float maxHp = 100.0f;

    //HP�̏��
    public HpState hState;

    //Enemy�̍U����
    private float enemyDamge = 50.0f;

    // �ړ����x
    public float speed_;

    //���X�|�[���n�_
    public Vector3 RespawnPoints;

    //�_���[�W���
    [SerializeField] Image DamageImg;

    //��ʂ̌�
    public Image bloodImg;

    //�|�X�g�G�t�F�N�g
    public PostProcessVolume postProcessVolume;

    //Vignette
    private Vignette vignette;

    //ChromaticAberration
    private ChromaticAberration chromaticAberration;

    //DepthOfField
    private DepthOfField depthOfField;

    //�{���I�u�W�F�N�g�̎擾
    public GameObject throwBomb;

    //�X�^���{���I�u�W�F�N�g�̎擾
    public GameObject throwStunBomb;

    //�J�����̎擾
    public Camera camera_;

    //�v���C���[�̏����A�C�e��
    [SerializeField] private ItemManager itemManager_;

    //�I�𒆂̃A�C�e��
    public ItemScroll itemScroll;

    //�t�F�[�h�C��
    public Fade fade;

    //�񕜂���܂ł̎���
    float deltaHealTime = 0.0f;

    void Start()
    {
        //�}�E�X�J�[�\���̔�\��
        Cursor.visible = false;

        //�_���[�W��ʂ̐F�̎擾
        DamageImg.color = Color.clear;

        //�{���̃v���n�u��GameObject�Ŏ擾
        throwBomb = (GameObject)Resources.Load("Weapon/Bomb");

        //�X�^���{���̃v���n�u��GameObject�Ŏ擾
        throwStunBomb = (GameObject)Resources.Load("Weapon/StunBomb");

        //Vignette�̒l�̎擾
        postProcessVolume.profile.TryGetSettings(out vignette);

        //ChromaticAberration�̒l�̎擾
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);

        //DepthOfField�̒l�̎擾
        postProcessVolume.profile.TryGetSettings(out depthOfField);

        //�R���[�`��
        StartCoroutine(PostEffectBlood());
        StartCoroutine(PostEffectAberration());
        StartCoroutine(PostEffectDepth());

    }

    // Update is called once per frame
    void Update()
    {
        //�ړ�����
        playerMove();

        //�ǂʂ������ۂ̃��X�|�[������
        Respawn();

        //�{���̐�������
        Throw();

        DamageImg.color = Color.Lerp(DamageImg.color,Color.clear,Time.deltaTime);

        //�񕜏���
        Heal();

    }

    void playerMove()
    {
        //�ړ�����
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


        //�}�E�X�J�[�\���̕\��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }

    }

    private void Respawn()
    {
        //���X�|�[������
        if(this.transform.position.y <= -7 ) 
        {
            this.transform.position = RespawnPoints;
        }
    }

    //�U���̓����蔻��
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyAttackCollision"))
        {
            //Debug.Log("����������");

            Damage();
        }
        
    }

    //�_���[�W����
    private void Damage()
    {
        //HP����
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
        //�񕜂��鐔�l
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

        //Rigidbody�̎擾
        Rigidbody bombRb = null;
        if (Input.GetKeyDown(KeyCode.R))
        {

            var serectItem = itemScroll.selectItem_.GetComponentInChildren<HaveItemCountText>();

            //Item�̏����m�F
            if (itemManager_.GetItemHave(serectItem.id_) > 0)
            {
                //�{���̐���
                var go = Instantiate(throwBomb, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 5), Quaternion.identity);
                go.transform.position = this.transform.position + new Vector3(0.0f, 0.2f, 1.0f);
                bombRb = go.GetComponent<Rigidbody>();

                //�J�����̉�]���
                Quaternion rot = camera_.transform.rotation;
                rot = rot * Quaternion.AngleAxis(-30.0f, Vector3.right);
                //�{���̕��o
                bombRb.AddForce(rot * Vector3.forward * 60.0f, ForceMode.Impulse);

                //Item�̌�������
                itemManager_.CountDownItem(serectItem.id_, 1);

            }

        }

        Rigidbody SbombRb = null;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //�X�^���{���̐���
            var go = Instantiate(throwStunBomb, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 5), Quaternion.identity);
            go.transform.position = this.transform.position + new Vector3(0.0f, 0.2f, 1.0f);
            SbombRb = go.GetComponent<Rigidbody>();

            //�{���̕��o
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

        //intensity�Ŏg�p
        //float iIncrement = 0.1f;
        //float iMin = 0.0f;
        //float iMax = 0.3f;

        //blood�Ŏg�p
        float imgIncrement = 0.1f;
        float imgMin = 0.0f;
        float imgMax = 0.4f;

        //chromaticAberration�Ŏg�p
        float vIncrement = 0.1f;
        float vMin = 0.0f;
        float vMax = 1.0f;

        //depthOfField�Ŏg�p
        float dIncrement = 100.0f;
        float dMin = 0.0f;
        float dMax = 300.0f;

        while (true)
        {

            Color bloodColor = bloodImg.color;

            //���`�⊮
            float a = Mathf.Lerp(imgMin, imgMax, 1.0f - HP / maxHp);

            bloodColor.a = Mathf.Clamp(a, imgMin, imgMax);

            bloodImg.color = bloodColor;

            yield return new WaitForSeconds(0.1f);

        }

    }


    IEnumerator PostEffectAberration()
    {

        //chromaticAberration�Ŏg�p
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

        //chromaticAberration�Ŏg�p
        float vIncrement = 0.1f;
        float vMin = 0.0f;
        float vMax = 1.0f;


        while (true)
        {
            //�����l�̍쐬(-1.0�`1.0)
            float sin = Mathf.Sin(Time.time);
            //0.0�`2.0
            sin+=1.0f;
            //0.0�`0.5
            sin /=4.0f;
            //0.5�`1.0
            sin += 0.5f;

            //float b = Mathf.Lerp(vMin, vMax, 1.0f - tmpHp / tmpMaxHp);

            //chromaticAberration.intensity.value = Mathf.Clamp(chromaticAberration.intensity.value + vIncrement, vMin, vMax);
            chromaticAberration.intensity.value = Mathf.Clamp(sin, vMin, vMax);

            yield return new WaitForSeconds(0.1f);

        }

    }


    IEnumerator PostEffectDepth()
    {

        //depthOfField�Ŏg�p
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

    //        //intensity�Ŏg�p
    //        //float iIncrement = 0.1f;
    //        //float iMin = 0.0f;
    //        //float iMax = 0.3f;

    //        //blood�Ŏg�p
    //        float imgIncrement = 0.1f;
    //        float imgMin = 0.0f;
    //        float imgMax = 0.4f;

    //        //chromaticAberration�Ŏg�p
    //        float vIncrement = 0.1f;
    //        float vMin = 0.0f;
    //        float vMax = 1.0f;

    //        //depthOfField�Ŏg�p
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
    //                    //���`�⊮
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
