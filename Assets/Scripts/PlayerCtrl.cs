using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    // 컴포넌트를 캐시 처리할 변수
    [SerializeField] Transform _tr;

    // Animation 컴포넌트를 저장할 변수
    [SerializeField] Animation _anim;

    // 이동 속력 변수
    [Range(0, 10f)][SerializeField] float _moveSpeed = 10.0f;

    // 회전 속도 변수
    [Range(0, 360f)][SerializeField] float _turnSpeed = 360.0f;

    // 초기 생명 값
    readonly float _initHp = 100.0f;
    // 현재 생명 값
    [SerializeField] float _curHp;

    // 델리게이트 선언
    public delegate void PlayerDieHandler();
    // 이벤트 선언
    public static event PlayerDieHandler OnPlayerDie;

    private IEnumerator Start()
    {
        // Hp 초기화
        _curHp = _initHp;

        // 컴포넌트를 추출해 변수에 대입
        _tr = GetComponent<Transform>();
        _anim = GetComponent<Animation>();

        // 애니메이션 실행
        _anim.Play("Idle");

        _turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f); // 0.3초 대기
        _turnSpeed = 360.0f;

        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 잠금
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

    // 충돌한 Collider의 IsTrigger 옵션이 체크됐을 때 발생
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 Collider가 몬스터의 Punch이면 Player의 Hp 차감
        if (_curHp >= 0.0f && other.CompareTag("Punch"))
        {
            _curHp -= 10.0f;
            Debug.Log($"Player의 현재 Hp = {_curHp}/{_initHp}");

            // Player의 생명이 0 이하이면 사망 처리
            if (_curHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    // Player의 사망 처리
    void PlayerDie()
    {
        Debug.Log("Player Die!");

        //// Monster 태그를 가진 모든 게임오브젝트를 찾아옴
        //GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        //// 모든 몬스터의 OnPlayerDie 함수를 순차적으로 호출
        //foreach(GameObject monster in monsters)
        //{
        //    // 몬스터의 OnPlayerDie 함수 호출
        //    monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}

        // 주인공 사망 이벤트 호출(발생)
        OnPlayerDie();
    }
}
