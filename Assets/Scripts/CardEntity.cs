using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カードのオリジナルデータ
[CreateAssetMenu(fileName ="CardEntity",menuName ="Create CardEntity")]//自作アセットを定義

public class CardEntity : ScriptableObject
{
    public int cardId;
    public new string name;
    public int cost;
    public int power;
    public Sprite icon;
}
