using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BaseControll : MonoBehaviour
{
    [SerializeField] GameObject LoadingWnd;

    public enum eSceneState
    {
        Lobby = 0,
        Loading,
        Play
    }
    int currentStage;
    eSceneState _currentScene;
    List<bool> ClearPP = new List<bool>();
    static BaseControll _uniqueinstance;
    public static BaseControll _instance
    {
        get
        {
            return _uniqueinstance;
        }
    }
    public int stagecurrent
    {
        get
        {
            return currentStage;
        }
        set
        {
            currentStage = value;
        }
    }
    public List<bool> PPclear
    {
        get
        {
            return ClearPP;
        }
        set
        {
            ClearPP = value;
        }
    }
    void Awake()
    {
        _uniqueinstance = this;
    }
    void Start()
    {
        StartCoroutine(GameStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartLobbyScene(string unloadScene = "")
    {
        _currentScene = eSceneState.Lobby;
        StartCoroutine(LoadingScene(unloadScene, "LobbyScene"));
    }
    public void StartPlayScene(string unloadScene = "")
    {
        currentStage = 1;
        _currentScene = eSceneState.Play;
        StartCoroutine(LoadingScene(unloadScene, "Stage1"));
    }
    public void StartPlayScene2(string unloadScene = "")
    {
        currentStage = 2;
        _currentScene = eSceneState.Play;
        StartCoroutine(LoadingScene(unloadScene, "Stage2"));
        
    }
    IEnumerator LoadingScene(string UnloadName, string LoadName)
    {
        GameObject go = Instantiate(LoadingWnd);
        LoadingManager Loadingwnd = go.transform.GetChild(0).GetComponent<LoadingManager>();

        AsyncOperation AO;
        if (UnloadName != string.Empty)
        {
            AO = SceneManager.UnloadSceneAsync(UnloadName);
            while (!AO.isDone)
            {
                Loadingwnd.SettingLoadingBar(0.33f);
                yield return new WaitForSeconds(2);
                yield return null;
            }
        }
        Loadingwnd.SettingLoadingBar(0.33f);
        if (_currentScene == eSceneState.Play)
        {
            AO = SceneManager.LoadSceneAsync("InGameScene", LoadSceneMode.Additive);

            while (!AO.isDone)
            {
                Loadingwnd.SettingLoadingBar(0.66f);
                yield return new WaitForSeconds(2);
                yield return null;
                
            }
            Loadingwnd.SettingLoadingBar(0.66f);
            AO = SceneManager.LoadSceneAsync(LoadName, LoadSceneMode.Additive);
            while (!AO.isDone)
            {
                Loadingwnd.SettingLoadingBar(1);
                yield return new WaitForSeconds(2);
                yield return null;
            }
            Loadingwnd.SettingLoadingBar(1);
            
            InGameManager._instance.GameMapSetting();
        }
        else
        {
            AO = SceneManager.UnloadSceneAsync("InGameScene");

            while (!AO.isDone)
            {
                Loadingwnd.SettingLoadingBar(0.66f);
                yield return null;
            }
            Loadingwnd.SettingLoadingBar(0.66f);
            AO = SceneManager.LoadSceneAsync(LoadName, LoadSceneMode.Additive);
            while (!AO.isDone)
            {
                Loadingwnd.SettingLoadingBar(1);
                yield return null;
            }
            Loadingwnd.SettingLoadingBar(1);
            yield return new WaitForSeconds(2);
            
        }
        Destroy(Loadingwnd.transform.parent.gameObject);
    }
    IEnumerator GameStart()
    {
        GameObject go = Instantiate(LoadingWnd);
        LoadingManager Loadingwnd = go.transform.GetChild(0).GetComponent<LoadingManager>();
        AsyncOperation AO;
        AO = SceneManager.LoadSceneAsync("LobbyScene", LoadSceneMode.Additive);

        while (!AO.isDone)
        {
            Loadingwnd.SettingLoadingBar(0.5f);
            yield return new WaitForSeconds(2);
            yield return null;

        }
        Loadingwnd.SettingLoadingBar(1);
        yield return new WaitForSeconds(2);
        Destroy(Loadingwnd.transform.parent.gameObject);
    }
}
