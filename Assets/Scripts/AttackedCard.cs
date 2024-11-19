using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//攻撃される側のコード
public class AttackedCard : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)//ドロップされた時
    {
        //攻撃
        //attackerを選択　マウスポインターに重なったカードをアタッカーにする
        CardController attackCard = eventData.pointerDrag.GetComponent<CardController>();

        //defenderを選択
        CardController defenceCard = GetComponent<CardController>();

        //バトルする
        GameManager.instance.CardBattle(attackCard, defenceCard);
    }
}
