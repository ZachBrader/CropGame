using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject dayTrackerText;

    // Keeps track of the day the player is currently on
    private int curDay;

    // When the player needs to meet their goal
    public int finalDate;



    // Start is called before the first frame update
    void Start()
    { 
        dayTrackerText.GetComponent<TMP_Text>().text = "Date: " + curDay + " / " + finalDate;
    }

    // Update is called once per frame
    void Update()
    {
        // Able to exit application by pressing escape button
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void EndDay()
    {
        curDay++;

        dayTrackerText.GetComponent<TMP_Text>().text = "Date: " + curDay + " / " + finalDate;

        if (curDay == finalDate)
        {
            Debug.Log("Level Completed");
        }
    }
}
