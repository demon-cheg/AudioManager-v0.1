using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    
    void Start()
    {
        Invoke("AutoPlay", 3);
    }
    private void AutoPlay()
    {
        Invoke("AutoPlay", 3);
        AudioManager.Instance.Play("RPG_fire");
    }
    
}
