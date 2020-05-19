using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    [SerializeField] float _forceForward = 500;
    [SerializeField] float _timeLife = 5;
    [SerializeField] GameObject _HitEffect;
    PlayerController _ownerPC;
    int _Damage;
    Rigidbody _rgd3D;
    static BulletControl _uniqueInstance;
    public static BulletControl _Instance
    {
        get
        {
            return _uniqueInstance;
        }

    }   
    public int BulletDamage
    {
        get
        {
            return _Damage;
        }
    }
    // Start is called before the first frame update
    private void Awake()
    {
        _uniqueInstance = this;
        _rgd3D = GetComponent<Rigidbody>();
    }
    void Start()
    {
        _rgd3D.AddForce(transform.forward * _forceForward);
        Destroy(gameObject, _timeLife);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("UnbrokeObj") || other.CompareTag("Monster"))
        {
            GameObject go = Instantiate(_HitEffect);
            go.transform.position = transform.position;
            Destroy(go, 1.8f);
            Destroy(gameObject);
        }
        
    }
    public void InitDamage(PlayerController pc)
    {
        _ownerPC = pc;
        _Damage = pc._FinishATT;
    }
}
