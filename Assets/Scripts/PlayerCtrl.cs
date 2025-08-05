using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    // 컴포넌트를 캐시 처리할 변수
    [SerializeField] Transform _tr;

    // 이동 속력 변수
    [Range(0, 10f)][SerializeField] float _moveSpeed = 10.0f;

    // 회전 속도 변수
    [Range(0, 360f)][SerializeField] float _turnSpeed = 360.0f;

    // Animation 컴포넌트를 저장할 변수
    [SerializeField] Animation _anim;

    private void Start()
    {
        // 컴포넌트를 추출해 변수에 대입
        _tr = GetComponent<Transform>();
        _anim = GetComponent<Animation>();

        // 애니메이션 실행
        _anim.Play("Idle");
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        //Debug.Log("h = " + h);
        //Debug.Log("v = " + v);

        //_tr.position += Vector3.forward * 1;

        // 전후좌우 이동 방향 벡터 계산
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        // Translate(이동 방향 * 속력 * Time.deltaTime);
        _tr.Translate(moveDir.normalized * _moveSpeed * Time.deltaTime);

        // Vector3.up 축을 기준으로 _turnSpeed만큼의 속도로 회전
        _tr.Rotate(Vector3.up * _turnSpeed * Time.deltaTime * r);

        PlayerAnim(h, v);
    }

    void PlayerAnim(float h, float v)
    {
        // 키보드 입력값을 기준으로 동작할 애니메이션 수행

        if (v >= 0.1f)
        {
            _anim.CrossFade("RunF", 0.25f); // 전진 애니메이션 실행
        }
        else if (v <= -0.1f)
        {
            _anim.CrossFade("RunB", 0.25f); // 후진 애니메이션 실행
        }
        else if(h <= -0.1f)
        {
            _anim.CrossFade("RunL", 0.25f); // 좌측 애니메이션 실행
        }
        else if (h >= 0.1f)
        {
            _anim.CrossFade("RunR", 0.25f); // 우측 애니메이션 실행
        }
        else
        {
            _anim.CrossFade("Idle", 0.25f); // 아무 동작도 하지 않을 때 Idle 애니메이션 실행
        }
    }
}
