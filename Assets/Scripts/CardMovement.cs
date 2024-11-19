using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler//Drag型のメソッド有効化のために継承
{
    public Transform cardParent;
    bool canDrag = true;//カードを動かせるかどうかのフラグ

    public void OnBeginDrag(PointerEventData eventData)//ドラッグを始める時に行う処理（OnBeginDragなどのメソッドは予測変換ででない！）
    {
        //（引数はポインターを取得してくれている）

        //カードが動かせるかどうかの判定
        CardController card = GetComponent<CardController>();
        canDrag = true;

        if (card.model.FieldCard == false)
        {
            if (card.model.canUse == false)//canUseの状態に合わせてdragできるか設定//マナコストより少ないカードは動かせない
            {
                canDrag = false;
            }
        }
        else
        {
            if (card.model.canAttack == false)//攻撃済みのカードは動かせない
            {
                canDrag = false;
            }
        }
        
        if (canDrag == false)//canDragによって中断するか決める
        {
            return;
        }

        cardParent = transform.parent;//現在の親オブジェクトを代入
        transform.SetParent(cardParent.parent, false);//現在の親オブジェクトの親オブジェクトを親に設定。第二引数(bool)でローカル座標を維持することを宣言
        GetComponent<CanvasGroup>().blocksRaycasts = false;//blocksRaycastsをオフにする
        //CanvasGroupはUIの子オブジェクトに影響を与える。blockRaycastsはRayにぶつかるかどうか。falseにしないとcard以外のものも反応してしまうため？わからん
    }

    public void OnDrag(PointerEventData eventData)//ドラッグした時起こす処理
    {
        if (canDrag == false)
        {
            return;
        }

        transform.position = eventData.position;//カードの場所をマウスの場所(eventData.position)にする
    }

    public void OnEndDrag(PointerEventData eventData)//カードを離した時に行う処理
    {
        if (canDrag == false)
        {
            return;
        }

        transform.SetParent(cardParent, false);//現在の親オブジェクトを親に設定。
        GetComponent<CanvasGroup>().blocksRaycasts = true;////blocksRaycastsをオンにする
    }
}
