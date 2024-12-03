using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleEnemyMove : MonoBehaviour
{
    //�ړ����x
    private float moveSpeed = 5.0f;


    enum MoveState
    {
        Wait,
        Run
    }
    MoveState moveState = MoveState.Run;

    // �� 1�`9�͈̔͂Ń����_���Ȑ����l���Ԃ�
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

                // �w��̑҂��Ԃ����������ǂ���������

                // �w�莞�Ԃ������Ă���Ώ��Run�ɕς���

                // ���b�܂������肷��


                //�b���ɕϊ�
                rnd-=Time.deltaTime;
                if (rnd <= 0.0f)
                {
                    moveState=MoveState.Run;

                    // �X�s�[�h��߂�
                    moveSpeed = 5.0f;
                }
                else
                {
                    // �ړ����~�߂�
                    moveSpeed = 0.0f;

                }

                break;

            case MoveState.Run:
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                break;
        }

    }

    // �Q�[���I�u�W�F�N�g���m���ڐG�����^�C�~���O�Ŏ��s
    void OnTriggerEnter(Collider other)
    {
        Vector3 posi = this.transform.position;

        // �����Փ˂�������I�u�W�F�N�g�̖��O��"Cube"�Ȃ��
        if (other.tag == "Replay")
        {

            posi = new Vector3(-32.473f, 0f, -8.75f);
            this.transform.position = posi;

            // ��Ԃ�҂��̏�Ԃɕς���
            moveState = MoveState.Wait;

            //�����_���Ȑ����̎擾
            rnd = UnityEngine.Random.Range(3,7);

        }
    }
}
