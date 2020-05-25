using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour{
    
    private AudioSource source;

    [SerializeField]
    private AudioClip waterSound;

    [SerializeField] 
    private AudioClip plantingSound;

    [SerializeField] 
    private AudioClip walkingSound;

    [SerializeField] 
    private AudioClip coinSound; 
    // Start is called before the first frame update
    void Start(){
        source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buySound(){
        source.PlayOneShot(coinSound);
    }
}
