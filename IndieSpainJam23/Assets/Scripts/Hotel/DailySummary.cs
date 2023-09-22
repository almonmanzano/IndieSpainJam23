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
        HotelManager hotel = HotelManager.Instance;
        int simpas = hotel.GetSimpas();
        int[] roomBenefits = new int[5];
        for (int i = 0; i < 5; i++)
        {
            roomBenefits[i] = hotel.GetRoomBenefits(i);
        }

        string simpaText = "";
        if (simpas == 0)
        {
            simpaText = "Todos los hu�spedes han pagado";
        }
        else if (simpas == 1)
        {
            simpaText = "1 hu�sped cobarde ha hecho un simpa";
        }
        else
        {
            simpaText = simpas + " hu�spedes cobardes han hecho un simpa";
        }

        int totalBenefits = 0;
        foreach (int benefit in roomBenefits)
        {
            totalBenefits += benefit;
        }
        string summary = "Dinerito ganado:\r\n" +
                         "\r\n" +
                         "Habitaci�n 1 .......... " + roomBenefits[0] + "\r\n" +
                         "Habitaci�n 2 .......... " + roomBenefits[1] + "\r\n" +
                         "Habitaci�n 3 .......... " + roomBenefits[2] + "\r\n" +
                         "Habitaci�n 4 .......... " + roomBenefits[3] + "\r\n" +
                         "Habitaci�n 5 .......... " + roomBenefits[4] + "\r\n" +
                         "\r\n" +
                         "Total ....................... " + totalBenefits;

        m_simpaText.text = simpaText;
        m_summaryText.text = summary;
    }
}