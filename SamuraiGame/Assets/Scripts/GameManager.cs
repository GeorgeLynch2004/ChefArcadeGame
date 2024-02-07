using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool m_GameRunning;
    [SerializeField] private int m_GameDifficulty;
    [SerializeField] private bool m_AIInUse;
    [SerializeField] private List<EnemySpawner> m_EnemySpawners;
    [SerializeField] private AIBrain[] m_NumberOfTotalAliveAI;
    [SerializeField] private List<AIBrain> m_NumberOfAliveFriendlyAI;
    [SerializeField] private List<AIBrain> m_NumberOfAliveHostileAI;
    [SerializeField] private float m_Ratio;
    [SerializeField] private int m_NextFreeID;
    [SerializeField] private float m_GameTimer;
    [SerializeField] private float m_GameTimeLimit;
    [SerializeField] private float m_GameScore;
    [SerializeField] private int m_OrdersCompleted;
    [SerializeField] private UIManager m_UIManager;

    private void Start() {
        m_NextFreeID = 1;
        m_GameTimer = m_GameTimeLimit;
        m_GameRunning = true;
    }

    private void Update()
    {
        if (!m_GameRunning) return;

        if (m_AIInUse)
        {
            // Get all AI in the level
            m_NumberOfTotalAliveAI = FindObjectsOfType<AIBrain>();

            m_NumberOfAliveFriendlyAI = new List<AIBrain>();
            m_NumberOfAliveHostileAI = new List<AIBrain>();
            FilterAI();
            AssignIDs();

            try
            {
                m_Ratio = m_NumberOfAliveHostileAI.Count / m_NumberOfAliveFriendlyAI.Count;
            }
            catch (DivideByZeroException)
            {
                m_Ratio = m_GameDifficulty + .1f;
            }

            if (m_EnemySpawners != null)
            {
                DifficultySpawnDecision(m_Ratio);
            }
        }

        if (m_GameTimer <=  0)
        {
            StartCoroutine(LevelOver());
        }

        if (m_GameRunning)
        {
            m_GameTimer -= Time.deltaTime;
        }
    }

    private IEnumerator LevelOver()
    {
            m_GameRunning = false;
            m_GameTimer = m_GameTimeLimit;

            yield return new WaitForSeconds(2);

            //drop the UI and level stats
            m_UIManager.ToggleScreen("down");

            yield return new WaitForSeconds(2);

            m_UIManager.DisplayPostLevelStats("in");

            yield return new WaitForSeconds(3);

            m_UIManager.DisplayPostLevelStats("out");

            yield return new WaitForSeconds(1);

            SceneManager.LoadSceneAsync(0);
            
    }

    public int GetOrdersCompletedCount()
    {
        return m_OrdersCompleted;
    }

    public void IncreaseOrdersCompletedCount()
    {
        m_OrdersCompleted++;
    }

    public float GetCurrentTimer()
    {
        return m_GameTimer;
    }

    public void ChangeScore(float change)
    {
        m_GameScore += change;
    }

    public float GetMaxTimer()
    {
        return m_GameTimeLimit;
    }

    public float GetCurrentScore()
    {
        return m_GameScore;
    }

    public bool GameRunning()
    {
        return m_GameRunning;
    }

    public int GetGameDifficulty()
    {
        return m_GameDifficulty;
    }

    private void FilterAI()
    {
        // Filter into good and bad AI
        foreach (AIBrain aiBrain in m_NumberOfTotalAliveAI)
        {
            if (aiBrain.IsHostile())
            {
                m_NumberOfAliveHostileAI.Add(aiBrain);
            }
            else
            {
                m_NumberOfAliveFriendlyAI.Add(aiBrain);            
            }
        }
    }

    private void AssignIDs()
    {
        foreach (AIBrain samurai in m_NumberOfAliveFriendlyAI)
        {
            if (samurai.ID == 0)
            {
                samurai.ID = m_NextFreeID;
                m_NextFreeID++;
            }
        }
    }

    public int GetNextFreeID()
    {
        return m_NextFreeID;
    }

    public void IncreaseNextFreeID()
    {
        m_NextFreeID++;
    }

    private void DifficultySpawnDecision(float ratio)
    {
        if (m_GameDifficulty > ratio)
        {
            int numOfSpawners = m_EnemySpawners.Count;
            int randomIndex = Random.Range(0, numOfSpawners);
            m_EnemySpawners[randomIndex].SpawnEnemy();
        }
    }
}
