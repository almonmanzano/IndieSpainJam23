using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterSelection : MonoBehaviour
{
    [SerializeField] private Image[] m_image;
    [SerializeField] private Sprite m_selectedSprite;
    [SerializeField] private Sprite m_unselectedSprite;

    private void Start()
    {
        m_image[0].sprite = m_selectedSprite;
        m_image[1].sprite = m_unselectedSprite;
    }

    private void SelectCharacter(int id)
    {
        m_image[1 - id].sprite = m_unselectedSprite;
        m_image[id].sprite = m_selectedSprite;
        CharacterSelection.Instance.SelectCharacter(id);
    }

    [ContextMenu("Select Igor")]
    public void SelectIgor()
    {
        SelectCharacter(0);
    }

    [ContextMenu("Select Igorina")]
    public void SelectIgorina()
    {
        SelectCharacter(1);
    }
}
