using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カード管理メソッドをまとめたスクリプト（データの受け渡し系）

public class CardController : MonoBehaviour
{
    public CardView view;//カードの見た目の処理
    public CardModel model;//カードのデータを処理

    public void Awake()
    {
        view = GetComponent<CardView>();
    }

    public void Init(int cardID)//カードを生成した時に呼ばれる関数
    {
        model = new CardModel(cardID);//カードデータを生成
        view.Show(model);//表示
    }

    public void DestroyCard(CardController card)//呼ばれたらカードを削除
    {
        Destroy(card.gameObject);
    }
}
