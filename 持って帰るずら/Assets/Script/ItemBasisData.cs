using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    //�{��
    BOMB = 0,
    //�X�^���{��
    STUNBOMB = 1,
}

[CreateAssetMenu(menuName = "BlogAssets/ItemSourceData")]
public class ItemBasisData : ScriptableObject
{
    //�A�C�e�����ʗpID
    [SerializeField] private string id_;
    //ID���擾
    public string id
    {
        get { return id_;}
    }

    //�A�C�e���̖��O
    [SerializeField] private string itemName_;
    //�A�C�e�������擾
    public string itemName
    { 
        get {  return itemName_;} 
    }

    //�A�C�e���̎��
    [SerializeField] private ItemType type_;
    //�A�C�e���̎�ނ��擾
    public ItemType type
    {
        get { return type_; }
    }

    //�A�C�e����Prefab
    [SerializeField] private GameObject prefab_;
    //�A�C�e����Prefab���擾
    public GameObject prefab
    {
        get {return prefab_; }
    }

    //�_���[�W
    [SerializeField] private float damge_;
    //�_���[�W���擾
    public float damge
    {
        get {return damge_;}
    }

}
