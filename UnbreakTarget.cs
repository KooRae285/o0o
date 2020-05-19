using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbreakTarget : MonoBehaviour
{
    [SerializeField] GameObject _prefabHitEffect;

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
            GameObject go = Instantiate(_prefabHitEffect);
            go.transform.position = other.gameObject.transform.position;
            Destroy(go, 1.0f);
            Destroy(other.gameObject);

        }
    }
    //public void SetComponent()
    //{
    //    for(int i =0; i<gameObject.transform.childCount; i++)
    //    {
    //        fieldObj[i] = gameObject.transform.GetChild(i).gameObject;
    //        fieldObj[i].AddComponent<UnbreakTarget>();
    //        fieldObj[i].AddComponent<BoxCollider>();
    //    }
        
    //}
    
}
