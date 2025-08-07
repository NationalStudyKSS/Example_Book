using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    // 몬스터의 상태 정보
    public enum State
    {
        Idle,      // 대기 상태
        Trace,     // 추적 상태
        Attack,    // 공격 상태
        Die        // 사망 상태
    }

    // 몬스터의 현재 상태
    [SerializeField] State _state = State.Idle;
    // 추적 사정거리
    [SerializeField] float _traceDist = 10.0f;
    // 공격 사정거리
    [SerializeField] float _attackDist = 2.0f;
    // 몬스터의 사망 여부
    [SerializeField] bool _isDie = false;

    // 컴포넌트의 캐시를 처리할 변수
    [SerializeField] Transform _monsterTr;
    [SerializeField] Transform _playerTr;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Animator _anim;

    // Animator 파라미터의 해시값 추출
    readonly int _hashTrace = Animator.StringToHash("IsTrace");
    readonly int _hashAttack = Animator.StringToHash("IsAttack");
    readonly int _hashHit = Animator.StringToHash("Hit");
    readonly int _hashPlayerDie = Animator.StringToHash("PlayerDie");
    readonly int _hashSpeed = Animator.StringToHash("Speed");
    readonly int _hashDie = Animator.StringToHash("Die");

    // 몬스터 생명 변수
    [SerializeField] int _hp = 100;

    // 혈흔 효과 프리펩
    [SerializeField] GameObject _bloodEffectPrefab;

    // 스크립트가 활성화 될 때마다 호출되는 함수
    private void OnEnable()
    {
        // 몬스터의 상태를 Idle로 초기화
        _state = State.Idle;

        // 이벤트 발생 시 수행할 함수 연결
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;

        // 몬스터의 상태를 체크하는 코루틴 함수 호출
        StartCoroutine(CheckMonsterState());

        // 상태에 따라 몬스터의 행동을 수행하는 코루틴 함수 호출
        StartCoroutine(MonsterAction());
    }

    // 스크립트가 비활성화 될 때마다 호출되는 함수
    private void OnDisable()
    {
        // 이벤트 발생 시 수행할 함수 연결 해제
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
    }

    private void Awake()
    {
        // 몬스터의 Transform 할당
        _monsterTr = GetComponent<Transform>();
        
        // 추적 대상인 Player의 Transform 할당
        _playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();

        // NaveMaeshAgent 컴포넌트 할당
        _agent = GetComponent<NavMeshAgent>();
        // NavMeshAgent의 자동 회전 기능 비활성화
        _agent.updateRotation = false;

        // Animator 컴포넌트 할당
        _anim = GetComponent<Animator>();

        // BloodEffect 프리펩 로드
        _bloodEffectPrefab = Resources.Load<GameObject>("BloodEffect");

        // 추적 대상의 위치를 설정하면 바로 추적 시작
        //_agent.SetDestination(_playerTr.position);
    }

    private void Update()
    {
        // 목적지까지 남은 거리로 회전 여부 판단
        if (_agent.remainingDistance >= 2.0f)
        {
            // 에이전트의 이동 방향 
            Vector3 direction = _agent.desiredVelocity;
            // 회전 각도(쿼터니언 산출)
            Quaternion rot = Quaternion.LookRotation(direction);
            // 구면 선형보간 함수로 부드러운 회전 처리
            _monsterTr.rotation = Quaternion.Slerp(_monsterTr.rotation, rot, Time.deltaTime * 10.0f);
        }
    }

    /// <summary>
    /// 일정한 간격으로 몬스터의 행동 상태를 체크하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckMonsterState()
    {
        while (!_isDie)
        {
            // 0.3초 동안 중지(대기)하는 동안 제어권을 메시지 루프에 양보
            yield return new WaitForSeconds(0.3f);

            // 몬스터의 상태가 Die일 때 코루틴을 종료
            if (_state == State.Die) yield break;

            // 몬스터와 주인공 캐릭터 사이의 거리 측정
            float distance = Vector3.Distance(_playerTr.position, _monsterTr.position);

            // 공격 사정거리 범위로 들어왔는지 확인
            if(distance <= _attackDist)
            {
                // 공격 상태로 전환
                _state = State.Attack;
            }

            // 추적 사정거리 범위로 들어왔는지 확인
            else if (distance <= _traceDist)
            {
                // 추적 상태로 전환
                _state = State.Trace;
            }

            else
            {
                // 대기 상태로 전환
                _state = State.Idle;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!_isDie)
        {
            switch (_state)
            {
                // Idle 상태
                case State.Idle:
                    // 추적 중지
                    _agent.isStopped = true; // 이동 중지

                    // Animator의 IsTrace 변수를 false로 설정
                    _anim.SetBool(_hashTrace, false);
                    break;

                // Trace 상태    
                case State.Trace:
                    // 추적 대상의 좌표로 이동 시작
                    _agent.SetDestination(_playerTr.position);
                    _agent.isStopped = false; // 이동 재개

                    // Animator의 IsTrace 변수를 true로 설정
                    _anim.SetBool(_hashTrace, true);

                    // Animator의 IsAttack 변수를 false로 설정
                    _anim.SetBool(_hashAttack, false);
                    break;

                // Attack 상태
                case State.Attack:
                    // Animator의 IsAttack 변수를 true로 설정
                    _anim.SetBool(_hashAttack, true);
                    break;

                case State.Die:
                    _isDie = true;
                    // 추적 정지
                    _agent.isStopped = true;
                    // 사망 애니메이션 실행
                    _anim.SetTrigger(_hashDie);
                    // 몬스터의 collider 컴포넌트 비활성화
                    GetComponent<Collider>().enabled = false;

                    // 일정 시간 대기 후 오브젝트 풀링으로 환원
                    yield return new WaitForSeconds(3.0f);

                    // 사망 후 다시 사용할 때를 위해 hp 값 초기화
                    _hp = 100;
                    _isDie = false;

                    // 몬스터의 Collider 컴포넌트 활성화
                    GetComponent<Collider>().enabled = true;
                    // 몬스터를 비활성화
                    this.gameObject.SetActive(false);
                    
                    break;

                default:
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            // 충돌한 총알을 삭제
            Destroy(collision.gameObject);
            // 피격 리액션 애니메이션 실행
            _anim.SetTrigger(_hashHit);

            // 총알의 충돌 지점
            Vector3 pos = collision.GetContact(0).point;
            // 총알의 충돌 지점의 법선 벡터
            Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);
            // 혈흔 효과를 생성하는 함수 호출
            ShowBloodEffect(pos, rot);

            // 몬스터의 hp 차감
            _hp -= 10;
            if (_hp <= 0)
            {
                _state = State.Die;
                // 몬스터가 사망했을 때 50점을 추가
                GameManager.Instance.DisplayScore(50);
            }
        }
    }

    /// <summary>
    /// 레이캐스트를 사용해 데미지를 입히는 로직
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="normal"></param>
    public void OnDamage(Vector3 pos, Vector3 normal)
    {
        _anim.SetTrigger(_hashHit);
        Quaternion rot = Quaternion.LookRotation(normal);

        // 혈흔 효과를 생성하는 함수 호출
        ShowBloodEffect(pos, rot);

        // 몬스터의 hp 차감
        _hp -= 30;
        if (_hp <= 0)
        {
            _state = State.Die;
            // 몬스터가 사망했을 때 50점을 추가
            GameManager.Instance.DisplayScore(50);
        }
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        // 혈흔 효과 생성
        GameObject blood = Instantiate<GameObject>(_bloodEffectPrefab, pos, rot, _monsterTr);
        Destroy(blood, 1.0f);
    }

    private void OnDrawGizmos()
    {
        // 몬스터의 현재 상태에 따라 Gizmos 색상 변경
        switch (_state)
        {
            case State.Idle:
                Gizmos.color = Color.green;
                break;
            case State.Trace:
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_monsterTr.position, _traceDist);
                break;
            case State.Attack:
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_monsterTr.position, _attackDist);
                break;
            case State.Die:
                Gizmos.color = Color.gray;
                break;
        }
    }

    /// <summary>
    /// Player가 죽었을 때 실행되는 함수
    /// </summary>
    void OnPlayerDie()
    {
        // 몬스터의 상태를 체크하는 코루틴 함수를 모두 정지시킴
        StopAllCoroutines();

        // 추적을 정지하고 애니메이션을 수행
        _agent.isStopped = true;
        _anim.SetFloat(_hashSpeed, Random.Range(0.8f, 1.2f));
        _anim.SetTrigger(_hashPlayerDie);
    }
}
