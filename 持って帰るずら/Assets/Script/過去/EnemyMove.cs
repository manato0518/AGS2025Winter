using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    [Tooltip("追いかける対象")]
    private GameObject player;

    private NavMeshAgent navMeshAgent;


    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        // NavMeshAgentを保持しておく
        navMeshAgent = GetComponent<NavMeshAgent>();
        //anim =  GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ////アニメーション
        //anim.SetBool("Run", true);

        //曲がり角でつまらないようにする
        navMeshAgent.velocity = (navMeshAgent.steeringTarget - transform.position).normalized * navMeshAgent.speed;
        transform.forward = navMeshAgent.steeringTarget - transform.position;

        // プレイヤーを目指して進む
        navMeshAgent.destination = player.transform.position;
    }

    private void LateUpdate()
    {

        //タ－ゲットのほうを向く
        transform.LookAt(player.transform);

    }

}