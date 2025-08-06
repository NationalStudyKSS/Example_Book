using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // ��ư�� ������ ����
    [SerializeField] Button _startButton;
    [SerializeField] Button _optionButton;
    [SerializeField] Button _shopButton;

    [SerializeField] UnityAction _action;

    private void Start()
    {
        // UnityAction�� ����� �̺�Ʈ ���� ���1
        _action = () => OnButtonClick(_startButton.name);
        _startButton.onClick.AddListener(_action);

        // ���� �޼��带 Ȱ���� �̺�Ʈ ���� ���
        _optionButton.onClick.AddListener(delegate
        { 
            OnButtonClick(_optionButton.name); 
        }
        );

        // ���ٽ��� Ȱ���� �̺�Ʈ ���� ���
        _shopButton.onClick.AddListener(() => OnButtonClick(_shopButton.name));
    }

    public void OnButtonClick(string msg)
    {
        Debug.Log($"Click Button : {msg}");
    }
}
