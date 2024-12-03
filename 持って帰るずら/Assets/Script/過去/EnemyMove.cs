using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�ǂ�������Ώ�")]
    private GameObject player;

    private NavMeshAgent navMeshAgent;


    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        // NavMeshAgent��ێ����Ă���
        navMeshAgent = GetComponent<NavMeshAgent>();
        //anim =  GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ////�A�j���[�V����
        //anim.SetBool("Run", true);

        //�Ȃ���p�ł܂�Ȃ��悤�ɂ���
        navMeshAgent.velocity = (navMeshAgent.steeringTarget - transform.position).normalized * navMeshAgent.speed;
        transform.forward = navMeshAgent.steeringTarget - transform.position;

        // �v���C���[��ڎw���Đi��
        navMeshAgent.destination = player.transform.position;
    }

    private void LateUpdate()
    {

        //�^�|�Q�b�g�̂ق�������
        transform.LookAt(player.transform);

    }

}