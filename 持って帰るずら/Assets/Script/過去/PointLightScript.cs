using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightScript : MonoBehaviour
{
    //�_�ł���������擾�i�C���X�y�N�^�[���ŃA�^�b�`���Ă��������j
    public GameObject startLight;
    //���̋����擾���邽�߂̕ϐ������
    Light lightIntence;

    public float Lightsflashing = 0;

    void Start()
    {
        //���̋���(Intensity)���擾�B
        lightIntence = startLight.GetComponent<Light>();
    }
    void Update()
    {
        //��q����t���b�V���֐����R���[�`���ŊJ�n�B
        //���̋�����0��1��0.7�b�Ԋu�ŕ\��������B
        StartCoroutine(Frash(0f, 0.65f));
    }


    //���̋���������ł���֐��i�����Œ����ł���j�B
    private void LightIntencity(float intenceAmount)
    {
        lightIntence.intensity = intenceAmount;
    }


    //���̓_�Ŋ֐��B
    IEnumerator Frash(float intenceAmount, float intenceAmount2)
    {
        LightIntencity(intenceAmount);
        yield return new WaitForSeconds(Lightsflashing);
        LightIntencity(intenceAmount2);
    }
}