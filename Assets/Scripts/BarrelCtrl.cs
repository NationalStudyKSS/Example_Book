using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    // 폭발 효과 파티클을 연결할 변수
    [SerializeField] GameObject _expEffect;
    // 무작위로 적용할 텍스쳐 배열
    [SerializeField] Texture[] _textures;
    // 폭발 반경
    [SerializeField] float _radius = 10.0f;
    // 하위에 있는 Mesh Renderer 컴포넌트를 저장할 변수
    [SerializeField] MeshRenderer _renderer;

    // 컴포넌트를 저장할 변수
    [SerializeField] Transform _tr;
    [SerializeField] Rigidbody _rb;

    // 총알 맞은 횟수를 누적시킬 변수
    [SerializeField] int _hitCount = 0;

    private void Start()
    {
        _tr = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        // 하위에 있는 MeshRender 컴포넌트 추출
        _renderer = GetComponentInChildren<MeshRenderer>();

        // 난수 발생
        int idx = Random.Range(0, _textures.Length);
        // 텍스처 지정
        _renderer.material.mainTexture = _textures[idx];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Bullet"))
        {
            // 총알 맞은 횟수를 증가시키고 3회 이상이면 폭발 처리
            if (++_hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }

    /// <summary>
    /// 드럼통을 폭발시킬 함수
    /// </summary>
    void ExpBarrel()
    {
        // 폭발 효과 파티클 생성
        GameObject exp = Instantiate(_expEffect, _tr.position, _tr.rotation);
        // 폭발 효과 파티클 5초 후에 제거
        Destroy(exp, 5f);

        // Rigidbody 컴포넌트의 amss를 1.0으로 수정해 무게를 가볍게 함
        _rb.mass = 1.0f;
        // 위로 솟구치는 힘을 가함
        _rb.AddForce(Vector3.up * 1500.0f);

        // 간접 폭발력 전달
        IndeirectDamage(_tr.position);

        // 3초 후에 드럼통 제거
        Destroy(gameObject, 3f);
    }

    /// <summary>
    /// 폭발력을 주변에 전달하는 함수
    /// </summary>
    /// <param name="pos"></param>
    void IndeirectDamage(Vector3 pos)
    {
        // 주변에 있는 드럼통을 모두 추출
        Collider[] colls = Physics.OverlapSphere(pos, _radius, 1 << 3);

        foreach (var coll in colls)
        {
            // 폭발 범위에 포함된 드럼통의 Rigidbody 컴포넌트 추출
            _rb = coll.GetComponent<Rigidbody>();
            // 드럼통의 무게를 가볍게 함
            _rb.mass = 1.0f;
            // freezeRotation 제한값을 해제
            _rb.constraints = RigidbodyConstraints.None;
            // 폭발력을 전달
            _rb.AddExplosionForce(1500.0f, pos, _radius, 1200.0f);
        }
    }
}
