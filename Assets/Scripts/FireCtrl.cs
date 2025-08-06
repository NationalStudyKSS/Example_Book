using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    // 총알 프리펩
    [SerializeField] GameObject _bullet;
    // 총알 발사 좌표
    [SerializeField] Transform _firePos;
    // 총소리에 사용할 오디오 음원
    [SerializeField] AudioClip _fireSfx;

    // AudioSource 컴포넌트를 저장할 변수
    [SerializeField] AudioSource _audio;
    // Muzzle Flash의 MeshRenderer 컴포넌트
    [SerializeField] MeshRenderer _muzzleFlash;
    [SerializeField] Light _light;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();

        // FirePos 하위에 있는 MuzzleFlash의 Material 컴포넌틀르 추출
        _muzzleFlash = _firePos.GetComponentInChildren<MeshRenderer>();
        // 처음 시작할 때 비활성화
        _muzzleFlash.enabled = false;
        _light.enabled = false;
    }

    private void Update()
    {
        // 마우스 왼쪽 버튼 클릭 시 총알 발사
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        // Bullet 프리펩을 동적으로 생성(생성할 객체, 위치, 회전)
        Instantiate(_bullet, _firePos.position, _firePos.rotation);
        // 총소리 발생
        _audio.PlayOneShot(_fireSfx, 0.7f);
        // 총구 화염 효과 코루틴 함수 호출
        StartCoroutine(ShowMuzzleFlash());
    }

    IEnumerator ShowMuzzleFlash()
    {
        // 오프셋 좌표값을 랜덤 함수로 생성
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        // 텍스쳐의 오프셋 값 설정
        _muzzleFlash.material.mainTextureOffset = offset;

        // MuzzleFlash의 회전 반경
        float angle = Random.Range(0, 360);
        _muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);

        //MmuzzleFlash의 크기 조절
        float scale = Random.Range(1.0f, 2.0f);
        _muzzleFlash.transform.localScale = Vector3.one * scale;

        // MuzzleFlash 활성화
        _muzzleFlash.enabled = true;
        _light.enabled = true;

        // 0.2초 동안 대기(정지)하는 동안 메시지 루프로 제어권을 양보
        yield return new WaitForSeconds(0.2f);

        // MuzzleFlash 비활성화
        _muzzleFlash.enabled = false;
        _light.enabled = false;
    }
}
