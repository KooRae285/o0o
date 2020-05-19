using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCamera : MonoBehaviour
{
    public enum eStateCameraAct
    {
        Loading = 0,
        none,
        WALKKING,
        CHANGE_FOLLOW,
        WAIT,
        FOLLOW
    }
    [SerializeField] float _movSpeed = 15;
    [SerializeField] Vector3 _followOffset = new Vector3( 0,1.8f,+2);

    Transform _tfRootPos;
    List<Vector3> _ltPositions;
    int _currentIndex;
    int _nextCameraPos;
    eStateCameraAct _stateAction;
    float _timeCheck = 0;
    Transform Player;
    Transform _lookPos;
    Vector3 _posGoal;

    public eStateCameraAct _stateCameraAction
    {
        set
        {
            _stateAction = value;
        }
    }
    private void Awake()
    {
        _ltPositions = new List<Vector3>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (_stateAction)
        {
            case eStateCameraAct.none:
                _timeCheck += Time.deltaTime;
                if (_timeCheck >= 2)
                    _stateAction = eStateCameraAct.WALKKING;
                break;
            case eStateCameraAct.WALKKING:
                if (Vector3.Distance(transform.position, _ltPositions[_nextCameraPos]) <= 0.05f)
                {
                    //Debug.Log("하하");
                    _currentIndex = _nextCameraPos;
                    _nextCameraPos = _currentIndex + 1;
                    if (_nextCameraPos >= _ltPositions.Count)
                    {
                        _nextCameraPos = _currentIndex;
                        _stateAction = eStateCameraAct.CHANGE_FOLLOW;
                        Player = GameObject.FindGameObjectWithTag("Player").transform;
                        _lookPos = GameObject.FindGameObjectWithTag("LookPos").transform;
                    }
                }
                transform.position = Vector3.MoveTowards(transform.position, _ltPositions[_nextCameraPos], Time.deltaTime * _movSpeed);
                transform.LookAt(_tfRootPos);
                break;
            case eStateCameraAct.CHANGE_FOLLOW:
                Vector3 tp = (Player.transform.position) + _followOffset;
                Quaternion tq = Quaternion.LookRotation(_lookPos.position - tp);
                transform.position = Vector3.Slerp(transform.position, tp, Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, tq, Time.deltaTime);
                //transform.LookAt(_lookPos);
                if (Vector3.Distance(transform.position, tp) <= 0.3f)
                {
                    transform.position = tp;
                    transform.LookAt(_lookPos);
                    _stateAction = eStateCameraAct.WAIT;
                    InGameManager._instance.GameStart();
                }
                break;
            case eStateCameraAct.FOLLOW:
                float currAngleY = Mathf.LerpAngle(transform.eulerAngles.y, Player.eulerAngles.y, Time.deltaTime*5);
                Quaternion rot = Quaternion.Euler(0, currAngleY, 0);
                _posGoal = Player.position - (rot * Vector3.forward * _followOffset.z) + (Vector3.up * _followOffset.y);
                transform.position =_posGoal;//Vector3.MoveTowards(transform.position, _posGoal, Time.deltaTime*5);
                transform.LookAt(_lookPos);
                break;
        }
    
    }
    public void SetCameraActionPosRoot(Transform tr)
    {
        _currentIndex = 0;
        _tfRootPos = tr;
        for (int n = 0; n < _tfRootPos.childCount; n++)
        {
            _ltPositions.Add(_tfRootPos.GetChild(n).position);
        }
        transform.position = _ltPositions[_currentIndex];
        transform.LookAt(_tfRootPos);
        _nextCameraPos = _currentIndex + 1;
        _stateAction = eStateCameraAct.none;
    }
}
    
