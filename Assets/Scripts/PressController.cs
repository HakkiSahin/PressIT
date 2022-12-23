using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;

public class PressController : MonoBehaviour
{
    Vector3 startPos, goPos;
    public bool isTouch;
    public float speed;
    public float maxPressCount = 10f, pressCount;
    [SerializeField] GameObject effects;
    public Material mat;
    public GameObject headPress, explosion, failExplosion;
    Color defaultColor;
    public List<Material> colors;

    [SerializeField] private GameObject _jelly;
    [SerializeField] private Transform _jellySpawn;
    [SerializeField] private GameObject jellyParent;

    [SerializeField] private List<GameObject> presses;
    [SerializeField] GameObject upgradePanel;

    bool isResting;
    float restingTime = 0f;
    float colorSize;

    bool botSide = false;
    private bool isPressed = false;
    [SerializeField] ControlType controlType;

    [SerializeField] List<float> maxPosY;

    private void Start()
    {
        colorSize = maxPressCount * 10f;
        mat.color = Color.grey;

        startPos = this.transform.position;
        goPos = new Vector3(startPos.x,
            maxPosY[PlayerPrefs.HasKey("ActivePress") ? PlayerPrefs.GetInt("ActivePress") : 0], startPos.z);
        speed = PlayerPrefs.HasKey("PressPower") ? PlayerPrefs.GetFloat("PressPower") : 5;


        this.transform.GetChild(PlayerPrefs.GetInt("ActivePress") + 1).GetComponent<Rigidbody>().mass =
            (1 + (speed - 5) * 5) / PlayerPrefs.GetInt("ActivePress") + 1;

        maxPressCount = 3.1f + ((speed - 5f) / 0.1f);
        Debug.Log(speed);
        defaultColor = mat.color;

        headPress = presses[PlayerPrefs.HasKey("ActivePress") ? PlayerPrefs.GetInt("ActivePress") : 0];
    }


    // Default Variables 

    void Update()
    {
        ScreenShot();
        switch (controlType)
        {
            case ControlType.Hold:
                if (Input.GetMouseButton(0) &&
                    this.transform.position.y >
                    maxPosY[PlayerPrefs.HasKey("ActivePress") ? PlayerPrefs.GetInt("ActivePress") : 0] && !isTouch)
                {
                    this.transform.Translate(Vector3.down * Time.deltaTime * speed, Space.World);
                    ChangeColor(colors[0].color);
                }
                else if (this.transform.position.y < -2f)
                {
                    explosion.SetActive(true);
                    StartCoroutine(RepeatLerp(this.transform.position, startPos, 1));
                }

                if (Input.GetMouseButtonUp(0))
                {
                    StartCoroutine(RepeatLerp(this.transform.position, startPos, 1));
                }


                break;
            case ControlType.Touch:

                if (Input.GetMouseButtonDown(0) && this.transform.position.y >
                    maxPosY[PlayerPrefs.HasKey("ActivePress") ? PlayerPrefs.GetInt("ActivePress") : 0] && !isPressed)
                {
                    pressCount++;
                    StartCoroutine(RepeatLerp_Touch(this.transform.position, goPos, 0.2f, true));
                }
                //else if (Input.GetMouseButtonUp(0) && this.transform.position.y < startPos.y)
                //{
                //    StartCoroutine(RepeatLerp(this.transform.position, startPos, 1));

                //}
                ChangeColor(colors[0].color);
                if (this.transform.position.y < -1f)
                {
                    // explosion.SetActive(true);
                }

                break;
            default:
                break;
        }


        if (Input.GetMouseButtonDown(0))
        {
            isResting = false;
            restingTime = 0f;
            if (pressCount > maxPressCount)
            {
                headPress.GetComponent<Rigidbody>().useGravity = true;
                headPress.GetComponent<Rigidbody>().isKinematic = false;
                failExplosion.SetActive(true);
                Invoke("OpenFailMenu", 2f);
            }
        } // Control MaxPressCount

        if (isResting)
        {
            restingTime += Time.deltaTime * 1f;

            if (restingTime >= 1f)
            {
                mat.color = Color.Lerp(mat.color, defaultColor, Time.deltaTime * 5f);
                if (mat.color == defaultColor)
                {
                    if (pressCount >= maxPressCount / 2)
                    {
                        maxPressCount = maxPressCount / 2;
                        Debug.Log("maxPress : " + maxPressCount);
                        pressCount = 0;
                        effects.SetActive(false);
                    }
                }
            }
        } // Back and Restore Press
    }

