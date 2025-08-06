using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    // 스파크 파티클 프리팹을 연결할 변수
    [SerializeField] GameObject _sparkEffect;

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 Bullet 태그를 가지고 있다면
        if (collision.collider.CompareTag("Bullet"))
        {
            // 첫 번째 충돌 지점의 정보 추출
            ContactPoint cp = collision.GetContact(0);

            // 충돌한 총알의 법선 벡터를 쿼터니언 타입으로 변환
            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            // 스파크 파티클을 동작으로 생성
            GameObject spark = Instantiate(_sparkEffect, cp.point, rot);
            // 일정 시간이 지난 후 스파크 파티클을 삭제
            Destroy(spark, 0.5f);

            // 해당 오브젝트를 파괴
            Destroy(collision.gameObject);
        }
    }
}
