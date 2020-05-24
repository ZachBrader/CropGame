using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGameMenu : InGameMenu
{
    public TMP_Text victoryText;
    public TMP_Text goldText;
    public TMP_Text dateText;
    public GameObject nextLevelButton;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = inGameMenuParent.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setVictoryText(string textToWrite)
    {
        if (isOpen)
        {
            victoryText.text = textToWrite;
        }
    }

    public void setGoldText(string textToWrite)
    {
        if (isOpen)
        {
            goldText.text = textToWrite;
        }
    }

    public void setDateText(string textToWrite)
    {
        if (isOpen)
        {
            dateText.text = textToWrite;
        }
    }
}
