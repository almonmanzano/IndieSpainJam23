using TMPro;
using UnityEngine;

public class UI_WinScreen : MonoBehaviour
{
    [SerializeField] private GameObject[] m_starImages;
    [SerializeField] private TMP_Text m_text;

    private void Start()
    {
        UpdateStars();
    }

    private void UpdateStars()
    {
        int stars = GameManagement.Instance.GetStars();
        for (int i = 0; i < m_starImages.Length; i++)
        {
            m_starImages[i].SetActive(i < stars ? true : false);
        }

        if (stars == 5)
        {
            m_text.text = "¡Enhorabuena! Quizás la próxima vez te pague las horas extra.";
        }
        else if (stars == 4)
        {
            m_text.text = "Veo que ya sabes lo que hace falta para ayudar en casa con la limpieza.";
        }
        else if (stars == 3)
        {
            m_text.text = "¿Quieres que te felicite por \"solo\" haber perdido 2 clientes? Wooow qué buen trabajador eres...";
        }
        else if (stars == 2)
        {
            m_text.text = "Si ser mediocre fuera un deporte olímpico, ganarías el oro, la plata y el bronce.";
        }
        else if (stars == 1)
        {
            m_text.text = "Este resultado nos humilla a ambos. Menos mal que no pensaba pagarte.";
        }
    }
}
