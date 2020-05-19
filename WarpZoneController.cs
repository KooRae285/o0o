using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpZoneController : MonoBehaviour
{

    [SerializeField] Transform TargetPosition;
    [SerializeField] GameObject _effRing;
    float _timeCheck = 0;
    bool _isIn;
    GameObject _playerObj;
    // Start is called before the first frame update
    void Awake()
    {
        _isIn = false;
    }
    void Start()
    {
        _effRing.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(_isIn)
        {
            _timeCheck += Time.deltaTime;
            if(_timeCheck >= 5)
            {
                _isIn = false;
                _effRing.SetActive(false);
                _playerObj.transform.position = TargetPosition.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isIn = true;
            _effRing.SetActive(true);
            _timeCheck = 0;
            _playerObj = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _isIn = false;
            _effRing.SetActive(false);
            _playerObj = null;
            
        }
    }
    

}
