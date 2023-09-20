using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float m_seconds;

    private void Start()
    {
        Destroy(gameObject, m_seconds);
    }
}
