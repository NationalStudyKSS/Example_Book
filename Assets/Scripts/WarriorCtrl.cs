using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WarriorCtrl : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] Vector3 _moveDir;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_moveDir != Vector3.zero)
        {
            // ���� �������� ȸ��
            transform.rotation = Quaternion.LookRotation(_moveDir);
            // ȸ���� �� ���� �������� �̵�
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }
    }

    void OnMove(InputValue inputValue)
    {
        Vector2 dir= inputValue.Get<Vector2>();
        // 2���� ��ǥ�� 3���� ��ǥ�� ��ȯ
        _moveDir = new Vector3(dir.x, 0, dir.y);
        
        // Warrior_Run �ִϸ��̼� ����
        _anim.SetFloat("MoveMent", dir.magnitude);

        Debug.Log($"Move = ({dir.x}, {dir.y})");
    }

    void OnAttack()    
    {
        Debug.Log("Attack");
        _anim.SetTrigger("Attack");
    }
}
