using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HotelUpgrades : MonoBehaviour
{
    [System.Serializable]
    public struct UpgradeButton
    {
        public Button Button;
        public TMP_Text Text;
        public Upgrade Upgrade;
    }

    [SerializeField] private TMP_Text m_moneyText;
    [SerializeField] private UpgradeButton[] m_upgradesHotel;
    [SerializeField] private UpgradeButton[] m_upgradesRoom1;
    [SerializeField] private UpgradeButton[] m_upgradesRoom2;
    [SerializeField] private UpgradeButton[] m_upgradesRoom3;
    [SerializeField] private UpgradeButton[] m_upgradesRoom4;
    [SerializeField] private UpgradeButton[] m_upgradesRoom5;

    private void Start()
    {
        UpdateAllUpgrades();
    }

    public void UpdateMoneyText()
    {
        m_moneyText.text = "Ahorrado: " + HotelManager.Instance.GetMoney();
    }

    [ContextMenu("Update All Upgrades")]
    public void UpdateAllUpgrades()
    {
        foreach (UpgradeButton upgrade in m_upgradesHotel)
        {
            UpdateUpgrade(upgrade, 0);
        }
        foreach (UpgradeButton upgrade in m_upgradesRoom1)
        {
            UpdateUpgrade(upgrade, 1);
        }
        foreach (UpgradeButton upgrade in m_upgradesRoom2)
        {
            UpdateUpgrade(upgrade, 2);
        }
        foreach (UpgradeButton upgrade in m_upgradesRoom3)
        {
            UpdateUpgrade(upgrade, 3);
        }
        foreach (UpgradeButton upgrade in m_upgradesRoom4)
        {
            UpdateUpgrade(upgrade, 4);
        }
        foreach (UpgradeButton upgrade in m_upgradesRoom5)
        {
            UpdateUpgrade(upgrade, 5);
        }
    }

    private void UpdateUpgrade(UpgradeButton upgrade, int roomID)
    {
        int cost = upgrade.Upgrade.Cost;
        upgrade.Text.text = upgrade.Upgrade.Text + "<br><size=100%>- " + cost + " -";
        upgrade.Button.onClick.AddListener(() =>
        {
            BuyUpgrade(upgrade, roomID, cost);
        });
    }

    private void BuyUpgrade(UpgradeButton upgrade, int roomID, int cost)
    {
        HotelManager hotel = HotelManager.Instance;
        int money = hotel.GetMoney();
        if (money >= cost)
        {
            hotel.BuyUpgrade(upgrade.Upgrade.Type, roomID, cost);
            upgrade.Button.interactable = false;
            UpdateMoneyText();
        }
    }
}
