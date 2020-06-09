using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AutoRead : MonoBehaviour{

    public float delay = 0.1f;

    public string fullText;

    private string currentText;
    
    public ScrollRect scrollRect;

    public bool realIntro;
    
    public int SceneIndex;

    private string endText = "\n\n\nPress Space when you are ready to start";
    
    private string bodyIntro = "\nWelcome New Farmer" +
                               "\n\nWe are hoping you can prove your farm is worth running. To do that we want you to meet" +
                               "some production expectations. Plant and harvest crops and if you can meet our " +
                               "expectations you will advance to getting an even nicer farm." +
                               "\n\nTo add further restrictions you have a set amount of energy per day, that will " +
                               "reset when you go to sleep. Planting and harvesting plants will decrease this energy." +
                               "\n\nTo move around your farm use WASD as controls" +
                               "\n\nYou can access the menu, your inventory, the store, and your goal list via buttons " +
                               "on the left of the screen, or by entering 1-4 respectively." +
                               "\n\nWhen you go into the store you can purchase plants, and then from your inventory, " +
                               "you can equip those plants to be planted." +
                               "\n\nThe dirt chunks across the map are where you can plant your plants" +
                               "\n\nTo plant you first must Press R on the location to till the land, plants are then " +
                               "planted by press the space bar" +
                               "\n\nAfter being planted your plants will need to be watered by pressing the F button, " +
                               "this will also double as the button to press to refill your water can, which you can" +
                               " keep track of on the bottom right of the screen." +
                               "\n\nYour plants will sparkle to let you know they have been watered, and darken in color" +
                               " if they are dying." +
                               "\n ";
    
    // Start is called before the first frame update
    void Start(){
        if (realIntro){
            StartCoroutine(Intro());
        }
        else{
            StartCoroutine(ShowText());
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            SceneManager.LoadScene(SceneIndex);
            
        }
    }

    IEnumerator Intro(){
        string tempText = bodyIntro + endText;
        for (int i = 0; i <= tempText.Length; i++){
            currentText = tempText.Substring(0, i);
            this.GetComponent<TMP_Text>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }
    
    IEnumerator ShowText(){
        string tempText = fullText + endText;
        for (int i = 0; i <= tempText.Length; i++){
            currentText = tempText.Substring(0, i);
            this.GetComponent<TMP_Text>().text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }
}
