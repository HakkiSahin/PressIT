using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElephantSDK;


public class AdsClass : MonoBehaviour
{
    static AdsClass instance;
    protected AdsClass()
    {
    }
    public static AdsClass Instance()
    {

        if (instance == null)
        {
            instance = new AdsClass();
        }
        return instance;
    }

    public void StartGame()
    {
        Debug.Log("Start Game Method");
        Elephant.LevelStarted(PlayerPrefs.GetInt("my_level") + 1);
    }


    public void EndGame()
    {
        Elephant.LevelCompleted(PlayerPrefs.GetInt("my_level") + 1);
        PlayerPrefs.SetInt("my_level", PlayerPrefs.GetInt("my_level")+1);
    }
}

