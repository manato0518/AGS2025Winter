using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScroll : MonoBehaviour
{

    public List<GameObject> itemList_;
    public GameObject selectItem_;
    public int selectItemIdx_ = 0;
    public List<Vector2> movePosList_;

    private bool isAnimation_ = false;
    private const float TIME_ANIM = 1.0f;
    private float step_ = 0.0f;

    //ItemÇÃîzíuä‘äu
    private const float WIDTH = 150.0f;
    private Vector2 START_POS = new Vector2(0.0f, 0.0f);


    // Start is called before the first frame update
    void Start()
    {

        // êÆóÒ
        int i = 0;
        foreach (var item in itemList_)
        {
            var rectTran = item.GetComponent<RectTransform>();
            if (rectTran == null)
            {
                continue;
            }

            rectTran.anchoredPosition = START_POS + new Vector2(i * WIDTH, 0.0f);
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Q) && !isAnimation_)
        {
            movePosList_.Clear();
            foreach(var item in itemList_)
            {
                var rectTran = item.GetComponent<RectTransform>();
                if(rectTran == null)
                {
                    continue;
                }

                Vector2 pos = rectTran.anchoredPosition;
                pos.x -= 150.0f;
                movePosList_.Add(pos);
            }

            isAnimation_ = true;
            step_ = 0.0f;

        }

        if(isAnimation_)
        {

            step_ += Time.deltaTime;
            float rate = step_ / TIME_ANIM;
            if(rate >= 1.0f)
            {
                rate = 1.0f;
                isAnimation_ = false;

                int i = 0;
                foreach (var item in itemList_)
                {
                    var rectTran = item.GetComponent<RectTransform>();
                    if (rectTran == null)
                    {
                        continue;
                    }

                    rectTran.anchoredPosition = Vector2.Lerp(
                        rectTran.anchoredPosition,
                        movePosList_[i],
                        rate
                        );
                    i++;
                }

                var rectTran2 = itemList_[selectItemIdx_].GetComponent<RectTransform>();
                if (rectTran2 != null)
                {
                    rectTran2.anchoredPosition = START_POS + new Vector2((itemList_.Count - 1) * WIDTH, 0.0f);

                    selectItemIdx_++;
                    if (selectItemIdx_ >= itemList_.Count)
                    {
                        selectItemIdx_ = 0;
                    }

                    selectItem_ = itemList_[selectItemIdx_];
                }
            }
            else
            {
                int i = 0;
                foreach (var item in itemList_)
                {
                    var rectTran = item.GetComponent<RectTransform>();
                    if (rectTran == null)
                    {
                        continue;
                    }

                    rectTran.anchoredPosition = Vector2.Lerp(
                        rectTran.anchoredPosition,
                        movePosList_[i],
                        rate
                        );
                    i++;
                }

            }

        }

    }
}
