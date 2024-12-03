using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // �J�����̉�]���x
    public float cameraRotate_ = 100f;

    // Z���̌Œ�l
    float fixedZRotation_ = 0.0f;

    // �v���C���[��Transform
    public Transform player;

    // �J�����̐�����]�Ɛ�����]�̐���
    private float maxRotate = 75.0f;   
    private float minRotate = -75.0f; 

    // X����Y���̉�]�p�x��ێ�
    private float xRotation = 0.0f; 
    private float yRotation = 0.0f;

    //�N���X�w�A��Image
    [SerializeField]
    private Image crosshairImg;

    //�N���X�w�A�ƃA�C�e���̐ڐG����
    private bool feelItem = false;

    //��������
    bool isDown = false;

    //�Q�[�W��Image
    [SerializeField]
    private Image Gauge;

    //�A�C�e��
    public GameObject Item;

    //Text�I�u�W�F�N�g�̎擾
    public GameObject getItemText;

    //�v���C���[�̏����A�C�e��
    [SerializeField] private ItemManager itemManager_;

    // Update is called once per frame
    private void Update()
    {
        // �J��������
        cameraMove();

        //Ray
        ItemFind();
    }

    void cameraMove()
    {
        // �}�E�X�̓��͂��擾
        float rotateX = Input.GetAxis("Mouse X") * cameraRotate_ * Time.deltaTime; 
        float rotateY = Input.GetAxis("Mouse Y") * cameraRotate_ * Time.deltaTime;

        // ������]�̐����iX����]�j
        xRotation -= rotateY;
        xRotation = Mathf.Clamp(xRotation, minRotate, maxRotate);

        // ������]�̓K�p
        yRotation += rotateX;

        // �J�����̐����E������]��K�p
        this.transform.localRotation = Quaternion.Euler(xRotation, yRotation, fixedZRotation_);

        // �v���C���[�̐��������iY���j����]������
        //player.Rotate(Vector3.up * rotateX);
        player.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);

        // �J�����̃|�W�V�������v���C���[�̈ʒu�ɒǏ]������
        Vector3 localPos = new Vector3(0.0f, 2.0f, 0.0f);
        Vector3 localRotPos = player.transform.rotation * localPos;
        this.transform.position = player.transform.position + localRotPos;
    }

    private void ItemFind()
    {
        //Ray�̕`��
        Ray ray = new Ray(transform.position, transform.forward);

        //Ray����������Item�̊i�[
        RaycastHit hit;

        //Raycast�̉���
        Debug.DrawRay(ray.origin, ray.direction*5.0f, Color.red);

        var g = Gauge.GetComponent<Image>();

        //OutLine�̎擾
        var outline = Item.GetComponent<Outline>();


        //��������
        if (Input.GetMouseButtonDown(0))
        {
            isDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDown = false;
        }

        //Ray���A�C�e���ɓ������Ă��邩�̔���
        if (Physics.Raycast(ray,out hit))
        {

            //Tag�ŃA�C�e���̔���
            if (hit.collider.CompareTag("Item"))
            {
                feelItem = true;

                getItemText.SetActive(true);

                //�A�C�e���ƐڐG���Ă����OutLine��\��
                outline.OutlineWidth = 2.5f;

                //�����Ă����
                if (isDown)
                {
                    g.fillAmount +=Time.deltaTime/3;

                    if (g.fillAmount >= 1.0f)
                    {
                        //�A�C�e���������
                        GetItem(hit.collider.gameObject);
                        g.fillAmount = 0.0f;
                    }

                }

            }
            else
            {
                getItemText.SetActive(false);

                feelItem= false;

                //OutLine�̔�\��
                outline.OutlineWidth = 0.0f;

            }
        }
        else
        {
            feelItem= false;
            g.fillAmount = 0.0f;

        }

        //�����Ă��Ȃ��Ƃ�
        if (!isDown)
        {
            g.fillAmount = 0.0f;
        }

        //�A�C�e���ƐڐG���Ă���ƃN���X�w�A�̐F��ς���
        crosshairImg.color = feelItem ? Color.red : Color.white;

    }

    private void GetItem(GameObject item)
    {
        //�A�C�e�����E������
        item.SetActive(false);

        var bomb = item.GetComponent<ItemBomb>();
        if(bomb != null)
        {
            itemManager_.CountUpItem(bomb.id_, 1);
        }
    }

}
