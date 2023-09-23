using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterSelection : MonoBehaviour
{
    [SerializeField] private Image[] m_image;
    [SerializeField] private Sprite m_selectedSprite;
    [SerializeField] private Sprite m_unselectedSprite;

    private int m_selected = 0;

    private void Start()
    {
        m_image[0].sprite = m_selectedSprite;
        m_image[1].sprite = m_unselectedSprite;
    }

    private void SelectCharacter(int id)
    {
        m_image[1 - id].sprite = m_unselectedSprite;
        m_image[1 - id].color = Color.white;
        m_image[id].sprite = m_selectedSprite;
        m_image[id].color = Color.white;
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