using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniEnemy : MonoBehaviour
{

    //�T���͈͂̔��a
    public float seachRadius = 10.0f;

    private Vector3 startPosition;

    //�i�r�Q�[�V����
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        //�i�r�Q�[�V�����̎擾
        agent = GetComponent<NavMeshAgent>();

        //�����ʒu����_��
        startPosition = transform.position;

        //�ړI�n�̐ݒ�
        SetNewDestination();
    }

    // Update is called once per frame
    void Update()
    {
        //�ړI�n�ɓ��B������ҋ@���Ď��̖ړI�n��ݒ�
        if(!agent.pathPending&&agent.remainingDistance<= agent.stoppingDistance)
        {
            SetNewDestination();
        }
    }

    //�V�����ړI�n��ݒ�
    private void SetNewDestination()
    {
        Vector3 randamDirection = Random.insideUnitSphere * seachRadius;

        //��_�ʒu�𒆐S�ɔ͈͓��Ń����_���ȃ|�C���g�𐶐�
        randamDirection += startPosition;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(randamDirection,out hit,seachRadius,1))
        {
            //NavMesh��̗L���n�_��ړI�n�ɐݒ�
            agent.SetDestination(hit.position);
        }

    }
}
