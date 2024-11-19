using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//カードのリアルタイムデータ
public class CardModel : MonoBehaviour
{
    //CardEntitiyに設定したステータスに対応してるデータ
    public int cardId;
    public string name;
    public int cost;
    public int power;
    public Sprite icon;

    public bool canUse = false;//手札からカードを使用可能かどうか

    public bool PlayerCard = false;//誰のカードか

    public bool FieldCard = false;//どこのカードか

    public bool canAttack = false;//攻撃できるかどうか




    public CardModel(int cardID,bool playerCard)//ココちょっと特殊//データを受け取り、その処理
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + cardID);//CardEitityのパス。実際に名前で検索かけてる

        cardId = cardEntity.cardId;
        name = cardEntity.name;
        cost = cardEntity.cost;
        power = cardEntity.power;
        icon = cardEntity.icon;

        PlayerCard = playerCard;
    }
}
