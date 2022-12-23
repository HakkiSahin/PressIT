using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class LevelController : MonoBehaviour
{
    public Slider slider;
    public static float smashCount;
    public Image nextLevel;
    public List<Sprite> levelImage;
    [SerializeField] List<GameObject> allPresses;
    int activePress = 0;

    [SerializeField] GameObject upgradePanel, hand;
    [SerializeField] PressController pressController;
    [SerializeField] followerPool followerPool;
    [SerializeField] ConveyorController conveyorController;
    bool isStart = false;


    [SerializeField] private List<float> nextLevelSmash;

    private void Start()
    {
        Time.timeScale = 0f;
        activePress = PlayerPrefs.HasKey("ActivePress") ? PlayerPrefs.GetInt("ActivePress") : 0;
        allPresses[activePress].SetActive(true);
        slider.maxValue = nextLevelSmash[activePress];
        slider.value = 0f;
        smashCount = 0;
        nextLevel.sprite = levelImage[activePress + 1 > 3 ? 3 : activePress + 1];
        pressController.enabled = false;
    }

    #region LastGameController

    //public static int smashCount;
    //public static int allItemCount;
    //int currentLevel;
    //[Header("Canvas Item")]
    //[SerializeField] List<Image> _stars;
    //float isSuccess;
    //[SerializeField] GameObject levelPanel, TapPanel;
    //public static bool isGame;

    //int currentlevel;
    //private void Start()
    //{
    //    Time.timeScale = 0f;
    //}

    //public void CalculateStar()
    //{
    //    Time.timeScale = 0f;
    //    float starCount = allItemCount / 3;

    //    for (int i = 1; i <= _stars.Count; i++)
    //    {
    //        if (smashCount > starCount * i)
    //        {
    //            _stars[i - 1].fillAmount = 1;
    //            isSuccess += _stars[i - 1].fillAmount;
    //        }
    //        else
    //        {
    //            float calcStar = smashCount - starCount * (i - 1);
    //            _stars[i - 1].fillAmount = (calcStar * 100 / starCount) / 100;
    //            Debug.Log((calcStar * 100 / starCount) / 100);
    //            isSuccess += _stars[i - 1].fillAmount;
    //            break;
    //        }
    //    }
    //    SuccessControl();
    //}

    //void SuccessControl()
    //{
    //    levelPanel.SetActive(true);

    //    if (isSuccess >= 1.5f)
    //    {
    //        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
    //    }
    //    else
    //    {
    //        Debug.Log("Failed Bitch");
    //    }
    //}


    //public void NextLevel()
    //{
    //    SceneManager.LoadScene(0);
    //}


    //public void StartLevel()
    //{
    //    Time.timeScale = 1f;
    //    TapPanel.SetActive(false);
    //}

    #endregion

    private void Update()
    {
        slider.value = smashCount;
        if (Input.GetKeyDown(KeyCode.L))
        {
            smashCount = nextLevelSmash[activePress] - 1;
        }

        if (smashCount >= nextLevelSmash[activePress])
        {
            ChangePress();
            LevelEnd();
        }
    }

    private void ChangePress()
    {
        allPresses[activePress].SetActive(false);
        allPresses[activePress + 1 >= 4 ? 3 : activePress + 1].SetActive(true);
        PlayerPrefs.SetInt("ActivePress", activePress + 1 > 3 ? 3 : activePress + 1);
        UpgradeItems();
        LevelEnd();
    }

    private void UpgradeItems()
    {
        float pressP = PlayerPrefs.GetFloat("PressPower") > 0
            ? PlayerPrefs.GetFloat("PressPower")
            : 5;

        Debug.Log("PRess " + pressP + "  " + (pressP - 5) / 2 + 5);
        pressP = (pressP - 5) / 2 + 5;
        if (pressP % 0.5f == 0) PlayerPrefs.SetFloat("PressPower", pressP);

        else PlayerPrefs.SetFloat("PressPower", pressP + 0.25f);


        float conveyorS = PlayerPrefs.GetFloat("ConveyorSpeed") > 0
            ? PlayerPrefs.GetFloat("ConveyorSpeed")
            : 2;
        Debug.Log("Con " + conveyorS + "  " + (conveyorS - 2) / 2 + 2);

        conveyorS = (conveyorS - 2) / 2 + 2;

        if (conveyorS % 0.05 == 0) PlayerPrefs.SetFloat("ConveyorSpeed", conveyorS);

        else PlayerPrefs.SetFloat("ConveyorSpeed", conveyorS + 0.025f);
    }

    public void StartGame()
    {
        if (!isStart)
        {
            Time.timeScale = 1f;
            upgradePanel.SetActive(false);
            hand.SetActive(true);
            isStart = true;
            pressController.enabled = true;
            followerPool.enabled = true;
            AddUpdate();
            AdsClass.Instance().StartGame();
        }
    }

    private void LevelEnd()
    {
        Debug.Log("Level End Method");
        AdsClass.Instance().EndGame();
        SceneManager.LoadScene(1);
    }

    void AddUpdate()
    {
        pressController.speed = PlayerPrefs.GetFloat("PressPower") > 0 ? PlayerPrefs.GetFloat("PressPower") : 5;
        //  followerPool.poolObject.GetComponent<TextMeshProUGUI>().text = "$" + PlayerPrefs.GetFloat("Income").ToString();
        // conveyorController.converyorSpeed =
        //     PlayerPrefs.HasKey("ConveyorSpeed") ? PlayerPrefs.GetFloat("ConveyorSpeed") : 2;
        // conveyorController.createSpeed = 6 / conveyorController.converyorSpeed;

        conveyorController.enabled = true;
    }
}