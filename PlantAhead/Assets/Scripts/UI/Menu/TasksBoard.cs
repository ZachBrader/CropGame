using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TasksBoard : InGameMenu
{
    private bool isReady = false;
    private TMP_Text LevelIndicatorText;

    private TMP_Text PlayerGoalText;
    private TMP_Text PlayerCurrentGoldText;

    private TMP_Text LevelFinalDateText;
    private TMP_Text CurrentDateText;

    private GameObject player;
    private Inventory playerInventory;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        showDisplay();
        LevelIndicatorText = inGameMenuParent.transform.Find("LevelIndicatorText").GetComponent<TMP_Text>();

        PlayerGoalText = inGameMenuParent.transform.Find("PlayerGoalText").GetComponent<TMP_Text>();
        PlayerCurrentGoldText = inGameMenuParent.transform.Find("PlayerCurrentGoldText").GetComponent<TMP_Text>();
        LevelFinalDateText = inGameMenuParent.transform.Find("LevelFinalDateText").GetComponent<TMP_Text>();
        CurrentDateText = inGameMenuParent.transform.Find("CurrentDateText").GetComponent<TMP_Text>();


        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<Inventory>();

        closeDisplay();
        isOpen = inGameMenuParent.activeSelf;
        isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen && isReady)
        {
            UpdateVisuals();
        }
    }

    private void UpdateVisuals()
    {
        LevelIndicatorText.text = "Level 1";

        PlayerGoalText.text = gameManager.playerGoldGoal.ToString();
        PlayerCurrentGoldText.text = playerInventory.GetGold().ToString();

        LevelFinalDateText.text = "Day " + gameManager.finalDate.ToString();
        CurrentDateText.text = "Day " + gameManager.curDay.ToString();

    }
}
