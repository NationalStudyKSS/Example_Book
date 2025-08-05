using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletCtrl : MonoBehaviour
{
    // ÃÑ¾ËÀÇ ÆÄ±«·Â
    [SerializeField] float _damage = 20.0f;

    // ÃÑ¾Ë ¹ß»ç Èû
    [SerializeField] float _force = 1500.0f;

    [SerializeField] Rigidbody _rb;

    private void Start()
    {
        // Rigidbody ÄÄÆ÷³ÍÆ®¸¦ ÃßÃâ
        _rb = GetComponent<Rigidbody>();

        // Rigidbody¿¡ ÈûÀ» °¡ÇØ ÃÑ¾ËÀ» ¹ß»ç
        _rb.AddForce(transform.forward * _force);
    }
}
