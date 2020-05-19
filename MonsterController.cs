using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterController : Status
{
    public enum ePersonality
    {
        DIRTY =0,    // 92% 이동.
        FIERY,       // 81%
        CRAZY,       // 75%
        LAZY,        // 15%
        MILD         // 34%
    }

    [SerializeField] Slider HPbar;
    [SerializeField] Text _MonsterNameInfo;
    [SerializeField] ePersonality _personal = ePersonality.MILD;
    [SerializeField] BoxCollider _collHitZone;
    [SerializeField] float _limitAreaX = 7;
    [SerializeField] float _limitAreaZ = 7;
    [SerializeField] float _minWaitTime = 3;
    [SerializeField] float _maxWaitTime = 8;
    [SerializeField] float _sightRange = 7;
    [SerializeField] float _attRange = 1.6f;

    string _Name;
    Animator Anictrl;
    PlayerController.eStateAction _curActState;
    NavMeshAgent _naviAgent;
    bool _isDead;
    Vector3 _posSpawn;
    Vector3 _posTarget;
    float _timeWait;
    bool _isSelectedAI;
    bool _isPP = false;
    float _activityRate;
    SpawnControl.eRoammingType _roamType;
    List<Vector3> _ltPoints;
    int _idxRoamming = 0;

    //임시
    Transform _tfPlayer;

    public void SettingPersonalityWithLate(ePersonality personality)
    {
        switch (personality)
        {
            case ePersonality.DIRTY:
                _activityRate = 92;
                break;
            case ePersonality.FIERY:
                _activityRate = 81;
                break;
            case ePersonality.CRAZY:
                _activityRate = 75;
                break;
            case ePersonality.LAZY:
                _activityRate = 15;
                break;
            case ePersonality.MILD:
                _activityRate = 34;
                break;
        }
    }
    void Awake()
    {
        
        Anictrl = GetComponent<Animator>();
        _naviAgent = GetComponent<NavMeshAgent>();
        _isDead = false;
        _isSelectedAI = false;
        _posTarget = _posSpawn = transform.position;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        _Name = "Badqwerty";
        InitData(5, 1, 10);
        DesableHitZone();
        //_tfPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log(_personal.ToString());
    }

    // Update is called once per frame
    
    void Update()
    {
        if (_isDead)
        {
            transform.position = transform.position;
           // HPbar.ac
            return;
        }
        if (_tfPlayer == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
                _tfPlayer = go.transform;

            switch (_curActState)
            {
                case PlayerController.eStateAction.IDLE:
                    _timeWait -= Time.deltaTime;
                    if (_timeWait <= 0)
                    {
                        _isSelectedAI = false;
                    }
                    break;
                //case PlayerController.eStateAction.WALK:
                //    if (Vector3.Distance(transform.position, _posTarget) <= 0.2f)
                //    {
                //        _isSelectedAI = false;
                //    }
                //    break;
            }
            
        }
        else
        {
            switch (_curActState)
            {
                case PlayerController.eStateAction.IDLE:
                    if (Vector3.Distance(transform.position, _tfPlayer.position) >= _sightRange)
                    {
                        _timeWait -= Time.deltaTime;
                        if (_timeWait <= 0)
                        {
                            _isSelectedAI = false;
                        }
                    }
                    else
                    {
                        ChangedAction(PlayerController.eStateAction.RUN);
                        _naviAgent.destination=_tfPlayer.position;
                    }
                    break;
                case PlayerController.eStateAction.WALK:
                    if (Vector3.Distance(transform.position, _tfPlayer.position) <= _sightRange)
                    //{
                    //    if (Vector3.Distance(transform.position, _posTarget) <= 0.2f)
                    //    {
                    //        _isSelectedAI = false;
                    //    }
                    //}
                    //else
                    {
                        ChangedAction(PlayerController.eStateAction.RUN);
                        _naviAgent.destination = _tfPlayer.position;
                    }
                    break;
                case PlayerController.eStateAction.RUN:
                    if(Vector3.Distance(transform.position,_tfPlayer.position)>=_attRange)
                    {
                        ChangedAction(PlayerController.eStateAction.RUN);
                        _naviAgent.destination = _tfPlayer.position;
                    }
                    else
                    {
                        ChangedAction(PlayerController.eStateAction.ATTACK);
                    }
                    break;
                case PlayerController.eStateAction.ATTACK:
                    if (Vector3.Distance(transform.position, _tfPlayer.position) >= _attRange)
                    {
                        ChangedAction(PlayerController.eStateAction.RUN);
                        _naviAgent.destination = _tfPlayer.position;
                    }
                    else
                    {
                        ChangedAction(PlayerController.eStateAction.ATTACK);
                        transform.LookAt(_tfPlayer);
                    }
                    break;
                case PlayerController.eStateAction.HIT:
                    if (Vector3.Distance(transform.position, _tfPlayer.position) >= _attRange)
                    {
                        ChangedAction(PlayerController.eStateAction.RUN);
                        _naviAgent.destination = _tfPlayer.position;
                    }
                    else
                    {
                        ChangedAction(PlayerController.eStateAction.ATTACK);
                        transform.LookAt(_tfPlayer);
                    }
                    break;
            }
        }
        ProcessAI();
        SetUIInfo();
        //_naviAgent.destination = _tfPlayer.position;
    }

    public void ProcessAI()
    {
        if (_isSelectedAI)
            return;
        int select = Random.Range(0, 100);
        

        if (select <= _activityRate)
        {//이동시
            ChangedAction(PlayerController.eStateAction.WALK);
            switch (_roamType)
            {//몬스터 배회방법에 따른 이동방식
                case SpawnControl.eRoammingType.RANDOMPOS:   // 랜덤하게 위치지정
                    _posTarget = GetRandomTargetPos(_posSpawn, _limitAreaX, _limitAreaZ);
                    break;
                case SpawnControl.eRoammingType.POINTPOS_SEQUENTIAL: // 지정한 위치들을 순차적으로 이동(반복)
                    _idxRoamming++;
                    if (_idxRoamming == _ltPoints.Count)
                        _idxRoamming = 0;
                    _posTarget = _ltPoints[_idxRoamming];
                    break;
                case SpawnControl.eRoammingType.POINTPOS_PINGPONG: // 지정한 위치들을 순차적으로 갔다 역순으로 돌아옴
                    if (_isPP == false)
                    {
                        _posTarget = _ltPoints[_idxRoamming];
                        _idxRoamming++;
                        if (_idxRoamming == (_ltPoints.Count-1))
                            _isPP = true;
                    }
                    else
                    {
                        _posTarget = _ltPoints[_idxRoamming];
                        _idxRoamming--;
                        if (_idxRoamming == 0)
                            _isPP = false;
                    }
                    break;
                case SpawnControl.eRoammingType.POINTPOS_RANDOM: // 지정한 위치들을 랜덤하게 돌아다님\
                    int _randRoamming = Random.Range(0, _ltPoints.Count);
                    _posTarget = _ltPoints[_randRoamming];
                    break;
                    
            }
            _naviAgent.SetDestination(_posTarget);
            _isSelectedAI = true;
            Debug.Log("이동 : " + _posTarget.ToString() + _activityRate.ToString());
        }
        else
        {//대기시
            ChangedAction(PlayerController.eStateAction.IDLE);
            _timeWait = Random.Range(_minWaitTime, _maxWaitTime);
            _isSelectedAI = true;
            Debug.Log("대기 : " + _timeWait.ToString() + "초" + _activityRate.ToString());
        }

    }
    public void ChangedAction(PlayerController.eStateAction state)
    {
        if (_isDead)
            return;

        switch (state)
        {
            case PlayerController.eStateAction.IDLE:
                _naviAgent.enabled = false;
                break;
            case PlayerController.eStateAction.WALK:
                _naviAgent.enabled = true;
                _naviAgent.speed = 1.5f;
                _naviAgent.stoppingDistance = 0;
                break;
            case PlayerController.eStateAction.RUN:
                _naviAgent.enabled = true;
                _naviAgent.speed = 4.5f;
                _naviAgent.stoppingDistance = _attRange;
                break;
            case PlayerController.eStateAction.DIE:
                Anictrl.SetTrigger("Death");
                _isDead = true;
                InGameManager._instance.countKill++;
                Destroy(gameObject, 5);
                break;
            case PlayerController.eStateAction.HIT:
                break;

            
        }
        Anictrl.SetInteger("AniState", (int)state);
        _curActState = state;

    }
    Vector3 GetRandomTargetPos(Vector3 centerPos, float limitX, float limitZ)
    {
        float x = Random.Range(-limitX, limitX);
        float z = Random.Range(-limitZ, limitZ);

        return centerPos + new Vector3(x, 0, z);
    }
    public void InitStatData(string name, int addAtt, int addDef, int addLife)
    {
        _Name = name;
        InitData(5, 1, 50);
        AddData(addAtt, addDef, addLife);
    }
    public void EnableHitZone()
    {
        _collHitZone.enabled = true;
    }
    public void DesableHitZone()
    {
        _collHitZone.enabled = false;
    }
    public void SettingPersonality(ePersonality p)
    {
        _personal = p;
        SettingPersonalityWithLate(_personal);
    }
    public void SettingRoammingType(SpawnControl.eRoammingType type, Transform[] points = null)
    {
        _roamType = type;
        if (_roamType != SpawnControl.eRoammingType.RANDOMPOS)
        {
            _ltPoints = new List<Vector3>();
            for(int n=0; n<points.Length; n++)
            {
                _ltPoints.Add(points[n].position);
            }
        }
            
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("RoammingPoint"))
        {
            _isSelectedAI = false;
        }
        if(other.CompareTag("Magic"))
        {
            GameObject go = GameObject.FindGameObjectWithTag("Magic");
            MagicControl Fire = go.GetComponent<MagicControl>();
            if (DamageMe(Fire.MagicDamage))
            {
                ChangedAction(PlayerController.eStateAction.DIE);   
            }
            else
            {
                ChangedAction(PlayerController.eStateAction.HIT);
            }
        }
        if (other.CompareTag("Plasma"))
        {
            if (DamageMe(BulletControl._Instance.BulletDamage))
            {
                ChangedAction(PlayerController.eStateAction.DIE);
            }
            else
            {
                ChangedAction(PlayerController.eStateAction.HIT);
            }
        }
    }
    public void SetUIInfo()
    {
        
        HPbar.maxValue = _MaxHP;
        HPbar.value = _CurHP;
        _MonsterNameInfo.text = (_Name);
    }
}
