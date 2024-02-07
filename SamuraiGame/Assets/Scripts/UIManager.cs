using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text m_GameTimer;
    [SerializeField] private TMP_Text m_GameScore;
    [SerializeField] private TMP_Text m_OrdersCompleted;
    [SerializeField] private GameManager m_GameManager;
    [SerializeField] private Animator m_Screen;
    [SerializeField] private Animator m_PostLevelStats;

    private void Start()
    {
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ToggleScreen("up");
    }

    private void Update()
    {
        if (m_GameManager != null)
        {
            float time = m_GameManager.GetCurrentTimer();
            int minutes = Mathf.FloorToInt(time / 60); // Get the whole minutes
            int seconds = Mathf.FloorToInt(time % 60); // Get the remaining seconds
            // Convert minutes and seconds into a string format
            string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
            m_GameTimer.text = "Time: " + timerString;
            m_GameScore.text = "Score: " + m_GameManager.GetCurrentScore().ToString();
            m_OrdersCompleted.text = "Orders Completed: " + m_GameManager.GetOrdersCompletedCount().ToString();
        } 
        
    }

    public void ToggleScreen(string movement)
    {
        if (movement == "up")
        {
            m_Screen.SetTrigger("Up");
        }
        else if (movement == "down")
        {
            m_Screen.SetTrigger("Down");
        }
    }

    public void DisplayPostLevelStats(string movement)
    {
        if (movement == "in")
        {
            m_PostLevelStats.SetTrigger("StatsFadeIn");
        }
        else if (movement == "out")
        {
            m_PostLevelStats.SetTrigger("StatsFadeOut");
        }
    }
}
