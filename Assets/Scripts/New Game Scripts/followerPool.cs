using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;


public class followerPool : MonoBehaviour
{
    public static followerPool _pool;
    public Canvas canvas;
    public GameObject poolObject;

    List<GameObject> pool = new List<GameObject>();
    List<RectTransform> rect = new List<RectTransform>();
    public int size;

    Vector2 offset;
    int counter;
    float timer;
    public RectTransform toGo;
    Vector2 toGo_pos;
    bool boolStart;
    public float speed;

    List<Vector2> randomPos = new List<Vector2>();
    Vector2 pos;

    int count;

    public TextMeshProUGUI textMesh;

    public float money;

    void Start()
    {
        money = PlayerPrefs.HasKey("Income") ? PlayerPrefs.GetFloat("Income") : 1.5f;
        poolObject.GetComponent<TextMeshProUGUI>().text = "$" + money.ToString();
        _pool = this;
        offset = new Vector2(Screen.width, Screen.height) / 2f;

        toGo_pos = toGo.position;

        for (int i = 0; i < size; i++)
        {
            GameObject copy = Instantiate(poolObject, transform.InverseTransformPoint(Vector3.zero),
                Quaternion.identity, canvas.transform);
            pool.Add(copy);
            rect.Add(copy.GetComponent<RectTransform>());
            rect[i].position = Vector2.zero + offset;
            randomPos.Add(Random.insideUnitCircle * 130f);
            copy.SetActive(false);
        }
    }

    void Update()
    {
        if (boolStart)
        {
            timer += Time.deltaTime;

            if (timer < 0.5f)
            {
                for (int i = 0; i < size; i++)
                {
                    LerpTo(rect[i], toGo_pos, count);
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    LerpTo(rect[i], toGo_pos, count);
                }
            }
        }
    }

    void LerpTo(RectTransform rt, Vector2 pos, int followerCount)
    {
        rt.position = Vector2.Lerp(rt.position, pos, speed * Time.deltaTime * 10f);
        if (Vector2.Distance(rt.position, toGo_pos) <= 5f)
        {
            rt.position = Vector2.zero + offset;
            rt.gameObject.SetActive(false);
            counter++;
            if (counter >= size)
            {
                timer = 0f;
                counter = 0;
                boolStart = false;
            }
        }
    }

    public void ActivateDeactivate(bool b, Vector3 worldPos, int foo)
    {
        textMesh.text = (Math.Round(float.Parse(textMesh.text) + money, 2)).ToString();
        PlayerPrefs.SetFloat("Money", float.Parse(textMesh.text));
        pos = Camera.main.WorldToScreenPoint(worldPos);
        for (int i = 0; i < pool.Count; i++)
        {
            pool[i].SetActive(b);
            rect[i].position = pos;
            randomPos[i] = Random.insideUnitCircle * 130f;
        }

        boolStart = true;
    }
}