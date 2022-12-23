using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    [Header("Level")]
    [SerializeField] int way;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform CreatespawnPoint;

    [SerializeField] GameObject spawnObject;
    [SerializeField] List<GameObject> _wayMaterial;
    [SerializeField] List<GameObject> _Objects;
    [SerializeField] List<GameObject> allItems;

    [SerializeField] LevelCreator levelCreator;

    public LevelController levelController;

    [Header("Level Variables")]

    [SerializeField] List<GameObject> CreateItems;
    public float speed = 1f,createObjectSpeed = 3f;
    int createCount = 0, destroyCount = 0;
    int _repeatCount;
    List<GameObject> defaultItems;


    private void Start()
    {



        StartCoroutine(CreateBlock());

        if (way == 1)
        {
            LevelCreate();
            StartCoroutine(CreateObject());
        }

    }

    private void Update()
    {

        for (int i = 0; i < _wayMaterial.Count; i++)
        {
            _wayMaterial[i].transform.position = new Vector3(_wayMaterial[i].transform.position.x + speed * Time.deltaTime * way, _wayMaterial[i].transform.position.y, _wayMaterial[i].transform.position.z);
        }

        for (int i = 0; i < _Objects.Count; i++)
        {
            _Objects[i].transform.position = new Vector3(_Objects[i].transform.position.x + speed * Time.deltaTime * way, _Objects[i].transform.position.y, _Objects[i].transform.position.z);


            if (_Objects[i].transform.position.x < 100 && _Objects[i].transform.position.x > 9)
            {
                DeleteItems(_Objects[i]);
                destroyCount++;
            }
        }
    }

    IEnumerator CreateBlock()
    {
        yield return new WaitForSeconds(0.34f);
        GameObject obj = Instantiate(spawnObject, spawnPoint.position, Quaternion.identity, this.transform);
        obj.transform.rotation = _wayMaterial[10].transform.rotation;
        _wayMaterial.Add(obj);

        if (_wayMaterial.Count > 24)
        {
            obj = _wayMaterial[0];
            _wayMaterial.RemoveAt(0);
            Destroy(obj);
        }

        StartCoroutine(CreateBlock());
    }


    IEnumerator CreateObject()
    {
        if (createCount < CreateItems.Count)
        {
            GameObject obj = Instantiate(CreateItems[createCount], CreatespawnPoint.position, Quaternion.identity, this.transform);
            createCount++;
            _Objects.Add(obj);
            yield return new WaitForSeconds(createObjectSpeed);
            StartCoroutine(CreateObject());
        }
       

    }


   
    public void DeleteItems(GameObject obj)
    {
        _Objects.RemoveAt(0);
        Destroy(obj, 0.3f);
    }


    void LevelCreate()
    {


        if (PlayerPrefs.HasKey("Level"))
        {
            int currentLevel = PlayerPrefs.GetInt("Level");
           // _repeatCount = levelCreator.levels[currentLevel].loopTime;
           // CreateDefaultItem(levelCreator.levels[currentLevel].itemsCount);
        }
        else
        {
            PlayerPrefs.SetInt("Level", 0);
            LevelCreate();
        }
    }

    private void CreateDefaultItem(int createCount)
    {
        defaultItems = new List<GameObject>();
        System.Random rnd = new System.Random();
        for (int i = 0; i < createCount; i++)
        {
            defaultItems.Add(allItems[rnd.Next(0, allItems.Count)]);
        }

        //defaultItems = CreateItems;

        for (int i = 0; i < _repeatCount; i++)
        {
            CreateItems.AddRange(defaultItems);
        }
    }
}
