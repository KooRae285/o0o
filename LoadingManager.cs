using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] Slider _LoadingBar;
    [SerializeField] Text _LoadingTxt;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SettingLoadingBar(float Value)
    {
        _LoadingBar.value = Value;
    }
    public void ShowText(string message)
    {
        _LoadingTxt.text = message;
    }
}
