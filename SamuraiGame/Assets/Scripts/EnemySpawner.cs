using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_EnemyPrefab;
    [SerializeField] private GameObject m_AIPool;

    public void SpawnEnemy()
    {
        Instantiate(m_EnemyPrefab, transform.position, transform.rotation, m_AIPool.transform);
    }
}
