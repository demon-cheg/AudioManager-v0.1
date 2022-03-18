using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private string          _pathToSoundFolder = "Sounds/";
    private const string    _pathToPrefabsFolder = "Prefabs/AudioManagerPrefabs/";

    private GameObject[]    _prefabsLoaded = new GameObject[0];

    public AudioSource Play(string name)
    {
        GameObject gm = null;
        foreach (var item in _prefabsLoaded)
        {
            if(item.name == name)
            {
                gm = Instantiate(item);
                gm.AddComponent<AudioGMDestroy>();
            }
        }
        return gm.GetComponent<AudioSource>();
    }
    public AudioSource Play(string name , Vector3 pos)
    {
        GameObject gm = null;
        foreach (var item in _prefabsLoaded)
        {
            if (item.name == name)
            {
                gm = Instantiate(item, pos, Quaternion.identity);
                gm.AddComponent<AudioGMDestroy>();
            }
        }
        return gm.GetComponent<AudioSource>();
    }
    private void Start()
    {
        Instance = this;
        LoadPrefabs();
    }
    private void LoadPrefabs()
    {
        _prefabsLoaded = Resources.LoadAll<GameObject>(_pathToPrefabsFolder);
    }
    
}
