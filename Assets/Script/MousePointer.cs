using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//マウスカーソルの見た目
public class MousePointer : MonoBehaviour
{
    [SerializeField] Image Mouse_Image;//設定ちゃんとRayCastTargetにしてね
    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform canvasRect;
    Vector2 MousePos;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect,
                Input.mousePosition, canvas.worldCamera, out MousePos);

        // Mouse_Imageを表示する位置にMousePosを使う
        Mouse_Image.GetComponent<RectTransform>().anchoredPosition
             = new Vector2(MousePos.x, MousePos.y);
    }

    public Vector2 GetMousePos() { return MousePos; }
}
