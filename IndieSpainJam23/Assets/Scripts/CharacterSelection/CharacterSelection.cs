using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public static CharacterSelection Instance { get; private set; }

    [SerializeField] private CharacterSelectedData m_data;

    [SerializeField] private GameObject[] m_prefabs;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectCharacter(int id)
    {
        m_data.Prefab = m_prefabs[id];
    }
}
