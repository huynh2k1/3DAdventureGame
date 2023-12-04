using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shooting : MonoBehaviour
{
    public Transform firePos;
    public GameObject dmgOrbPrefab;
    public Character cc;
    public void Shoot()
    {
        Instantiate(dmgOrbPrefab, firePos.position, Quaternion.LookRotation(firePos.forward));
    }

    private void Update()
    {
        cc.RotateToTarget();
        
    }

}
