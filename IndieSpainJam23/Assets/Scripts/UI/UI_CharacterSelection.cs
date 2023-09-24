using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterSelection : MonoBehaviour
{
    [SerializeField] private Image[] m_image;
    [SerializeField] private Sprite m_selectedSprite;
    [SerializeField] private Sprite m_unselectedSprite;

    [SerializeField] private Image[] m_button;
    [SerializeField] private Color m_selectedColor;
    [SerializeField] private Color m_unselectedColor;

    private int m_selected = 0;

    private void Start()
    {
        m_image[0].sprite = m_selectedSprite;
        m_image[1].sprite = m_unselectedSprite;

        m_button[0].color = m_selectedColor;
        m_button[1].color = m_unselectedColor;
    }

    private void SelectCharacter(int id)
    {
        m_image[1 - id].sprite = m_unselectedSprite;
        m_image[1 - id].color = Color.white;
        m_image[id].sprite = m_selectedSprite;
        m_image[id].color = Color.white;

        m_button[1 - id].color = m_unselectedColor;
        m_button[id].color = m_selectedColor;

        CharacterSelection.Instance.SelectCharacter(id);
    }

    public void OnHover(int id)
    {
        if (id == m_selected) return;

        m_image[id].sprite = m_selectedSprite;
        m_image[id].color = Color.gray;
    }

    public void ExitOver(int id)
    {
        if (id == m_selected) return;

        m_image[id].sprite = m_unselectedSprite;
        m_image[id].color = Color.white;
    }

    [ContextMenu("Select Igor")]
    public void SelectIgor()
    {
        m_selected = 0;
        SelectCharacter(0);
    }

    [ContextMenu("Select Igorina")]
    public void SelectIgorina()
    {
        m_selected = 1;
        SelectCharacter(1);
    }
}
