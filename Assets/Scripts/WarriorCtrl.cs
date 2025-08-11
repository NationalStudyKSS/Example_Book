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

        // ActionMap 추출
        _mainAcitonMap = _playerInput.actions.FindActionMap("PlayerActions");

        // Move, Attack 액션 추출
        _moveAction = _mainAcitonMap.FindAction("Move");
        _attackAction = _mainAcitonMap.FindAction("Attack");

        // Move 액션의 performed 이벤트 연결
        _moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            // 2차원 좌표를 3차원 좌표로 변환
            _moveDir = new Vector3(dir.x, 0, dir.y);
            // Warrior_Run 애니메이션 실행
            _anim.SetFloat("Movement", dir.magnitude);
        };

        // Move 액션의 canceled 이벤트 연결
        _moveAction.canceled += ctx =>
        {
            _moveDir = Vector3.zero;
            // Warrior_Run 애니메이션 정지
            _anim.SetFloat("Movement", 0);
        };

        // Attack 액션의 performed 이벤트 연결
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
            // 진행 방향으로 회전
            transform.rotation = Quaternion.LookRotation(_moveDir);
            // 회전한 후 전진 방향으로 이동
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }
    }

    #region Send_Message
    void OnMove(InputValue inputValue)
    {
        Vector2 dir= inputValue.Get<Vector2>();
        // 2차원 좌표를 3차원 좌표로 변환
        _moveDir = new Vector3(dir.x, 0, dir.y);
        
        // Warrior_Run 애니메이션 실행
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

        // 2차원 좌표를 3차원 좌표로 변환
        _moveDir = new Vector3(dir.x, 0, dir.y);

        // Warrior_Run 애니메이션 실행
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
