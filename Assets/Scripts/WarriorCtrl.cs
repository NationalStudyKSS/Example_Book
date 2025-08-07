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
            // 진행 방향으로 회전
            transform.rotation = Quaternion.LookRotation(_moveDir);
            // 회전한 후 전진 방향으로 이동
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }
    }

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
}
