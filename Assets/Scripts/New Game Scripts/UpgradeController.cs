using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UpgradeController : MonoBehaviour
{
    float _IncomeMoney;
    float _PressPower;
    float _ConveyorSpeed;

    [Header("Update Item Text")] [SerializeField]
    TextMeshProUGUI incomeText;

    [SerializeField] TextMeshProUGUI pressText;
    [SerializeField] TextMeshProUGUI conveyorText;
    [SerializeField] TextMeshProUGUI myMoney;

    [SerializeField] float incomeUpgradeIncrease;
    [SerializeField] float pressPowerUpgradeIncrease;
    [SerializeField] float conveyorSpeedUpgradeIncrease;


    [SerializeField] TextMeshProUGUI incomeLevelText;
    [SerializeField] TextMeshProUGUI pressLevelText;
    [SerializeField] TextMeshProUGUI conveyorLevelText;


    //public TextMeshProUGUI textMesh;

    float testInt;
    bool isBuyUpgrade;

    void Start()
    {
        myMoney.text = (PlayerPrefs.HasKey("Money") ? PlayerPrefs.GetFloat("Money") : 25f).ToString("0.0");

        _ConveyorSpeed =
            PlayerPrefs.GetFloat("ConveyorSpeed") > 0
                ? PlayerPrefs.GetFloat("ConveyorSpeed")
                : 2; //0.05f // Money : 10 //15 20 25 30 5.1  5.5  5/0.05 level * 5 + 10 5-
        _PressPower =
            PlayerPrefs.GetFloat("PressPower") > 0
                ? PlayerPrefs.GetFloat("PressPower")
                : 5;
        _IncomeMoney =
            PlayerPrefs.GetFloat("Income") > 0
                ? PlayerPrefs.GetFloat("Income")
                : 1.5f; // 0.2f;   / 2.1f - 1.5f/0.2f = 3

        UpdateText();
    }

    // Pow-Level-Money
    public void Upgrade(TextMeshProUGUI moneyText) // Ugrade Inc Items
    {
        MoneyCalculate(float.Parse(moneyText.text));

        if (isBuyUpgrade)
        {
            if (moneyText.name == "Income")
            {
                _IncomeMoney = (float)System.Math.Round((double)(_IncomeMoney + incomeUpgradeIncrease), 1);
                PlayerPrefs.SetFloat("Income", _IncomeMoney);
            }
            else if (moneyText.name == "PressPower")
            {
                _PressPower = (float)System.Math.Round((double)(_PressPower + pressPowerUpgradeIncrease), 1);
                PlayerPrefs.SetFloat("PressPower", _PressPower);
                Debug.Log(_PressPower);
            }
            else if (moneyText.name == "ConveyorSpeed")
            {
                _ConveyorSpeed = (float)System.Math.Round((double)(_ConveyorSpeed + conveyorSpeedUpgradeIncrease), 2);
                PlayerPrefs.SetFloat("ConveyorSpeed", _ConveyorSpeed);
            }

            float level = int.Parse(moneyText.transform.parent.GetChild(2).GetComponent<TextMeshProUGUI>().text);
            float money = int.Parse(moneyText.transform.parent.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            level++;
            money += moneyText.name == "PressPower" ? 10 : 5;
            moneyText.transform.parent.GetChild(2).GetComponent<TextMeshProUGUI>().text = level.ToString();
            moneyText.transform.parent.GetChild(0).GetComponent<TextMeshProUGUI>().text = money.ToString("R");
            isBuyUpgrade = false;
        }
    }

    void MoneyCalculate(float money)
    {
        if ((float.Parse(myMoney.text) > money))
        {
            //myMoney.text = (float.Parse(myMoney.text) - money).ToString("R");
            myMoney.text = System.MathF.Round(float.Parse(myMoney.text) - money).ToString();
            isBuyUpgrade = true;
            PlayerPrefs.SetFloat("Money", float.Parse(myMoney.text));
            //UpdateText();
        }
    }

    //Pow-Lvl-Money
    void UpdateText()
    {
        incomeLevelText.text = (Mathf.Round((_IncomeMoney - 1.5f) / incomeUpgradeIncrease)).ToString();
        incomeText.text = (Mathf.Round(float.Parse(incomeLevelText.text) * 5 + 10)).ToString();

        pressLevelText.text = (Mathf.Round((_PressPower - 5f) / pressPowerUpgradeIncrease)).ToString();
        pressText.text = (Mathf.Round(float.Parse(pressLevelText.text) * 10 + 10)).ToString();

        conveyorLevelText.text = (Mathf.Round((_ConveyorSpeed - 2f) / conveyorSpeedUpgradeIncrease)).ToString();
        conveyorText.text = (Mathf.Round(float.Parse(conveyorLevelText.text) * 5 + 10)).ToString();
    }
}


public enum UpdateItem
{
    Income,
    PressPower,
    ConveyorSpeed
}