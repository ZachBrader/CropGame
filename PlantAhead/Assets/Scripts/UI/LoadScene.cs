using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour{

    public Transform main;
    public Transform levelSelect;
    public Transform instructions;


    private void Start(){
        main.gameObject.SetActive(true);
        levelSelect.gameObject.SetActive(false);
        instructions.gameObject.SetActive(false);
    }

    public void SceneLoader(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    public void TutorialToggle(){
        if (instructions.gameObject.activeInHierarchy == false)
        {
            instructions.gameObject.SetActive(true);
            main.gameObject.SetActive(false);
        }
        else
        {
            instructions.gameObject.SetActive(false);
            main.gameObject.SetActive(true);
        }
        
    }

    public void SubMenuToggle(){
        if (levelSelect.gameObject.activeInHierarchy == false)
        {
            levelSelect.gameObject.SetActive(true);
            main.gameObject.SetActive(false);
        }
        else
        {
            levelSelect.gameObject.SetActive(false);
            main.gameObject.SetActive(true);
        }
    }
}
