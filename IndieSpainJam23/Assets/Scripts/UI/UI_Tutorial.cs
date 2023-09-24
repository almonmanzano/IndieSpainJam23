using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject m_textGameObject;
    [SerializeField] private Line[] m_lines;
    [SerializeField] private float m_textSpeed;

    [SerializeField] private GameObject m_tranquilityHint;
    [SerializeField] private GameObject m_mapHint;
    [SerializeField] private GameObject m_clockHint;

    [System.Serializable]
    public struct Line
    {
        public string text;
        public bool animate;
        public Vector2 animatedRange;
    }

    private int m_index;
    private TextMeshProUGUI m_textComponent;
    private UI_WobblyText m_wobblyText;

    private void Awake()
    {
        m_textComponent = m_textGameObject.GetComponent<TextMeshProUGUI>();
        m_wobblyText = m_textGameObject.GetComponent<UI_WobblyText>();
    }

    private void Start()
    {
        m_textComponent.text = string.Empty;
        StartDialogue();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_textComponent.text == m_lines[m_index].text)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                m_textComponent.text = m_lines[m_index].text;
                EndLine();
            }
        }
    }

    private void StartDialogue()
    {
        m_index = 0;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        m_wobblyText.SetAnimatedRange(m_lines[m_index].animate, m_lines[m_index].animatedRange);
        char[] text = m_lines[m_index].text.ToCharArray();
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            m_textComponent.text += c;
            if (c == '<' || (i > 0 && text[i - 1] == '<') || c == '>') continue;
            yield return new WaitForSeconds(1f / m_textSpeed);
        }

        EndLine();
    }

    private void EndLine()
    {
        if (m_index == 1)
        {
            m_tranquilityHint.SetActive(true);
        }
        else if (m_index == 3)
        {
            m_mapHint.SetActive(true);
        }
        else if (m_index == 6)
        {
            m_clockHint.SetActive(true);
        }
    }

    private void NextLine()
    {
        m_tranquilityHint.SetActive(false);
        if (m_mapHint.activeInHierarchy)
        {
            m_mapHint.GetComponent<Animator>().SetTrigger("Out");
        }
        m_clockHint.SetActive(false);

        if (m_index < m_lines.Length - 1)
        {
            m_index++;
            m_textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            Gameflow.Instance.StartFirstNight();
            gameObject.SetActive(false);
        }
    }
}
