using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GhostMainMenu : MonoBehaviour
{
    [SerializeField] private float m_yMax = 12f;
    [SerializeField] private float m_speedMin = 2f;
    [SerializeField] private float m_speedMax = 5f;
    [SerializeField] private float m_minScaleMultiplier = 0.3f;

    private float m_speed;

    private void Start()
    {
        m_speed = Random.Range(m_speedMin, m_speedMax);
        transform.localScale *= Random.Range(m_minScaleMultiplier, 1f);
    }

    private void Update()
    {
        transform.position += Vector3.up * m_speed * Time.deltaTime;
        if (transform.position.y > m_yMax)
        {
            Destroy(gameObject);
        }
    }
}
