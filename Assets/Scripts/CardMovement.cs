using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler//Drag型のメソッド有効化のために継承
{
    public Transform cardParent;

    public void OnBeginDrag(PointerEventData eventData)//ドラッグを始める時に行う処理（OnBeginDragなどのメソッドは予測変換ででない！）
    {
        //（引数はポインターを取得してくれている）
        cardParent = transform.parent;//現在の親オブジェクトを代入
        transform.SetParent(cardParent.parent, false);//現在の親オブジェクトの親オブジェクトを親に設定。第二引数(bool)でローカル座標を維持することを宣言
        GetComponent<CanvasGroup>().blocksRaycasts = false;//blocksRaycastsをオフにする
        //CanvasGroupはUIの子オブジェクトに影響を与える。blockRaycastsはRayにぶつかるかどうか。falseにしないとcard以外のものも反応してしまうため？わからん
    }

    public void OnDrag(PointerEventData eventData)//ドラッグした時起こす処理
    {
        transform.position = eventData.position;//カードの場所をマウスの場所(eventData.position)にする
    }

    public void OnEndDrag(PointerEventData eventData)//カードを離した時に行う処理
    {
        transform.SetParent(cardParent, false);//現在の親オブジェクトを親に設定。
        GetComponent<CanvasGroup>().blocksRaycasts = true;////blocksRaycastsをオンにする
    }
}
