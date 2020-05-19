using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public enum eGameState
    {
        READY = 0,
        MAPSETTING,
        CAMERAWARKING,
        START,
        PLAY,
        END,
        RESULT
    }
    [SerializeField] GameObject _InGameUIFrame;
    [SerializeField] Text _txtInfo;
    [SerializeField] GameObject _BgInfo;
    [SerializeField] GameObject _prefabPlayer;
    [SerializeField] Text _NameTxt;
    [SerializeField] Slider _HPbar;
    [SerializeField] GameObject _resultWnd;
    GameObject _posPlayerStart;
    eGameState _currentState;
    SpawnControl[] _ctrlSpawn;
    bool _isSpawn;
    bool _isClear = false;
    PlayerController _player;
    int _maxMonsterCount;
    float _timeCheck;
    float _playTime = 0;
    int _killCount = 0;

    static InGameManager _uniqueinstance;
    public static InGameManager _instance
    {
        get
        {
            return _uniqueinstance;
        }
    }
    public eGameState _nowGameState
    {
        get
        {
            return _currentState;
        }
        set
        {
            _currentState = value;
        }
        
    }
    public int countKill
    {
        get
        {
            return _killCount;
        }
        set
        {
            _killCount = value;
        }
    }
    public int TimePlay
    {
        get
        {
            return (int)_playTime;
        }
    }
    public bool Clearfail
    {
        get
        {
            return _isClear;
        }
    }
    public bool _EnableSpawn
    {
        get { return _isSpawn; }
    }
    void Awake()
    {
        _uniqueinstance = this;
        _isSpawn = false;
        _InGameUIFrame.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        //임시
        _maxMonsterCount = 30;
        GameReady();
    }
    // Update is called once per frame
    void Update()
    {
       switch(_currentState)
       {
            case eGameState.START:
                _timeCheck += Time.deltaTime;
                if(_timeCheck >= 1.2f)
                {
                    Camera.main.GetComponent<ActionCamera>()._stateCameraAction = ActionCamera.eStateCameraAct.FOLLOW;
                    _timeCheck = 0;
                    GamePlay();
                }
                break;
            case eGameState.PLAY:
                _HPbar.value = _player._nowHP;
                _playTime += Time.deltaTime;
                if(_killCount>=10)
                {
                    _isClear = true;
                    GameEnd();
                }
                break;
            case eGameState.END:
                _timeCheck += Time.deltaTime;
                if(_timeCheck >= 3.0f)
                {
                    GameResult();
                }
                break;

       }
    }
    void LateUpdate()
    {
        
    }
    public void GameReady()
    {
        _currentState = eGameState.READY;
        // 문구 출력 후 대기(맵 로드가 끝날 때 까지).
        _txtInfo.text = "READY!!";
    }
    public void GameMapSetting()
    {
        _currentState = eGameState.MAPSETTING;
        
        //카메라 워킹 할 위치들 설정.
        Transform tf = GameObject.FindGameObjectWithTag("CameraPosRoot").transform;
        Camera.main.GetComponent<ActionCamera>().SetCameraActionPosRoot(tf);
        _InGameUIFrame.SetActive(true);
        // 스폰 포인트 활성화.
        _ctrlSpawn = FindObjectsOfType<SpawnControl>();
        _isSpawn = true;
        // 플레이어 생성.
        SettingPlayer();
        GameObject go = Instantiate(_prefabPlayer, _posPlayerStart.transform.position, _posPlayerStart.transform.rotation);
        go.transform.SetParent(GameObject.Find("Player").transform);
        _player = go.GetComponent<PlayerController>();
        _player.InitStatData("qwerty", 5, 1, 15);

        GameCameraWark();
    }
    public void GameCameraWark()
    {
        _currentState = eGameState.CAMERAWARKING;
        //카메라를 지정한 위치로 움직이고, 마지막엔 플레이어 뒤로 오도록 한다.
    }
    public void GameStart()
    {
        _HPbar.maxValue = _player._FullHP;
        _NameTxt.text = _player.GetName;
        _currentState = eGameState.START;
        _txtInfo.text = "GAME START!";
        _timeCheck = 0;
        
    }
    public void GamePlay()
    {
        _currentState = eGameState.PLAY;
        _BgInfo.SetActive(false);

    }
    public void GameEnd()
    {
        _currentState = eGameState.END;
        _HPbar.value = _player._nowHP;
        _BgInfo.SetActive(true);
        _txtInfo.text = "GameEnd";
    }
    public void GameResult()
    {
        _currentState = eGameState.RESULT;
        _BgInfo.SetActive(false);
        _InGameUIFrame.SetActive(false);
        ResultWindow();
        if (_isClear)
        {
            if ((LobbyManager._instance.StageNum) <= BaseControll._instance.PPclear.Count)
            {
                BaseControll._instance.PPclear[(LobbyManager._instance.StageNum)] = true;
            }
        }
    }
    public void SettingPlayer()
    {
        _posPlayerStart = GameObject.FindGameObjectWithTag("StartingPosition");
    }
    public void CheckCountMonster()
    {
        int tCount = 0;
        for(int n=0; n<_ctrlSpawn.Length; n++)
        {
            tCount += _ctrlSpawn[n]._curLiveMonsterCount;
        }

        if (tCount >= _maxMonsterCount)
        {
            _isSpawn = false;
        }
        else
            _isSpawn = true;
    }
    public void ClickFireButton()
    {
        if(_player != null && _currentState == eGameState.PLAY)
        {
            _player.ClickAttack();
        }
    }
    public void ClickBulletButton()
    {
        if (_player != null && _currentState == eGameState.PLAY)
        {
            _player.ClickAttack2();
        }
    }
    public void ResultWindow()
    {
        GameObject go = Instantiate(_resultWnd);
        
    }

}
