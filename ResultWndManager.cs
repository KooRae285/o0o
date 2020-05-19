using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultWndManager : MonoBehaviour
{
    [SerializeField] Text Kill;
    [SerializeField] Text Time;
    [SerializeField] Text Clear;

    // Start is called before the first frame update
    void Start()
    {
        SetResultWnd();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetResultWnd()
    {
        Kill.text = (InGameManager._instance.countKill.ToString() + "마리 죽임");
        Time.text = (InGameManager._instance.TimePlay.ToString() + "초 만큼 걸림");
        if(InGameManager._instance.Clearfail)
        {
            Clear.text = ("Clear !!");
        }
        else
        {
            Clear.text = ("Fail....");
        }
    }
    public void ClickLobbyButton()
    {
        BaseControll._instance.StartLobbyScene("Stage" + BaseControll._instance.stagecurrent.ToString());
        Destroy(gameObject);
    }
}
