using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    public enum eRoammingType
    {
        RANDOMPOS = 0,
        POINTPOS_SEQUENTIAL,
        POINTPOS_PINGPONG,
        POINTPOS_RANDOM
    }

    [SerializeField] eRoammingType _roamType;
    [SerializeField] MonsterController.ePersonality _spawnPersonality;
    [SerializeField] GameObject _prefabMonster1;
    [SerializeField] float _timeSpawn = 3;
    [SerializeField] int _limitCountSpawn = 4;

    float _timeCheck;
    List<GameObject> monsters;
    Transform _rootRoam;
    Transform[] _roamPoints;

    public int _curLiveMonsterCount
    {
        get
        {
            return monsters.Count;
        }
    }
    

    void Awake()
    {
        monsters = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _rootRoam = transform.GetChild(0);
        GatheringRoammingPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (InGameManager._instance._EnableSpawn)
        {
            if (monsters.Count < _limitCountSpawn)
            {
                _timeCheck += Time.deltaTime;
                if (_timeCheck >= _timeSpawn)
                {
                    _timeCheck = 0;
                    SpawnObject();
                    InGameManager._instance.CheckCountMonster();

                }
            }
        }
    }
    void LateUpdate()
    {
        foreach (GameObject i in monsters)
        {
            if (i == null)
            {
                monsters.Remove(i);
                InGameManager._instance.CheckCountMonster();
                break;
            }
        }
        
    }

    void SpawnObject()
    {
        GameObject go = Instantiate(_prefabMonster1, transform.position, transform.rotation);
        go.transform.SetParent(transform);
        MonsterController mc = go.GetComponent<MonsterController>();
        mc.SettingPersonality(_spawnPersonality);
        mc.SettingRoammingType(_roamType, _roamPoints);
        monsters.Add(go);

    }
    public void GatheringRoammingPoint()
    {
        if (_rootRoam.childCount == 0)
            return;

        _roamPoints = new Transform[_rootRoam.childCount];
        for(int n = 0; n<_roamPoints.Length; n++)
        {
            _roamPoints[n] = _rootRoam.GetChild(n);
        }
    }
}
