using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGMDestroy : MonoBehaviour
{
    private void Start()
    {
        if(!GetComponent<AudioSource>().loop)
        {
            Destroy(gameObject, GetComponent<AudioSource>().clip.length);// After can add ñondition for looping audio
        }
    }
}
