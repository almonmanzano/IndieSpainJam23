using TMPro;
using UnityEngine;

public class DailySummary : MonoBehaviour
{
    [SerializeField] private TMP_Text m_simpaText;
    [SerializeField] private TMP_Text m_summaryText;

    public void FillTexts()
    {
        HotelManager hotel = HotelManager.Instance;
        int simpas = hotel.GetSimpas();
        int[] roomBenefits = new int[5];
        int totalBenefits = 0;
        for (int i = 0; i < 5; i++)
        {
            roomBenefits[i] = hotel.GetRoomBenefits(i);
            totalBenefits += roomBenefits[i];
        }
        hotel.AddMoney(totalBenefits);

        string simpaText = "";
        if (simpas == 0)
        {
            simpaText = "Todos los clientes han pagado";
        }
        else if (simpas == 1)
        {
            simpaText = "1 cliente cobarde ha hecho un simpa";
        }
        else
        {
            simpaText = simpas + " clientes cobardes han hecho un simpa";
        }

        string summary = "Dinerito ganado:\r\n" +
                         "\r\n" +
                         "Habitacion 1 .......... " + roomBenefits[0] + "\r\n" +
                         "Habitacion 2 .......... " + roomBenefits[1] + "\r\n" +
                         "Habitacion 3 .......... " + roomBenefits[2] + "\r\n" +
                         "Habitacion 4 .......... " + roomBenefits[3] + "\r\n" +
                         "Habitacion 5 .......... " + roomBenefits[4] + "\r\n" +
                         "\r\n" +
                         "Total ....................... " + totalBenefits;

        m_simpaText.text = simpaText;
        m_summaryText.text = summary;
    }
}
