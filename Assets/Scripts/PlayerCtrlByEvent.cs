using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrlByEvent : MonoBehaviour
{
    [SerializeField] InputAction _moveAction;
    [SerializeField] InputAction _attackAction;

    [SerializeField] Animator _anim;
    [SerializeField] Vector3 _moveDir;

    private void Start()
    {
        _anim = GetComponent<Animator>();

        // Move 액션 생성 및 타입 설정
        _moveAction = new InputAction("Move", InputActionType.Value);

        // Move 액션의 복합 바인딩 정보 정의
        _moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        // Move 액션의 performed, canceld, 이벤트 연결
        _moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            // 2차원 좌표를 3차원 좌표로 변환
            _moveDir = new Vector3(dir.x, 0, dir.y);
            // Warrior_Run 애니메이션 실행
            _anim.SetFloat("Movement", dir.magnitude);
        };

        _moveAction.canceled += ctx =>
        {
            _moveDir = Vector3.zero;
            // Warrior_Run 애니메이션 정지
            _anim.SetFloat("Movement", 0);
        };

        // Move 액션의 활성화
        _moveAction.Enable();

        // Attack 액션 생성
        _attackAction = new InputAction("Attack", InputActionType.Button, "<Keyboard>/space");

        // Attack 액션의 performed 이벤트 연결
        _attackAction.performed += ctx =>
        {
            _anim.SetTrigger("Attack");
        };
        // Attack 액션의 활성화
        _attackAction.Enable();
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
}
