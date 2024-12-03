using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    //ボム
    BOMB = 0,
    //スタンボム
    STUNBOMB = 1,
}

[CreateAssetMenu(menuName = "BlogAssets/ItemSourceData")]
public class ItemBasisData : ScriptableObject
{
    //アイテム識別用ID
    [SerializeField] private string id_;
    //IDを取得
    public string id
    {
        get { return id_;}
    }

    //アイテムの名前
    [SerializeField] private string itemName_;
    //アイテム名を取得
    public string itemName
    { 
        get {  return itemName_;} 
    }

    //アイテムの種類
    [SerializeField] private ItemType type_;
    //アイテムの種類を取得
    public ItemType type
    {
        get { return type_; }
    }

    //アイテムのPrefab
    [SerializeField] private GameObject prefab_;
    //アイテムのPrefabを取得
    public GameObject prefab
    {
        get {return prefab_; }
    }

    //ダメージ
    [SerializeField] private float damge_;
    //ダメージを取得
    public float damge
    {
        get {return damge_;}
    }

}
