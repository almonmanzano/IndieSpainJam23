using UnityEngine;

public class HotelManager : MonoBehaviour
{
    public static HotelManager Instance { get; private set; }

    [SerializeField] private GameObject[] m_TVs;
    [SerializeField] private GameObject[] m_bars;

    [SerializeField] private int m_normalRoomBenefit = 10;
    [SerializeField] private int m_extraBenefitsTV = 20;
    [SerializeField] private int m_extraBenefitsBar = 30;

    private int[] m_roomBenefits = new int[3];

    private int m_money = 0;

    private bool[] m_simpas = new bool[5] { false, false, false, false, false };

    // Hotel upgrades
    private int m_speedIncrease = 0;
    private bool m_dash = false;
    private bool m_vacuumUpgrade = false;

    // Rooms upgrades
    private bool[] m_hasTV;
    private bool[] m_hasBar;

    private GameObject m_player;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_hasTV = new bool[5];
        m_hasBar = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            m_hasTV[i] = false;
            m_hasBar[i] = false;
            m_TVs[i].SetActive(false);
            m_bars[i].SetActive(false);
        }

        m_roomBenefits[0] = m_normalRoomBenefit;
        m_roomBenefits[1] = m_extraBenefitsTV;
        m_roomBenefits[2] = m_extraBenefitsBar;
    }

    public void SetPlayer(GameObject player)
    {
        m_player = player;
    }

    public void AddSimpa(int roomID)
    {
        m_simpas[roomID] = true;
    }

    public void ResetSimpas()
    {
        m_simpas = new bool[5] { false, false, false, false, false };
    }

    public int GetSimpas()
    {
        int simpas = 0;
        foreach (bool b in m_simpas)
        {
            if (b) simpas++;
        }
        return simpas;
    }

    public int GetRoomBenefits(int roomID)
    {
        if (m_simpas[roomID]) return 0;

        int benefits = m_roomBenefits[0];
        if (m_hasTV[roomID]) benefits += m_roomBenefits[1];
        if (m_hasBar[roomID]) benefits += m_roomBenefits[2];
        return benefits;
    }

    public int GetMoney()
    {
        return m_money;
    }

    public void AddMoney(int money)
    {
        m_money += money;
    }

    public bool BuyUpgrade(Upgrade.UpgradeType type, int roomID, int cost)
    {
        m_money -= cost;

        if (type == Upgrade.UpgradeType.Speed)
        {
            AddSpeedIncrease();
            if (m_speedIncrease < 2)
            {
                return false;
            }
        }
        else if (type == Upgrade.UpgradeType.Dash)
        {
            AddDash();
        }
        else if (type == Upgrade.UpgradeType.Vacuum)
        {
            AddVacuumUpgrade();
        }
        else if (type == Upgrade.UpgradeType.TV)
        {
            AddTV(roomID - 1);
        }
        else if (type == Upgrade.UpgradeType.Bar)
        {
            AddBar(roomID - 1);
        }

        return true;
    }

    private void AddSpeedIncrease()
    {
        if (m_speedIncrease == 2) return;

        m_speedIncrease += 1;
        m_player.GetComponent<PlayerMovement>().AddSpeed();
    }

    private void AddDash()
    {
        if (m_dash) return;

        m_dash = true;
        m_player.GetComponent<PlayerMovement>().AddDash();
    }

    private void AddVacuumUpgrade()
    {
        if (m_vacuumUpgrade) return;

        m_vacuumUpgrade = true;
        m_player.GetComponent<PlayerAction>().AddVacuumUpgrade();
    }

    private void AddTV(int roomID)
    {
        m_hasTV[roomID] = true;
        m_TVs[roomID].SetActive(true);
        HauntedRoom room = Gameflow.Instance.GetHauntedRoom(roomID);
        room.GetGuest().IncreaseRestore();
    }

    private void AddBar(int roomID)
    {
        m_hasBar[roomID] = true;
        m_bars[roomID].SetActive(true);
        HauntedRoom room = Gameflow.Instance.GetHauntedRoom(roomID);
        room.GetGuest().RelaxFear();
    }
}
