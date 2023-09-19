using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private Transform m_hand;
    [SerializeField] private float m_range = 1.5f;
    [SerializeField] private LayerMask m_monstersLayerMask;

    private void Update()
    {
        if (GameManagement.Instance.GameIsOver()) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider2D[] monstersInRange = Physics2D.OverlapCircleAll(m_hand.position, m_range, m_monstersLayerMask);
            for (int i = 0; i < monstersInRange.Length; i++)
            {
                GameObject monster = monstersInRange[i].gameObject;
                monster.GetComponent<Monster>().LeaveRoom();
                Destroy(monster);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_hand.position, m_range);
    }
}
