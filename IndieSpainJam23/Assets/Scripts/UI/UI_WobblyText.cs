using TMPro;
using UnityEngine;

public class UI_WobblyText : MonoBehaviour
{
    [SerializeField] private bool m_animate = false;
    [SerializeField] private Vector2 m_charIndexRange;
    [SerializeField] private float m_timeMultiplier = 7f;
    [SerializeField] private float m_waveWidth = 0.07f;
    [SerializeField] private float m_waveHeight = 5f;

    private TMP_Text m_textComponent;

    private void Awake()
    {
        m_textComponent = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (!m_animate) return;

        m_textComponent.ForceMeshUpdate();
        var textInfo = m_textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            if (i < m_charIndexRange.x || i > m_charIndexRange.y)
            {
                continue;
            }

            var charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
            {
                continue;
            }

            var meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];

            for (int j = 0; j < 4; ++j)
            {
                var index = charInfo.vertexIndex + j;
                var orig = meshInfo.vertices[index];
                meshInfo.vertices[index] = orig + new Vector3(0f, Mathf.Sin(Time.time * m_timeMultiplier + orig.x * m_waveWidth) * m_waveHeight, 0f);
            }

        }

        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            meshInfo.mesh.colors32 = meshInfo.colors32;
            m_textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    public void SetAnimatedRange(bool animate, Vector2 range)
    {
        m_animate = animate;
        m_charIndexRange = range;
    }
}
