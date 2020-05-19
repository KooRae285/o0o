using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicControl : MonoBehaviour
{
    [SerializeField] float _forceForward =500;
    [SerializeField] float _timeLife = 5;
    [SerializeField] GameObject _HitEffect;
    PlayerController _ownerPC;
    int _Damage;
    Rigidbody _rgd3D;

    public int MagicDamage
    {
        get
        {
            return _Damage;
        }
    }
    private void Awake()
    {
        _rgd3D = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
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
        if(other.CompareTag("UnbrokeObj"))
        {
            return;
        }
        GameObject go = Instantiate(_HitEffect);
        go.transform.position = transform.position;
        Destroy(go, 1.5f);
        Destroy(gameObject);
        
    }
    public void InitDamage(PlayerController pc)
    {
        _ownerPC = pc;
        _Damage = pc._FinishATT;
    }

}
