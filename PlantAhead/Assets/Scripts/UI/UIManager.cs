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
    
    private IEnumerator displayBadAction(float time = 1f) 
    {
        badActionIcon.SetActive(true);
        yield return new WaitForSeconds(time);
        badActionIcon.SetActive(false);
    }

}