using UnityEngine;

public class StanBomb : MonoBehaviour
{
    //�p�[�e�B�N��
    public GameObject particleObj;

    //Enemey�I�u�W�F�N�g
    Enemy enemy;

    //�_���[�W
    private float damage = 5.0f;

    private void OnCollisionEnter(Collision collision)
    {

        //�G�̏ꍇ
        if (collision.gameObject.tag == "Enemy")
        {

            //Enemy�̎擾
            enemy = GameObject.Find("Enemy").GetComponent<Enemy>();

            //�_���[�W
            enemy.eHP -= damage;

            //�p�[�e�B�N���p�Q�[���I�u�W�F�N�g����
            Instantiate(particleObj, this.transform.position, Quaternion.identity);

            //�����������{���̍폜
            Destroy(this.gameObject);

            //�X�e�[�g�ύX
            enemy.SetState(Enemy.EnemyState.STUN);

        }

        //�X�e�[�W�̏ꍇ
        if (collision.gameObject.tag == "Stage")
        {
            //�p�[�e�B�N���p�Q�[���I�u�W�F�N�g����
            Instantiate(particleObj, this.transform.position, Quaternion.identity);

            //�����������{���̍폜
            Destroy(this.gameObject);

        }


    }

}
