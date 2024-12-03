using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBomb : MonoBehaviour
{

    public string id_;

    private float nowPos;
    // Start is called before the first frame update
    void Start()
    {
        nowPos = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position = new Vector3(transform.position.x, nowPos + Mathf.PingPong(Time.time/3, 0.3f), transform.position.z);
    }
}
