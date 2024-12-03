using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//�A�C�e���Ǘ��N���X
public class ItemManager : MonoBehaviour
{
    //�A�C�e���̃\�[�X���X�g
    [SerializeField] private List<ItemBasisData> itemBasisDataList_;

    //�v���C���[�̏����A�C�e��
    public List<ItemData> playerHaveItemList_ = new List<ItemData>();

    //Item�������n���p
    public int GetItemHave(string id)
    {
        for (int i = 0; i < playerHaveItemList_.Count; i++)
        {
            //���łɏ������Ă�����J�E���g
            if (playerHaveItemList_[i].id == id)
            {
                return playerHaveItemList_[i].GetCount();
            }
        }
        return 0;
    }

    private void Awake()
    {
        //�A�C�e����{�f�[�^�̃��[�h
        LoadItemData();
    }

    //�A�C�e����{�f�[�^�̃��[�h
    private void LoadItemData()
    {
        itemBasisDataList_ = Resources.LoadAll("Weapon/BasisData", typeof(ItemBasisData)).Cast<ItemBasisData>().ToList();
    }

    //�A�C�e���̃\�[�X�f�[�^���擾
    public ItemBasisData GetItemBasisData(string id)
    {
        //�A�C�e��������
        foreach(var basisData in itemBasisDataList_)
        {
            //ID����v���Ă�����
            if(basisData.id == id)
            {
                return basisData;
            }
        }
        return null;
    }

    //�A�C�e�����擾
    public void CountUpItem(string itemId, int count)
    {
        for(int i = 0; i <playerHaveItemList_.Count; i++) 
        {
            //���łɏ������Ă�����J�E���g
            if (playerHaveItemList_[i].id == itemId)
            {
                playerHaveItemList_[i].CountUp(count);
                break;
            }
        }

        //���߂ďE���A�C�e��
        ItemData itemData = new ItemData(itemId, count);
        playerHaveItemList_.Add(itemData);
    }

    public void CountDownItem(string itemId, int count)
    {
        for (int i = 0; i < playerHaveItemList_.Count; i++)
        {
            //���łɏ������Ă�����J�E���g
            if (playerHaveItemList_[i].id == itemId)
            {
                playerHaveItemList_[i].CountDown(count);
                break;
            }
        }
    }


}



