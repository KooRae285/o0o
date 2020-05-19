using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBox : MonoBehaviour
{
    [SerializeField] GameObject _prefabExplosion;
    [SerializeField] int _limitDuration = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Magic"))
        {
            _limitDuration--;
            if(_limitDuration <= 0)
            {
                GameObject go = Instantiate(_prefabExplosion);
                go.transform.position = other.gameObject.transform.position;
                Destroy(go, 3);
                Destroy(gameObject);
            }
            Destroy(other.gameObject);
        }
    }
}
