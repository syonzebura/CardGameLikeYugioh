using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform playerHand, playerField, enemyField;

    bool isPlayerTrun = true;
    List<int> deck = new List<int>() { 1, 2, 1, 2, 1, 2, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3 };

    //リーダーHP関係
    public int playerLeaderHP;
    public int enemyLeaderHP;
    [SerializeField] Text playerLeaderHPText;
    [SerializeField] Text enemyLeaderHPText;

    //マナポイント関係
    [SerializeField] Text playerManaPointText;
    [SerializeField] Text playerDefaultManaPointText;
    public int playerManaPoint;//使用すると減るマナポイント
    public int playerDefaultManaPoint;//毎ターン増えていくベースのマナポイント

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
        //各リーダーの初期HP
        enemyLeaderHP = 5000;
        playerLeaderHP = 5000;
        ShowLeaderHP();

        //マナの初期値設定
        playerManaPoint = 1;
        playerDefaultManaPoint = 1;
        ShowManaPoint();

        //初期手札を配る
        SetStartHand();

        //ターンの決定
        TrunCalc();
    }

    void ShowManaPoint()//マナポイントを表示するメソッド
    {
        playerManaPointText.text = playerManaPoint.ToString();
        playerDefaultManaPointText.text = playerDefaultManaPoint.ToString();
    }

    public void ReduceManaPoint(int cost)//コストの分、マナコストを減らすメソッド
    {
        playerManaPoint -= cost;
        ShowManaPoint();

        SetCanUsePanelHand();
    }

    void SetCanUsePanelHand()//手札のカードを取得して、使用可能なカードにCanUsePanelを付ける
    {
        CardController[] playerHandCardList = playerHand.GetComponentsInChildren<CardController>();
        foreach(CardController card in playerHandCardList)
        {
            if (card.model.cost <= playerManaPoint)
            {
                card.model.canUse = true;
                card.view.SetCanUsePanel(card.model.canUse);
            }
            else
            {
                card.model.canUse = false;
                card.view.SetCanUsePanel(card.model.canUse);
            }
        }
    }

    void CreateCard(int cardID,Transform place)//カードを生成するメソッド
    {
        CardController card = Instantiate(cardPrefab, place);//ここでcardに代入することでカードを個別に管理することができている？

        //Playerの手札に生成されたカードはPlayerのカードとする
        if (place == playerHand)
        {
            card.Init(cardID, true);
        }
        else
        {
            card.Init(cardID, false);
        }
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

        SetCanUsePanelHand();

        
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

        //マナを増やす
        playerDefaultManaPoint++;
        playerManaPoint = playerDefaultManaPoint;
        ShowManaPoint();

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

    public void CardBattle(CardController attackCard,CardController defenceCard)//カードの攻撃処理
    {
        //攻撃カードと攻撃されるカードが同じプレイヤーのカードならバトルしない
        if (attackCard.model.PlayerCard == defenceCard.model.PlayerCard)
        {
            return;
        }

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
        attackCard.view.SetCanAttackPanel(false);//攻撃可能な枠を消す
    }

    void SetAttackableFieldCard(CardController[] cardList,bool canAttack)//（どちらかの）フィールドのカード全てを攻撃可能（あるいは不可能）にする
    {
        foreach(CardController card in cardList)
        {
            card.model.canAttack = canAttack;
            card.view.SetCanAttackPanel(canAttack);//攻撃可能かの枠を出すか消すか
        }
    }

    public void AttackToLeader(CardController attackCard,bool isPlayerCard)//リーダーへの攻撃処理
    {
        if (attackCard.model.canAttack == false)
        {
            return;
        }

        enemyLeaderHP -= attackCard.model.power;

        attackCard.model.canAttack = false;
        attackCard.view.SetCanAttackPanel(false);//攻撃可能な枠を消す
        Debug.Log("敵のHPは" + enemyLeaderHP);
        ShowLeaderHP();
    }

    public void ShowLeaderHP()//LeaderHPをテキストUIに反映する
    {
        if (playerLeaderHP <= 0)
        {
            playerLeaderHP = 0;
        }
        if (enemyLeaderHP <= 0)
        {
            enemyLeaderHP = 0;
        }

        playerLeaderHPText.text = playerLeaderHP.ToString();
        enemyLeaderHPText.text = enemyLeaderHP.ToString();
    }
}
