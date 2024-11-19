using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//カードのリアルタイムデータをデザインに反映するスクリプト
public class CardView : MonoBehaviour
{
    [SerializeField] Text nameText, powerText, costText;
    [SerializeField] Image iconImage;

    [SerializeField] GameObject canAttackPanel,canUsePanel;//攻撃可能かどうかの枠と使用可能かどうかの枠

    public void Show(CardModel cardModel)//cardModelのデータ取得と反映
    {
        nameText.text = cardModel.name;
        powerText.text = cardModel.power.ToString();
        costText.text = cardModel.cost.ToString();
        iconImage.sprite = cardModel.icon;
    }

    public void SetCanAttackPanel(bool flag)//フラグに合わせてcanAttackPanelを付けるor消す
    {
        canAttackPanel.SetActive(flag);
    }

    public void SetCanUsePanel(bool flag)//フラグに合わせてCanUsePanelを付けるor消す
    {
        canUsePanel.SetActive(flag);
    }
    
}