    public void CreateJellyEffect()
    {
        Instantiate(_jelly, _jellySpawn.position, new Quaternion(0f, 0f, 0, 0), jellyParent.transform);
    }

    public void FailCrash()
    {
        Debug.Log("Fail Before :" + pressCount);
        //maxPressCount = maxPressCount * 0.6f;
        pressCount += maxPressCount * 0.15f;
        Debug.Log("Fail after :" + pressCount);
    } // Rare Item Crash

    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * 3;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            this.transform.position = Vector3.Lerp(a, b, i);
            yield return null;
        }

        isResting = true;
        isTouch = false;
        isPressed = false;
        explosion.SetActive(false);
    } // Move To start Pos

    public IEnumerator RepeatLerp_Touch(Vector3 a, Vector3 b, float time, bool isTouch)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * 3;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            this.transform.position = Vector3.Lerp(a, b, i);
            yield return null;
        }

        explosion.SetActive(false);
        if (isTouch == true)
        {
            StartCoroutine(WaitESecond());
        }
    } // Touch Move

    int currentSS = 0;

    void ScreenShot()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ScreenCapture.CaptureScreenshot("game" + PlayerPrefs.GetInt("s") + ".png");
            PlayerPrefs.SetInt("s", PlayerPrefs.GetInt("s") + 1);
            Time.timeScale = 0.001f;
            Debug.Log("Selection");
        }

        if (Input.GetKeyDown(KeyCode.A)) Time.timeScale = 1;
    }

    void OpenFailMenu()
    {
        AdsClass.Instance().EndGame();
        SceneManager.LoadScene(1);
    }

    private void OnTriggerStay(Collider other)
    {
        explosion.SetActive(false);
        ;
    }

    public MMFeedbacks feedbacks;

    void ChangeColor(Color color)
    {
        if (pressCount >= maxPressCount * 0.7f)
        {
            effects.SetActive(true);
        }

        if (pressCount >= maxPressCount * 0.9f) mat.color = Color.Lerp(mat.color, colors[3].color, Time.deltaTime);
        else if (pressCount >= maxPressCount * 0.8f)
        {
            mat.color = Color.Lerp(mat.color, colors[2].color, Time.deltaTime);
        }
        else if (pressCount >= maxPressCount * 0.75f)
            mat.color = Color.Lerp(mat.color, colors[1].color, Time.deltaTime);
        else if (pressCount >= maxPressCount * 0.7f) mat.color = Color.Lerp(mat.color, colors[0].color, Time.deltaTime);

        CheckedMaxPress();
    } // Color Change Areaq

    void CheckedMaxPress()
    {
        if (maxPressCount <= 3)
        {
            if (pressCount >= maxPressCount * 0.5f) mat.color = Color.Lerp(mat.color, colors[3].color, Time.deltaTime);
            else if (pressCount >= maxPressCount * 0.3f)
                mat.color = Color.Lerp(mat.color, colors[1].color, Time.deltaTime);
        }
        else if (maxPressCount <= 5)
        {
            if (pressCount >= maxPressCount * 0.8f) mat.color = Color.Lerp(mat.color, colors[3].color, Time.deltaTime);
            else if (pressCount >= maxPressCount * 0.4f)
                mat.color = Color.Lerp(mat.color, colors[1].color, Time.deltaTime);
        }
    } // Final Color Test

    IEnumerator WaitESecond()
    {
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(RepeatLerp(this.transform.position, startPos, 1));
    }
}

public enum ControlType
{
    Hold,
    Touch
}