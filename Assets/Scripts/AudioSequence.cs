using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSequence : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip BGM_Intro;
    public AudioClip BGM_GhostsNormal;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayIntroThenNormal());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private System.Collections.IEnumerator PlayIntroThenNormal()
    {
        audioSource.clip = BGM_Intro;
        audioSource.loop = false;
        audioSource.Play();
        yield return new WaitForSeconds(3f);
        audioSource.clip = BGM_GhostsNormal;
        audioSource.loop = true;
        audioSource.Play();
    }
}
