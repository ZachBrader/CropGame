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

    public GameObject badActionIcon;

    public GameObject Heart;

    public Sprite HeartM;
    public Sprite HeartL;
    public TMP_Text ActionStatus;
    // Start is called before the first frame update
    void Start()
    {
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

}