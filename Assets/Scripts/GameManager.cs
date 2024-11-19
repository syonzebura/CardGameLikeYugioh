using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand, playerField, enemyField;

    bool isPlayerTrun = true;
    List<int> deck = new List<int>() { 1, 2, 1, 2, 1, 2, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3 };

    public static GameManager instance;//これでどこからでもGameManagerを呼べる
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {

        StartGame();

    }

    void StartGame()//初期値の設定
    {

        //初期手札を配る
        SetStartHand();

        //ターンの決定
        TrunCalc();
    }

    void CreateCard(int cardID,Transform place)//カードを生成するメソッド
    {
        CardController card = Instantiate(cardPrefab, place);//ここでcardに代入することでカードを個別に管理することができている？
        card.Init(cardID);
    }

    void DrawCard(Transform hand)//カードを引く
    {
        //デッキがないなら引かない
        if (deck.Count == 0)
        {
            return;
        }

        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();//CardController型でリストを作りplayerHandの子オブジェクト達を代入

        if (playerHandCardList.Length < 9)//9枚まで引ける
        {
            //デッキの一番上のカードを抜き取り、手札に加える
            int cardID = deck[0];
            deck.RemoveAt(0);
            CreateCard(cardID, hand);
        }

        
    }

    void SetStartHand()//手札を3枚配る
    {
        for(int i = 0; i < 3; i++)
        {
            DrawCard(playerHand);
        }
    }

    void TrunCalc()//ターンを管理する
    {
        if (isPlayerTrun)
        {
            PlayerTrun();
        }
        else
        {
            EnemyTrun();
        }
    }

    public void ChangeTrun()//ターンエンドボタンにつける処理
    {
        isPlayerTrun = !isPlayerTrun;//ターンを逆にする
        TrunCalc();//ターンを相手に回す
    }

    void PlayerTrun()
    {
        Debug.Log("Playerのターン");

        //自分フィールドのモンスターを攻撃可能にする
        CardController[] playerFieldCardList = playerField.GetComponentsInChildren<CardController>();
        SetAttackableFieldCard(playerFieldCardList, true);

        DrawCard(playerHand);//手札を一枚加える
    }

    void EnemyTrun()
    {
        Debug.Log("Enemeyのターン");
        CardController[] enemyFieldCardList = enemyField.GetComponentsInChildren<CardController>();//CardController型でリストを作りenemyFieldの子オブジェクト達を代入
        if (enemyFieldCardList.Length < 5)//5体まで出せる
        {
            if (enemyFieldCardList.Length < 2)
            {
                CreateCard(3, enemyField);//カードを召喚
            }
            else
            {
                CreateCard(2, enemyField);//カードを召喚
            }
            
        }
        

        ChangeTrun();//ターンエンドする
    }

    public void CardBattle(CardController attackCard,CardController defenceCard)
    {
        //攻撃カードがアタック可能でなければ攻撃しないで処理を終了する（召喚酔いなど）
        if (attackCard.model.canAttack == false)
        {
            return;
        }
        

        //攻撃側のパワーが高かった場合、攻撃されたカードを破壊する
        if (attackCard.model.power > defenceCard.model.power)
        {
            defenceCard.DestroyCard(defenceCard);
        }
        //攻撃された側のパワーが高かった場合、攻撃側のカードを破壊する
        if (attackCard.model.power < defenceCard.model.power)
        {
            defenceCard.DestroyCard(attackCard);
        }
        //パワーが同じだった場合、両方のカードを破壊する
        if (attackCard.model.power == defenceCard.model.power)
        {
            defenceCard.DestroyCard(attackCard);
            defenceCard.DestroyCard(defenceCard);
        }

        attackCard.model.canAttack = false;//攻撃済みにする
    }

    void SetAttackableFieldCard(CardController[] cardList,bool canAttack)//（どちらかの）フィールドのカード全てを攻撃可能（あるいは不可能）にする
    {
        foreach(CardController card in cardList)
        {
            card.model.canAttack = canAttack;
        }
    }
}
