using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ONLY ONE PER SCENE
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Image waterBar;
    public Image EnergyBar;

    public GameObject notificationPosition;
    public GameObject notificationPrefab;
    public GameObject badActionIcon;

    public GameObject Heart;

    public Sprite HeartM;
    public Sprite HeartL;
    public TMP_Text ActionStatus;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Pls work");
        Instance = this;
        ActionStatus.text = "";
    }

    public void displayX(float time = 1f)
    {
        StartCoroutine(displayBadAction(time));
    }
    
    public void DisplayHearts(float time = 0.25f)
    {
        StartCoroutine(displayHeartAction(time));    
        
    }
    
    private IEnumerator displayHeartAction(float time)
    {
        Heart.SetActive(true);
        Heart.GetComponent<SpriteRenderer>().sprite = HeartM;
        yield return new WaitForSeconds(time);
        Heart.GetComponent<SpriteRenderer>().sprite = HeartL;
        yield return new WaitForSeconds(time);
        Heart.GetComponent<SpriteRenderer>().sprite = HeartM;
        Heart.SetActive(false);
    }

    private IEnumerator displayBadAction(float time = 1f) 
    {
        badActionIcon.SetActive(true);
        yield return new WaitForSeconds(time);
        badActionIcon.SetActive(false);
        yield return new WaitForSeconds(time * 3);
        ActionStatus.text = "";
    }

    public void SendNotification(string message)
    {
        StartCoroutine(Notification(message, new Vector3(100, 100, 0)));
    }

    IEnumerator Notification(string message, Vector3 NotificationLocation)
    {
        GameObject notificationBackground = Instantiate(notificationPrefab);
        notificationBackground.transform.SetParent(notificationPosition.transform);
        notificationBackground.transform.localPosition = NotificationLocation;

        TMP_Text notificationText = notificationBackground.transform.Find("NotificationText").GetComponent<TMP_Text>();
        notificationText.text = message;
        CanvasRenderer backgroundRender = notificationBackground.GetComponent<CanvasRenderer>();
        CanvasRenderer textRender = notificationText.GetComponent<CanvasRenderer>();

        yield return new WaitForSeconds(1);

        while (backgroundRender.GetAlpha() > 0)
        {
            backgroundRender.SetAlpha(backgroundRender.GetAlpha() - (float)(Time.deltaTime) / .5f);
            textRender.SetAlpha(textRender.GetAlpha() - (float)(Time.deltaTime));
            yield return null;
        }
        Destroy(notificationText);
        Destroy(notificationBackground);
        yield return null;
    }

}