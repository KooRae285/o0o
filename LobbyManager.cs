using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] Button[] StageButton;
    List<Button> AllStage;
    int CurrentStageNum;
    // Start is called before the first frame update
    static LobbyManager _uniqinstance;
    public static LobbyManager _instance
    {
        get
        {
            return _uniqinstance;
        }
    }
    public int StageNum
    {
        get
        {
            return CurrentStageNum;
        }
    }
    void Awake()
    {
        AllStage = new List<Button>();
        _uniqinstance = this;
        SetStageButton();
        AllStage[0].interactable = true;
        InteractableStageButton();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClickStage1()
    {
        BaseControll._instance.StartPlayScene("LobbyScene");
        CurrentStageNum = 1;
    }
    public void ClickStage2()
    {
        BaseControll._instance.StartPlayScene2("LobbyScene");
        CurrentStageNum = 2;
    }
    public void ClickStage3()
    {
        BaseControll._instance.StartPlayScene2("LobbyScene");
        CurrentStageNum = 3;
    }
    public void ClickStage4()
    {
        BaseControll._instance.StartPlayScene2("LobbyScene");
        CurrentStageNum = 4;
    }
    public void ClickStage5()
    {
        BaseControll._instance.StartPlayScene2("LobbyScene");
        CurrentStageNum = 5;
    }
    public void SetStageButton()
    {
        for(int i=0; i<StageButton.Length; i++)
        {
            AllStage.Add(StageButton[i]);
            BaseControll._instance.PPclear.Add(false);
            AllStage[i].interactable = false;
        }
    }
    public void InteractableStageButton()
    {
        for (int i = 1; i < AllStage.Count; i++)
        {
            AllStage[i].interactable = BaseControll._instance.PPclear[i];
        }
    }
}
