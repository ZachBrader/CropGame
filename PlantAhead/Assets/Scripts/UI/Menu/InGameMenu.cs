using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    protected bool isOpen;
    public GameObject inGameMenuParent;
    
    // Start is called before the first frame update
    void Start()
    {
        isOpen = inGameMenuParent.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool checkOpen()
    {
        return isOpen;
    }

    public void toggleDisplay()
    {
        if (isOpen) // Close Display
        {
            closeDisplay();
        }
        else // Open Display
        {
            showDisplay();
        }
    }

    public void showDisplay()
    {

        inGameMenuParent.SetActive(true);
        isOpen = true;
    }

    public void closeDisplay()
    {
        inGameMenuParent.SetActive(false);
        isOpen = false;
    }

    public void SceneLoader(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void ReloadScene()
    {
        Debug.Log("Reloading " + SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ActivateObject(GameObject objectToActivate)
    {
        objectToActivate.SetActive(true);
    }

    public void DeactivateObject(GameObject objectToDeactivate)
    {
        objectToDeactivate.SetActive(false);
    }
}
