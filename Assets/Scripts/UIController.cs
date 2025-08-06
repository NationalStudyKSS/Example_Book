using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // 버튼을 연결할 변수
    [SerializeField] Button _startButton;
    [SerializeField] Button _optionButton;
    [SerializeField] Button _shopButton;

    [SerializeField] UnityAction _action;

    private void Start()
    {
        // UnityAction을 사용한 이벤트 연결 방식1
        _action = () => OnButtonClick(_startButton.name);
        _startButton.onClick.AddListener(_action);

        // 무명 메서드를 활용한 이벤트 연결 방식
        _optionButton.onClick.AddListener(delegate
        { 
            OnButtonClick(_optionButton.name); 
        }
        );

        // 람다식을 활용한 이벤트 연결 방식
        _shopButton.onClick.AddListener(() => OnButtonClick(_shopButton.name));
    }

    public void OnButtonClick(string msg)
    {
        Debug.Log($"Click Button : {msg}");
    }
}
