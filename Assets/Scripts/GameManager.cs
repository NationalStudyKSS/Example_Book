using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 몬스터가 출현할 위치를 저장할 List 타입 변수
    [SerializeField] List<Transform> _points = new List<Transform>();

    // 몬스터를 미리 생성해 저장할 리스트 자료형
    [SerializeField] List<GameObject> _monsterPool = new List<GameObject>();

    // 오브젝트 풀(Object Pool)에 생성할 몬스터의 최대 개수
    [SerializeField] int _maxMosters = 10;

    // 몬스터 프리펩을 연결할 변수
    [SerializeField] GameObject _monster;

    // 몬스터의 생성 간격
    [SerializeField] float _createTime = 3.0f;

    // 게임의 종료 여부를 저장할 멤버 변수
    [SerializeField] bool _isGameOver;

    // 스코어 텍스트를 연결할 변수
    [SerializeField] TextMeshProUGUI _scoreText;
    // 누적 점수를 기록하기 위한 변수
    [SerializeField] int _totScore = 0;

    // 게임의 종료 여부를 저장할 프로퍼티
    public bool IsGameObver
    {
        get { return _isGameOver; }
        set
        {
            _isGameOver = value;
            if (_isGameOver)
            {
                CancelInvoke("CreateMonster");
            }
        }
    }

    // 싱글턴 인스턴스 선언
    public static GameManager Instance = null;

    // 스크립트가 실행되면 가장 먼저 호출되는 유니티 이벤트 함수
    private void Awake()
    {
        // instance가 할당되지 않았을 경우
        if (Instance == null)
        {
            Instance = this;
        }
        // Instance에 할당된 클래스의 인스턴스가 다를 경우 새로 생성된 클래스를 의미함
        else if(Instance != this)
        {
            Destroy(this.gameObject);
        }

        // 다른 씬으로 넘어가더라도 삭제하지 않고 유지함
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        // 몬스터 오브젝트 풀 생성
        CreateMonsterPool();

        // SpawnPointGroup 게임오브젝트의 Trasnform 컴포넌트를 추출
        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        // SpawnPointGroup 하위에 있는 모든 차일드 게임오브젝트의 Transform 컴포넌트 추출
        //spawnPointGroup?.GetComponentsInChildren<Transform>(_points);
        foreach(Transform point in spawnPointGroup)
        {
            // List에 Transform 컴포넌트를 추가
            _points.Add(point);
        }

        // 일정한 시간 간격으로 함수 호출
        InvokeRepeating("CreateMonster", 2.0f, _createTime);

        // 스코어 점수 출력
        _totScore = PlayerPrefs.GetInt("Tot_Score", 0);
        DisplayScore(0);
    }

    void CreateMonster()
    {
        // 몬스터의 불규칙한 생성 위치 산출
        int idx = Random.Range(0, _points.Count);
        // 몬스터 프리펩 생성
        //Instantiate(_monster, _points[idx].position, _points[idx].rotation);

        // 오브젝트 풀에서 몬스터 추출
        GameObject monster = GetMonsterInPool();
        // 추출한 몬스터의 위치와 회전을 설정
        monster?.transform.SetPositionAndRotation(_points[idx].position, _points[idx].rotation);
        // 추출한 몬스터를 활성화
        monster?.SetActive(true);
    }

    /// <summary>
    /// 오브젝트 풀에 몬스터를 생성하는 함수
    /// </summary>
    void CreateMonsterPool()
    {
        for(int i =0;i<_maxMosters; i++)
        {
            // 몬스터 생성
            var monster = Instantiate<GameObject>(_monster);
            // 몬스터의 이름을 지정
            monster.name = $"Monster_{i:00}";
            // 몬스터 비활성화
            monster.SetActive(false);

            // 생성한 몬스털르 오브젝트 풀에 추가
            _monsterPool.Add(monster);
        }
    }

    /// <summary>
    /// 오브젝트 풀에서 사용 가능한 몬스터를 추출해 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public GameObject GetMonsterInPool()
    {
        // 오브젝트 풀의 처음부터 끝까지 순회
        foreach (var monster in _monsterPool)
        {
            // 비활성화 여부로 사용 가능한 몬스터를 판단
            if (monster.activeSelf == false)
            {
                // 몬스터 반환
                return monster;
            }
        }

        return null;
    }

    /// <summary>
    /// 점수를 누적하고 출력하는 함수
    /// </summary>
    /// <param name="score"></param>
    public void DisplayScore(int score)
    {
        _totScore += score;
        _scoreText.text = $"<color=#00ff00><b>Score :</color><color=#ff0000> {_totScore:#,##0}</b></color>";
        // 스코어 저장
        PlayerPrefs.SetInt("Tot_Score", _totScore);
    }
}
