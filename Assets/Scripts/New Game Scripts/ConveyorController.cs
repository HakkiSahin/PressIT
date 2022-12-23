using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    public float converyorSpeed, createSpeed;

    [Header("Level Item")]
    //[SerializeField] List<GameObject> _wayMaterial;
    [SerializeField]
    GameObject Item;

    [SerializeField] LevelCreator levelCreator;
    [SerializeField] GameObject conveyorMat;
    [SerializeField] Transform spawnPoint, itemSpawnPoint;
    [SerializeField] List<GameObject> rareItem;
    List<GameObject> createItems = new List<GameObject>();
    [SerializeField] private ParticleSystem _particleSystem;

    [Header("Level Variables")] [SerializeField]
    List<float> levelRareItemPer;

    [SerializeField] List<int> levelItemCount;

    int count = 0;

    private int activeLevel;

    void Start()
    {
        activeLevel = (PlayerPrefs.GetInt("ActivePress"));
        createItems = levelCreator
            .levels[(PlayerPrefs.GetInt("ActivePress") > -1 ? PlayerPrefs.GetInt("ActivePress") : 0)].levelItems;
        converyorSpeed = PlayerPrefs.HasKey("ConveyorSpeed") ? PlayerPrefs.GetFloat("ConveyorSpeed") : 2;

        SetCreateSpeed();

        //StartCoroutine(CreateConveyor());
        StartCoroutine(CreateItem());
    }

    private void SetCreateSpeed()
    {
        //converyorSpeed = converyorSpeed > 6 ? 6 : converyorSpeed;

        if (converyorSpeed >= 2 && converyorSpeed <= 3)
            createSpeed = 1.5f - (converyorSpeed - 2) / 2;
        else if (converyorSpeed > 3 && converyorSpeed <= 4)
            createSpeed = 1 - (converyorSpeed - 3) / 4;
        else if (converyorSpeed > 4 && converyorSpeed <= 5)
            createSpeed = 0.75f - (converyorSpeed - 3) / 8;
        else if (converyorSpeed > 5 && converyorSpeed <= 6)
            createSpeed = 0.625f - (converyorSpeed - 3) / 16;

        Debug.Log(createSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).transform.position = new Vector3(
                this.transform.GetChild(i).transform.position.x + converyorSpeed * Time.deltaTime * 1,
                this.transform.GetChild(i).transform.position.y, this.transform.GetChild(i).transform.position.z);
            if (transform.GetChild(i).transform.position.x > 16)
            {
                // DeleteItem(transform.GetChild(i).gameObject);
            }
        }
    }

    private void DeleteItem(GameObject obj)
    {
        Destroy(obj);
    }

    //IEnumerator CreateConveyor()
    //{
    //    GameObject obj = Instantiate(conveyorMat, spawnPoint.position, Quaternion.identity, this.transform);
    //    obj.transform.rotation = spawnPoint.rotation;

    //    //_wayMaterial.Add(obj);
    //    yield return new WaitForSeconds(0.30f);
    //    StartCoroutine(CreateConveyor());
    //}

    IEnumerator CreateItem()
    {
        yield return new WaitForSeconds(createSpeed);
        count++;
        if (Random.Range(0, 100) < levelRareItemPer[activeLevel] && count > levelItemCount[activeLevel])
            //if (Random.Range(0, 100) < 90 && count > 2)
        {
            _particleSystem.Play();
            if (activeLevel > 2)
            {
                GameObject obj = Instantiate(rareItem[Random.Range(0, 100) > activeLevel * 20 ? 0 : 1],
                    itemSpawnPoint.position, Quaternion.identity, this.transform);
            }
            else
            {
                GameObject obj = Instantiate(rareItem[0], itemSpawnPoint.position, Quaternion.identity, this.transform);
            }
        }
        else
        {
            GameObject obj = Instantiate(createItems[Random.Range(0, createItems.Count)], itemSpawnPoint.position,
                Quaternion.identity, this.transform);
        }

        StartCoroutine(CreateItem());
    }
}