using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    //アイテムID
    public string id;

    //アイテムの所持数
    private int count;

    public ItemData(string id, int count = 1)
    {
        this.id = id;
        this.count = count;
    }

    //所持数カウントアップ
    public void CountUp(int value = 1)
    {
        count += value;
    }

    //所持数のカウントダウン
    public void CountDown(int value = 1)
    {
        count -= value;
    }

    public int GetCount()
    {
        return count;
    }
}

