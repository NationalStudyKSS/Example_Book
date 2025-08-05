using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    // 총알 프리펩
    [SerializeField] GameObject _bullet;
    // 총알 발사 좌표
    [SerializeField] Transform _firePos;

    private void Start()
    {
    }

    private void Update()
    {
        // 마우스 왼쪽 버튼 클릭 시 총알 발사
        if (Input.GetMouseButton(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        // Bullet 프리펩을 동적으로 생성(생성할 객체, 위치, 회전)
        Instantiate(_bullet, _firePos.position, _firePos.rotation);
    }
}
