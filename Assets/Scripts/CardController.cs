using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カード管理メソッドをまとめたスクリプト（データの受け渡し系）

public class CardController : MonoBehaviour
{
    public CardView view;//カードの見た目の処理
    public CardModel model;//カードのデータを処理
    public CardMovement movement;//移動（movement)に関することを操作

    public void Awake()
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
    }

    public void Init(int cardID,bool playerCard)//カードを生成した時に呼ばれる関数
    {
        model = new CardModel(cardID,playerCard);//カードデータを生成
        view.Show(model);//表示
    }

    public void DestroyCard(CardController card)//呼ばれたらカードを削除
    {
        Destroy(card.gameObject);
    }

    public void DropField()//カードがFieldにドロップした時
    {
        GameManager.instance.ReduceManaPoint(model.cost);
        model.FieldCard = true;//フィールドのカードのフラグを立てる
        model.canUse = false;
        view.SetCanUsePanel(model.canUse);//出した時にCanUsePanelを消す

    }
}
