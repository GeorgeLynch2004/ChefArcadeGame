using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_Rigidbody2D;
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_MaxSpeed;
    [SerializeField] private Vector2 m_LastDirection;
    private GameManager m_GameManager;

    private void Start()
    {
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (!m_GameManager.GameRunning()) return;

        Vector2 velocity = m_Rigidbody2D.velocity;

        if (Input.GetKey(KeyCode.W))
        {
            velocity += (Vector2.up * m_Speed * Time.deltaTime);
            m_LastDirection = Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity -= (Vector2.up * m_Speed * Time.deltaTime);
            m_LastDirection = -Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity -= (Vector2.right * m_Speed * Time.deltaTime);
            m_LastDirection = -Vector2.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity += (Vector2.right * m_Speed * Time.deltaTime);
            m_LastDirection = Vector2.right;
        }

        velocity.x = Mathf.Clamp(velocity.x, -m_MaxSpeed, m_MaxSpeed);
        velocity.y = Mathf.Clamp(velocity.y, -m_MaxSpeed, m_MaxSpeed);

        m_Rigidbody2D.velocity = velocity;
    }

    public Vector2 LastDirection()
    {
        return m_LastDirection;
    }
}
