using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private Transform m_hand;
    [SerializeField] private float m_range = 1.5f;
    [SerializeField] private float m_suctionForce = 1f;
    [SerializeField] private float m_minDistance = 1f;
    //[SerializeField] private float m_rangeH = 1.5f;
    [SerializeField] private LayerMask m_monstersLayerMask;

    private void Update()
    {
        if (!GameManagement.Instance.IsPlayable()) return;

        if (Input.GetMouseButton(0))
        {
            Collider2D[] monstersInRange = Physics2D.OverlapCircleAll(m_hand.position, m_range, m_monstersLayerMask);
            for (int i = 0; i < monstersInRange.Length; i++)
            {
                GameObject monsterObj = monstersInRange[i].gameObject;
                Monster monster = monsterObj.GetComponent<Monster>();
                if (!monster.IsAlive()) continue;

                if (Vector2.Distance(m_hand.position, monsterObj.transform.position) > m_minDistance)
                {
                    // Atract monster
                    Vector2 direction = (m_hand.transform.position - monsterObj.transform.position).normalized;
                    if ((direction.x < 0 && transform.localScale.x == 1f) || (direction.x > 0 && transform.localScale.x == -1f))
                    {
                        monsterObj.transform.position += (Vector3)direction * m_suctionForce * Time.deltaTime;
                    }
                }
                else
                {
                    // Destroy monster
                    monster.Die(m_hand);
                }
            }
        }
    }

    public void AddVacuumUpgrade()
    {
        m_suctionForce *= 1.5f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_hand.position, m_range);
        //Vector3[] points = new Vector3[4]
        //{
        //    m_hand.position,
        //    m_hand.position + Vector3.right * m_range + new Vector3(0, 1f, 0f) * m_rangeH,
        //    m_hand.position,
        //    m_hand.position + Vector3.right * m_range - new Vector3(0, 1f, 0f) * m_rangeH
        //};
        //Gizmos.DrawLineList(points);
    }
}
