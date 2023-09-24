using UnityEngine;
using UnityEngine.UI;

public class UI_MainMenuAnimation : MonoBehaviour
{
    [SerializeField] private float m_secondsPerNight = 30f;
    [SerializeField] private Transform m_clockHandTransform;
    
    private float m_night;

    private void Update()
    {
        m_night += Time.deltaTime / m_secondsPerNight;
        float dayNormalized = m_night % 1f;
        float rotationDegreesPerNight = 360f;
        m_clockHandTransform.eulerAngles = new Vector3(0f, 0f, -dayNormalized * rotationDegreesPerNight);
    }
}
