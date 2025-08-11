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

        // Move �׼� ���� �� Ÿ�� ����
        _moveAction = new InputAction("Move", InputActionType.Value);

        // Move �׼��� ���� ���ε� ���� ����
        _moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        // Move �׼��� performed, canceld, �̺�Ʈ ����
        _moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            // 2���� ��ǥ�� 3���� ��ǥ�� ��ȯ
            _moveDir = new Vector3(dir.x, 0, dir.y);
            // Warrior_Run �ִϸ��̼� ����
            _anim.SetFloat("Movement", dir.magnitude);
        };

        _moveAction.canceled += ctx =>
        {
            _moveDir = Vector3.zero;
            // Warrior_Run �ִϸ��̼� ����
            _anim.SetFloat("Movement", 0);
        };

        // Move �׼��� Ȱ��ȭ
        _moveAction.Enable();

        // Attack �׼� ����
        _attackAction = new InputAction("Attack", InputActionType.Button, "<Keyboard>/space");

        // Attack �׼��� performed �̺�Ʈ ����
        _attackAction.performed += ctx =>
        {
            _anim.SetTrigger("Attack");
        };
        // Attack �׼��� Ȱ��ȭ
        _attackAction.Enable();
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
}
