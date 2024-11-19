using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//フィールドにアタッチするクラス
public class DropPlace : MonoBehaviour,IDropHandler//Drop型のメソッドの有効化
{
    public void OnDrop(PointerEventData eventData)//ドロップされた時に行う処理
    {
        //PointerEventData eventDataはマウスのデータ、pointerDragはdragされているゲームオブジェクトを取得（予測）
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();//ドラッグしてきた情報からCardMovementを取得
        if (card != null)//もしカードがあれば
        {
            card.cardParent = this.transform;//カード要素を自分（アタッチされているオブジェクト）にする
        }

    }
}
