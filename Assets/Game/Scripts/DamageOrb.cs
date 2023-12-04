using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOrb : MonoBehaviour
{
    public float speed = 2f;
    public int damage = 10;
    public ParticleSystem HitVFX;
    public Rigidbody _rb;

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        Character cc = other.gameObject.GetComponent<Character>();

        if (cc != null && cc.isPlayer)
        {
            cc.ApplyDamage(damage, transform.position);
        }
        Instantiate(HitVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
