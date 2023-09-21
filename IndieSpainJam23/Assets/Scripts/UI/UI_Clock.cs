using UnityEngine;
using UnityEngine.UI;

public class UI_Clock : MonoBehaviour
{
    public static UI_Clock Instance { get; private set; }

    [SerializeField] private Transform m_clockHandTransform;
    [SerializeField] private Image m_radialImage;
    
    private float m_secondsPerNight;
    private float m_night;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_secondsPerNight = Gameflow.Instance.GetNightDuration();
    }

    private void Update()
    {
        if (!GameManagement.Instance.IsPlayable()) return;

        m_night += Time.deltaTime / m_secondsPerNight;
        float dayNormalized = m_night % 1f;
        float rotationDegreesPerNight = 360f;
        m_clockHandTransform.eulerAngles = new Vector3(0f, 0f, -dayNormalized * rotationDegreesPerNight);
        m_radialImage.fillAmount = m_night;
    }

    public void Restart()
    {
        m_night = 0f;
    }
}
