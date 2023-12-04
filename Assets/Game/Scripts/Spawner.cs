using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    public List<SpawnPoint> spawnPointList;
    public List<Character> spawnedCharacterList;
    public bool hasSpawned;
    public BoxCollider boxCollider;
    public UnityEvent OnAllSpawnedCharacterEliminated;//tất cả nv sinh ra đã được loại bỏ



    private void Update()
    {
        if(!hasSpawned || spawnedCharacterList.Count == 0)
            return;

        bool allSpawnedAreDead = true;

        foreach(Character c in spawnedCharacterList)
        {
            if(c.CurrentState != CharacterState.Dead)
            {
                allSpawnedAreDead = false;
                break;
            }
        }
        if (allSpawnedAreDead)
        {
            if (OnAllSpawnedCharacterEliminated != null)
            {
                OnAllSpawnedCharacterEliminated.Invoke();
            }

            spawnedCharacterList.Clear();

        }
    }

    public void Spawn()
    {
        if (hasSpawned)
            return;
        hasSpawned = true;

        foreach(SpawnPoint point in spawnPointList)
        {
            if(point.enemyPrefab != null)
            {
                Character enemyObj = Instantiate(point.enemyPrefab, point.transform.position, point.transform.rotation);
                spawnedCharacterList.Add(enemyObj);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Spawn();
            boxCollider.enabled = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxCollider.bounds.size);
    }
}
