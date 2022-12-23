using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Feedbacks;


public class DestroyController : MonoBehaviour
{
    [SerializeField] List<GameObject> _objs;
    [SerializeField] CameraShake cameraShake;

    [SerializeField] public followerPool follower;
    [SerializeField] Transform spawnPoint;

    public MMFeedbacks feedback;

    private void Start()
    {
        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        follower = GameObject.Find("LevelManager").GetComponent<followerPool>();
        spawnPoint = GameObject.Find("IncomeSpawn").transform;
        feedback = GameObject.Find("FeedBacks").GetComponent<MMFeedbacks>();
    }

    public void ChangeDestroyObject()
    {
        _objs[0].SetActive(false);
        _objs[1].SetActive(true);
        StartCoroutine(cameraShake.Shake(0.2f, 0.3f));

        if (this.transform.GetChild(0).tag == "Rare")
        {
            StartCoroutine(DelayLoop(10));
        }
        else if (this.transform.GetChild(0).tag == "Rare_1")
        {
            StartCoroutine(DelayLoop(25));
        }
        else
            follower.ActivateDeactivate(true, spawnPoint.position, 1);

        LevelController.smashCount++;

        feedback.PlayFeedbacks();
    }

    int count = 0;

    IEnumerator DelayLoop(int countSize)
    {
        yield return new WaitForSeconds(0.1f);
        count++;

        if (count < countSize)
        {
            follower.ActivateDeactivate(true, spawnPoint.position, 10);
            StartCoroutine(DelayLoop(countSize));
        }
    }
}