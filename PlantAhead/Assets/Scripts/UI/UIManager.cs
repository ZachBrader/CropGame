using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ONLY ONE PER SCENE
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Image waterBar;
    public Image EnergyBar;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }


}