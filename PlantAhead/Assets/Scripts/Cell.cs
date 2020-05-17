using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    #region Public Variables
    // Holds the crop object -- In progress
    [HideInInspector]
    public GameObject crop;

    // Position
    [HideInInspector]
    public Vector2 mFarmPosition = Vector2.zero;

    // Reference to board
    [HideInInspector]
    public Farm mFarm = null;

    // Reference to rect transform
    [HideInInspector]
    public RectTransform mRectTransform = null;
    #endregion

    public void Setup(Vector2 newFarmPosition, Farm newFarm)
    {
        mFarmPosition = newFarmPosition;
        mFarm = newFarm;
        mRectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
