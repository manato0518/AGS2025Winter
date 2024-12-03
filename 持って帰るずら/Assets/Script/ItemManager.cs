using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//アイテム管理クラス
public class ItemManager : MonoBehaviour
{
    //アイテムのソースリスト
    [SerializeField] private List<ItemBasisData> itemBasisDataList_;

    //プレイヤーの所持アイテム
    public List<ItemData> playerHaveItemList_ = new List<ItemData>();

    //Item数引き渡し用
    public int GetItemHave(string id)
    {
        for (int i = 0; i < playerHaveItemList_.Count; i++)
        {
            //すでに所持していたらカウント
            if (playerHaveItemList_[i].id == id)
            {
                return playerHaveItemList_[i].GetCount();
            }
        }
        return 0;
    }

    private void Awake()
    {
        //アイテム基本データのロード
        LoadItemData();
    }

    //アイテム基本データのロード
    private void LoadItemData()
    {
        itemBasisDataList_ = Resources.LoadAll("Weapon/BasisData", typeof(ItemBasisData)).Cast<ItemBasisData>().ToList();
    }

    //アイテムのソースデータを取得
    public ItemBasisData GetItemBasisData(string id)
    {
        //アイテムを検索
        foreach(var basisData in itemBasisDataList_)
        {
            //IDが一致していたら
            if(basisData.id == id)
            {
                return basisData;
            }
        }
        return null;
    }

    //アイテムを取得
    public void CountUpItem(string itemId, int count)
    {
        for(int i = 0; i <playerHaveItemList_.Count; i++) 
        {
            //すでに所持していたらカウント
            if (playerHaveItemList_[i].id == itemId)
            {
                playerHaveItemList_[i].CountUp(count);
                break;
            }
        }

        //初めて拾うアイテム
        ItemData itemData = new ItemData(itemId, count);
        playerHaveItemList_.Add(itemData);
    }

    public void CountDownItem(string itemId, int count)
    {
        for (int i = 0; i < playerHaveItemList_.Count; i++)
        {
            //すでに所持していたらカウント
            if (playerHaveItemList_[i].id == itemId)
            {
                playerHaveItemList_[i].CountDown(count);
                break;
            }
        }
    }


}



