using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class AudioManagerEditor : EditorWindow
{
    public static AudioManagerEditor Instance { get; private set; }
    private string          _pathToSoundFolder   = "Sounds/";
    private const string    _pathToPrefabsFolder = "Prefabs/AudioManagerPrefabs/";


    
    private List<PrefabButton>  _prefabButtons = new List<PrefabButton>();
    private GameObject[]        _prefabsLoaded = new GameObject [0];
    private AudioClip[]         _audioClips = new AudioClip[0];

    private Vector2 _scrollView;

    [MenuItem("Tools/Audio Manager")]
    private static void ShowWindow()
    {
        Instance = (AudioManagerEditor)GetWindow(typeof(AudioManagerEditor), true, "Audio Manager", true);
    }

    private void OnGUI()
    {
        
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        _pathToSoundFolder = EditorGUILayout.TextField("Folder with sounds ",_pathToSoundFolder);
        
        
        GUILayout.Space(70);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh"))
        {
            Refresh();
        }
        if (Selection.activeGameObject != null)
        {
            if (GUILayout.Button("Dublicate"))
            {
                Duplicate();
            }
            if (GUILayout.Button("Delete"))
            {
                Delete();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(30);
        _scrollView = GUILayout.BeginScrollView(_scrollView, GUILayout.Height(150));
        InitPrefabs();
        GUILayout.EndScrollView();
    }
    private void Delete()
    {
        Debug.Log(Selection.activeGameObject.name);
        
        foreach (var item in _prefabButtons)
        {
            
            if (item._gm.name == Selection.activeGameObject.name)
            {
                Debug.Log(item._gm.name);
                
                List<GameObject> prefabsLoaded = new List<GameObject>(_prefabsLoaded);
                foreach (var _item in prefabsLoaded)
                {
                    if (_item.name == Selection.activeGameObject.name)
                    {
                        prefabsLoaded.Remove(_item);
                        AssetDatabase.DeleteAsset("Assets/Resources/" + _pathToPrefabsFolder + Selection.activeGameObject.name + ".prefab");
                        _prefabButtons.Remove(item);
                        break;
                        //Debug.Log(item._gm.name);

                    }
                }
                _prefabsLoaded = prefabsLoaded.ToArray();
                break;
                //Debug.Log(item._gm.name);

            }
        }
        Selection.activeGameObject = null;
        Refresh();
    }
    private void Duplicate()
    {
        GameObject newGm = Instantiate(Selection.activeGameObject);
        CreatePrefab("Assets/Resources/" + _pathToPrefabsFolder + newGm.name + ".prefab", newGm);
        Refresh();
        DestroyImmediate(newGm);
        
    }
    private void InitPrefabs()
    {
        _prefabButtons.Clear();
        //SerializedObject so = new SerializedObject(this);
        //SerializedProperty stringsProperty = so.FindProperty("prefabsLoaded");
        //Color color_default = GUI.backgroundColor;
        //var p_list = so.FindProperty("prefabsLoaded");

        for (int i = 0; i < _prefabsLoaded.Length; i++)
        {
            CreateButtonPrefab(_prefabsLoaded[i]);
        }
        ShowButtonPrefabs();
    }
    private void CreateButtonPrefab(GameObject gm)
    {
         _prefabButtons.Add(new PrefabButton(gm));
    }
    private void ShowButtonPrefabs()
    {
        foreach (var item in _prefabButtons)
        {
            if(GUILayout.Button(item._gm.name))
            {
                Selection.activeGameObject = item._gm;
            }
        }
    }
    private void Refresh()
    {
        LoadSounds();
        LoadPrefabs();
        
        Debug.Log("refresh");
    }
    private void LoadPrefabs()
    {
        _prefabsLoaded = Resources.LoadAll<GameObject>(_pathToPrefabsFolder);
        foreach (var item in _prefabsLoaded)
        {
            Debug.Log(item.name);
        }
    }
    private void SavePrefabs()
    {
        foreach (var item in _prefabsLoaded)
        {
            CreatePrefab("Assets/Resources/" + _pathToPrefabsFolder + item.name + ".prefab", item);
        }
    }
    private void CheckExistExistOrCreatePrefab(string path , AudioClip audioClip)
    {
        if (File.Exists(path)) return;
        CreatePrefab(path, audioClip);
    }
    private void CreatePrefab(string path , AudioClip audioClip)
    {
        GameObject gm = new GameObject(audioClip.name);
        gm.AddComponent<AudioSource>().clip = audioClip;
        PrefabUtility.SaveAsPrefabAsset(gm,path);
        Destroy(gm);
        Debug.Log("path" + " is resaved");
    }
    private void CreatePrefab(string path, GameObject gm)
    {
        PrefabUtility.SaveAsPrefabAsset(gm, path);
        Debug.Log("path" + " is resaved");
    }
    private void LoadSounds()
    {
        //CheckExistOrCreateSoundsFolder("Resources/" + pathToSoundFolder);
        _audioClips = Resources.LoadAll<AudioClip>(_pathToSoundFolder);
        
        foreach (var item in _audioClips)
        {
            CheckExistExistOrCreatePrefab("Assets/Resources/" + _pathToPrefabsFolder + item.name + ".prefab",item);
        }
    }
    //private bool CheckExistOrCreateSoundsFolder(string path)
    //{
    //    if (!Directory.Exists(path))
    //    {
    //        //if it doesn't, create it
    //        Directory.CreateDirectory(path);

    //    }
    //    return true;
    //}
    private class PrefabButton
    {
        public GameObject _gm { get; private set; }
        private GUILayout guiLayout;
        public PrefabButton(GameObject gm)
        {
            _gm = gm;
        } 
    }
}

