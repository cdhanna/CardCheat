using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlHintBehaviour : MonoBehaviour
{
    public TextMeshProUGUI controlText;

    private Action _action;
    private KeyCode _code;
    private bool _hasAction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_hasAction && Input.GetKeyDown(_code))
        {
            _action?.Invoke();
        }        
    }

    public void SetAction(KeyCode code, Action action)
    {
        _code = code;
        _hasAction = true;
        _action = action;
        controlText.text = $"({code.ToString()})";
    }
}
