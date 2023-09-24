using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenuAnimation : MonoBehaviour
{
    [SerializeField] private Transform m_ghostRangeMin;
    [SerializeField] private Transform m_ghostRangeMax;
    [SerializeField] private float m_tMin = 0.5f;
    [SerializeField] private float m_tMax = 2.5f;
    [SerializeField] private GameObject m_ghostPrefab;

    [SerializeField] private float m_secondsPerNightClock = 30f;
    [SerializeField] private float m_secondsPerNight1 = 30f;
    [SerializeField] private float m_secondsPerNight2 = 30f;
    [SerializeField] private float m_secondsPerNight3 = 30f;
    [SerializeField] private Transform m_clockHandTransform;
    [SerializeField] private Transform m_star1;
    [SerializeField] private Transform m_star2;
    [SerializeField] private Transform m_star3;
    
    private float m_nightClock;
    private float m_night1;
    private float m_night2;
    private float m_night3;

    private void Start()
    {
        StartCoroutine(SpawnGhost());
    }

    private void Update()
    {
        m_nightClock += Time.deltaTime / m_secondsPerNightClock;
        m_night1 += Time.deltaTime / m_secondsPerNight1;
        m_night2 += Time.deltaTime / m_secondsPerNight2;
        m_night3 += Time.deltaTime / m_secondsPerNight3;
        float dayNormalizedClock = m_nightClock % 1f;
        float dayNormalized1 = m_night1 % 1f;
        float dayNormalized2 = m_night2 % 1f;
        float dayNormalized3 = m_night3 % 1f;
        float rotationDegreesPerNight = 360f;
        m_clockHandTransform.eulerAngles = new Vector3(0f, 0f, -dayNormalizedClock * rotationDegreesPerNight);
        m_star1.eulerAngles = new Vector3(0f, 0f, -dayNormalized1 * rotationDegreesPerNight);
        m_star2.eulerAngles = new Vector3(0f, 0f, -dayNormalized2 * rotationDegreesPerNight);
        m_star3.eulerAngles = new Vector3(0f, 0f, -dayNormalized3 * rotationDegreesPerNight);

        float sin1 = Mathf.Sin(Time.time * 3f);
        float sin2 = Mathf.Sin(Time.time * 2.5f);
        float sin3 = Mathf.Sin(Time.time * 2f);
        m_star1.localScale = new Vector3(sin1, sin1, sin1) / 2f * 0.2f + Vector3.one * 0.9f;
        m_star2.localScale = new Vector3(sin2, sin2, sin2) / 2f * 0.2f + Vector3.one * 0.9f;
        m_star3.localScale = new Vector3(sin3, sin3, sin3) / 2f * 0.2f + Vector3.one * 0.9f;
    }

    private IEnumerator SpawnGhost()
    {
        float rand = Random.Range(m_tMin, m_tMax);
        yield return new WaitForSeconds(rand);

        float x = Random.Range(m_ghostRangeMin.position.x, m_ghostRangeMax.position.x);
        Vector3 pos = new Vector3(x, m_ghostPrefab.transform.position.y, 0f);
        Instantiate(m_ghostPrefab, pos, Quaternion.identity);

        StartCoroutine(SpawnGhost());
    }
}
