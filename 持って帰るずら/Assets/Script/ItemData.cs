using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    //�A�C�e��ID
    public string id;

    //�A�C�e���̏�����
    private int count;

    public ItemData(string id, int count = 1)
    {
        this.id = id;
        this.count = count;
    }

    //�������J�E���g�A�b�v
    public void CountUp(int value = 1)
    {
        count += value;
    }

    //�������̃J�E���g�_�E��
    public void CountDown(int value = 1)
    {
        count -= value;
    }

    public int GetCount()
    {
        return count;
    }
}

