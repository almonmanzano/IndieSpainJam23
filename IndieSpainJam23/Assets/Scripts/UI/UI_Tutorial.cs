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

    [SerializeField] private AudioSource m_textAudioSource;
    [SerializeField] private AudioClip[] m_textClips;

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
    private bool m_waitingForMap = true;
    private IEnumerator m_typeLineCoroutine;

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
                if (m_index == 3 && m_waitingForMap) return;
                NextLine();
            }
            else
            {
                StopCoroutine(m_typeLineCoroutine);
                m_textComponent.text = m_lines[m_index].text;
                EndLine();
            }
        }
    }

    private void StartDialogue()
    {
        m_index = 0;
        m_typeLineCoroutine = TypeLine();
        StartCoroutine(m_typeLineCoroutine);
    }

    private IEnumerator TypeLine()
    {
        // Map
        if (m_index == 3)
        {
            StartCoroutine(WaitForMap());
        }

        m_wobblyText.SetAnimatedRange(m_lines[m_index].animate, m_lines[m_index].animatedRange);
        char[] text = m_lines[m_index].text.ToCharArray();
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            m_textComponent.text += c;
            if (c == '<' || (i > 0 && text[i - 1] == '<') || (i < text.Length - 1 && text[i + 1] == '>') || c == '>') continue;

            if (!m_textAudioSource.isPlaying)
            {
                int rand = Random.Range(0, m_textClips.Length);
                m_textAudioSource.clip = m_textClips[rand];
                m_textAudioSource.Play();
            }
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
            m_typeLineCoroutine = TypeLine();
            StartCoroutine(m_typeLineCoroutine);
        }
        else
        {
            Gameflow.Instance.StartFirstNight();
            gameObject.SetActive(false);
        }
    }

    private IEnumerator WaitForMap()
    {
        yield return new WaitForSeconds(1f);
        m_waitingForMap = false;
    }
}
