using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Status
{
    public enum eStateAction
    {
        IDLE = 0,
        WALK,
        RUN,
        ATTACK,
        ATTACK2,
        HIT,
        DIE
    }
    [SerializeField] GameObject _HitEffect;
    [SerializeField] GameObject _preMagic;
    [SerializeField] GameObject _preBullet;
    [SerializeField] Transform _tfMagicPosition;
    eStateAction _curActState;
    Animator Anictrl;
    bool _isDead;
    bool _isAttack;
    public float MovSpeed = 10;
    public float RotAngle = 150;
    string _Name;

    public string GetName
    {
        get
        {
            return _Name;
        }
    }
    
    void Awake()
    {
        Anictrl = GetComponent<Animator>();
        _isDead = false;
    }
    void Start()
    {
        //InitStatData("qwerty", 5, 1, 15);
    }

    // Update is called once per frame
    void Update()
    {
        if (InGameManager._instance._nowGameState == InGameManager.eGameState.PLAY)
        {
            if (_isDead)
                return;
            if (_isAttack)
                return;
            Charmov();
        }
    }
    
    public void ChangedAction(eStateAction state)
    {
        if (_isDead)
            return;
        switch (state)
        {
            case eStateAction.DIE:
                Anictrl.SetTrigger("Death");
                _isDead = true;
                break;
            case eStateAction.ATTACK:
                Anictrl.SetTrigger("Attack");
                _isAttack = true;
                break;
            case eStateAction.ATTACK2:
                Anictrl.SetTrigger("Attack2");
                _isAttack = true;
                break;
            default:
                Anictrl.SetInteger("AniState", (int)state);
                break;
        }
        _curActState = state;

    }

    public void CancelAttacked()
    {
        Debug.Log("CancelAttacked");
        _isAttack = false;
    }



    //void OnGUI()
    //{
    //    //if (GUI.Button(new Rect(0, 0, 300, 120), "<size=50>IDLE</size>"))
    //    //{
    //    //    ChangedAction(eStateAction.IDLE);
    //    //}
    //    //if (GUI.Button(new Rect(300, 0, 300, 120), "<size=50>RUN</size>"))
    //    //{
    //    //    ChangedAction(eStateAction.RUN);
    //    //}
    //    //if (GUI.Button(new Rect(1200, 0, 300, 120), "<size=50>WALK</size>"))
    //    //{
    //    //    ChangedAction(eStateAction.WALK);
    //    //}
    //    if (GUI.Button(new Rect(600, 0, 300, 120), "<size=50>ATTACK</size>"))
    //    {
    //        ChangedAction(eStateAction.ATTACK);
    //    }
    //    if (GUI.Button(new Rect(900, 0, 300, 120), "<size=50>DIE</size>"))
    //    {
    //        ChangedAction(eStateAction.DIE);
    //    }
    //}

    void Charmov()
    {

        float mz = Input.GetAxis("Vertical");
        float mx = Input.GetAxis("Horizontal2");
        float rx = Input.GetAxis("Horizontal");
        Vector3 vector = new Vector3(mx, 0F, mz);
        vector = (vector.magnitude > 1) ? vector.normalized : vector;
        transform.Rotate(Vector3.up * rx * RotAngle * Time.deltaTime);
        transform.Translate(vector * MovSpeed * Time.deltaTime);


        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal2"))
        {
            ChangedAction(eStateAction.RUN);
        }
        else if (Input.GetButton("Horizontal"))
        {
            ChangedAction(eStateAction.WALK);
        }
        else
        {
            ChangedAction(eStateAction.IDLE);
        }


    }

    public void InitStatData(string name, int addAtt, int addDef, int addLife)
    {
        _Name = name;
        InitData(10, 1, 50);
        AddData(addAtt, addDef, addLife);
    }
    public void ClickAttack()
    {
        ChangedAction(eStateAction.ATTACK);
    }
    public void ClickAttack2()
    {
        ChangedAction(eStateAction.ATTACK2);
    }
    public void LuncherFireBall()
    {
        GameObject go = Instantiate(_preMagic);
        go.transform.position = _tfMagicPosition.position;
        go.transform.rotation = _tfMagicPosition.rotation;
        MagicControl fireball = go.GetComponent<MagicControl>();
        fireball.InitDamage(this);
    }
    public void LuncherBulletBall()
    {
        GameObject go = Instantiate(_preBullet);
        go.transform.position = _tfMagicPosition.position;
        go.transform.rotation = _tfMagicPosition.rotation;
        BulletControl bull = go.GetComponent<BulletControl>();
        bull.InitDamage(this);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            
            MonsterController mc = other.transform.parent.GetComponent<MonsterController>();
            if(DamageMe(mc._FinishATT))
            {
                GameObject go = Instantiate(_HitEffect);
                go.transform.position = transform.position;
                Destroy(go, 3.0f);
                Debug.Log("현재 HP : " + _nowHP.ToString());
                ChangedAction(eStateAction.DIE);
                Destroy(gameObject,5);
                Debug.Log("DIE");
            }
            else
            {
                GameObject go = Instantiate(_HitEffect);
                go.transform.position = transform.position;
                Destroy(go, 3.0f);
                Debug.Log("현재 HP : " + _nowHP.ToString());
                ChangedAction(eStateAction.HIT);
                
            }
            
        }
    }

}
