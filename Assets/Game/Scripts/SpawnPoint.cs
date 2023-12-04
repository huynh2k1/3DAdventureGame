using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Character enemyPrefab;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position + new Vector3(0f, 0.5f, 0f); ;
        Gizmos.DrawWireCube(center, Vector3.one);
        Gizmos.DrawLine(center, center + transform.forward * 2);

    }
}
