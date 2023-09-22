using TMPro;
using UnityEngine;

public class DailySummary : MonoBehaviour
{
    [SerializeField] private TMP_Text m_simpaText;
    [SerializeField] private TMP_Text m_summaryText;

    private void Start()
    {
        FillTexts();
    }

    public void FillTexts()
    {
        int simpas = 0;
        string simpaText = "";
        if (simpas == 0)
        {
            simpaText = "Todos los hu�spedes han pagado";
        }
        else if (simpas == 1)
        {
            simpaText = "1 hu�sped cobarde se ha hecho un simpa";
        }
        else
        {
            simpaText = simpas.ToString() + " hu�spedes cobardes se han hecho un simpa";
        }
        string summary = "Dinerito ganado:\r\n" +
                         "\r\n" +
                         "Habitaci�n 1 .......... 0\r\n" +
                         "Habitaci�n 2 .......... 0\r\n" +
                         "Habitaci�n 3 .......... 100\r\n" +
                         "Habitaci�n 3 .......... 150\r\n" +
                         "Habitaci�n 3 .......... 150\r\n" +
                         "\r\n" +
                         "Total ....................... 400";

        m_simpaText.text = simpaText;
        m_summaryText.text = summary;
    }
}
