using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour{

    public Transform main;
    public Transform levelSelect;


    private void Start(){
        main.gameObject.SetActive(true);
        levelSelect.gameObject.SetActive(false);
    }

    public void SceneLoader(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);

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
