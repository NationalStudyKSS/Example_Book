using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WarriorCtrl : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [SerializeField] Vector3 _moveDir;

    [SerializeField] PlayerInput _playerInput;
    [SerializeField] InputActionMap _mainAcitonMap;
    [SerializeField] InputAction _moveAction;
    [SerializeField] InputAction _attackAction;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();

        // ActionMap ����
        _mainAcitonMap = _playerInput.actions.FindActionMap("PlayerActions");

        // Move, Attack �׼� ����
        _moveAction = _mainAcitonMap.FindAction("Move");
        _attackAction = _mainAcitonMap.FindAction("Attack");

        // Move �׼��� performed �̺�Ʈ ����
        _moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            // 2���� ��ǥ�� 3���� ��ǥ�� ��ȯ
            _moveDir = new Vector3(dir.x, 0, dir.y);
            // Warrior_Run �ִϸ��̼� ����
            _anim.SetFloat("Movement", dir.magnitude);
        };

        // Move �׼��� canceled �̺�Ʈ ����
        _moveAction.canceled += ctx =>
        {
            _moveDir = Vector3.zero;
            // Warrior_Run �ִϸ��̼� ����
            _anim.SetFloat("Movement", 0);
        };

        // Attack �׼��� performed �̺�Ʈ ����
        _attackAction.performed += ctx =>
        {
            Debug.Log("Attack bt c# event");
            _anim.SetTrigger("Attack");
        };
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

    #region Send_Message
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
    #endregion

    #region Unity_Events
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();

        // 2���� ��ǥ�� 3���� ��ǥ�� ��ȯ
        _moveDir = new Vector3(dir.x, 0, dir.y);

        // Warrior_Run �ִϸ��̼� ����
        _anim.SetFloat("Movement", dir.magnitude);
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log($"cxt.phase = {ctx.phase}");

        if (ctx.performed)
        {
            Debug.Log("Attack");
            _anim.SetTrigger("Attack");
        }
    }
    #endregion
}
