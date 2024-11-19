using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedLeader : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //攻撃
        //attackerを選択　マウスに重なったカードをアタッカーにする
        CardController attackCard = eventData.pointerDrag.GetComponent<CardController>();

        GameManager.instance.AttackToLeader(attackCard, true);//GameManaerはstaticで"instance"という名前で定義している
    }
}
