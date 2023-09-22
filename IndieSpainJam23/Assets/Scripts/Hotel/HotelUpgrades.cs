using TMPro;
using UnityEngine;

public class HotelUpgrades : MonoBehaviour
{
    [System.Serializable]
    public struct UpgradeButton
    {
        public TMP_Text Text;
        public Upgrade Upgrade;
    }

    [SerializeField] private UpgradeButton[] m_button;

    private void Start()
    {
        foreach (UpgradeButton button in m_button)
        {
            button.Text.text = button.Upgrade.Text;
        }
    }
}
