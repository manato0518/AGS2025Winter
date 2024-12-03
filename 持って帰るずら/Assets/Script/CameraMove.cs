using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // カメラの回転速度
    public float cameraRotate_ = 100f;

    // Z軸の固定値
    float fixedZRotation_ = 0.0f;

    // プレイヤーのTransform
    public Transform player;

    // カメラの垂直回転と水平回転の制限
    private float maxRotate = 75.0f;   
    private float minRotate = -75.0f; 

    // X軸とY軸の回転角度を保持
    private float xRotation = 0.0f; 
    private float yRotation = 0.0f;

    //クロスヘアのImage
    [SerializeField]
    private Image crosshairImg;

    //クロスヘアとアイテムの接触判定
    private bool feelItem = false;

    //押下判定
    bool isDown = false;

    //ゲージのImage
    [SerializeField]
    private Image Gauge;

    //アイテム
    public GameObject Item;

    //Textオブジェクトの取得
    public GameObject getItemText;

    //プレイヤーの所持アイテム
    [SerializeField] private ItemManager itemManager_;

    // Update is called once per frame
    private void Update()
    {
        // カメラ操作
        cameraMove();

        //Ray
        ItemFind();
    }

    void cameraMove()
    {
        // マウスの入力を取得
        float rotateX = Input.GetAxis("Mouse X") * cameraRotate_ * Time.deltaTime; 
        float rotateY = Input.GetAxis("Mouse Y") * cameraRotate_ * Time.deltaTime;

        // 垂直回転の制限（X軸回転）
        xRotation -= rotateY;
        xRotation = Mathf.Clamp(xRotation, minRotate, maxRotate);

        // 水平回転の適用
        yRotation += rotateX;

        // カメラの垂直・水平回転を適用
        this.transform.localRotation = Quaternion.Euler(xRotation, yRotation, fixedZRotation_);

        // プレイヤーの水平方向（Y軸）を回転させる
        //player.Rotate(Vector3.up * rotateX);
        player.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);

        // カメラのポジションをプレイヤーの位置に追従させる
        Vector3 localPos = new Vector3(0.0f, 2.0f, 0.0f);
        Vector3 localRotPos = player.transform.rotation * localPos;
        this.transform.position = player.transform.position + localRotPos;
    }

    private void ItemFind()
    {
        //Rayの描画
        Ray ray = new Ray(transform.position, transform.forward);

        //Rayが当たったItemの格納
        RaycastHit hit;

        //Raycastの可視化
        Debug.DrawRay(ray.origin, ray.direction*5.0f, Color.red);

        var g = Gauge.GetComponent<Image>();

        //OutLineの取得
        var outline = Item.GetComponent<Outline>();


        //押下判定
        if (Input.GetMouseButtonDown(0))
        {
            isDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDown = false;
        }

        //Rayがアイテムに当たっているかの判定
        if (Physics.Raycast(ray,out hit))
        {

            //Tagでアイテムの判定
            if (hit.collider.CompareTag("Item"))
            {
                feelItem = true;

                getItemText.SetActive(true);

                //アイテムと接触しているとOutLineを表示
                outline.OutlineWidth = 2.5f;

                //押している間
                if (isDown)
                {
                    g.fillAmount +=Time.deltaTime/3;

                    if (g.fillAmount >= 1.0f)
                    {
                        //アイテム回収処理
                        GetItem(hit.collider.gameObject);
                        g.fillAmount = 0.0f;
                    }

                }

            }
            else
            {
                getItemText.SetActive(false);

                feelItem= false;

                //OutLineの非表示
                outline.OutlineWidth = 0.0f;

            }
        }
        else
        {
            feelItem= false;
            g.fillAmount = 0.0f;

        }

        //押していないとき
        if (!isDown)
        {
            g.fillAmount = 0.0f;
        }

        //アイテムと接触しているとクロスヘアの色を変える
        crosshairImg.color = feelItem ? Color.red : Color.white;

    }

    private void GetItem(GameObject item)
    {
        //アイテムを拾う処理
        item.SetActive(false);

        var bomb = item.GetComponent<ItemBomb>();
        if(bomb != null)
        {
            itemManager_.CountUpItem(bomb.id_, 1);
        }
    }

}
