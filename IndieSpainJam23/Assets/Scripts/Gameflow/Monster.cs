using System.Collections;
using System.Threading;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float m_deathTime = 0.5f;
    [SerializeField] private GameObject m_vfx;

    private HauntedRoom m_room;
    private bool m_alive = true;
    private Transform[] m_patrolPoints;

    public void SetHauntedRoom(HauntedRoom room, Transform[] patrolPoints)
    {
        m_room = room;
        m_patrolPoints = patrolPoints;
    }

    public void LeaveRoom()
    {
        m_room.StopHaunted();
    }

    public bool IsAlive()
    {
        return m_alive;
    }

    public void Die(Transform transf)
    {
        m_alive = false;
        GetComponent<Animator>().SetTrigger("Die");
        LeaveRoom();
        Destroy(GetComponent<Collider2D>()); // Bug or feature?

        StartCoroutine(DieCoroutine(transf, m_deathTime));
    }

    private IEnumerator DieCoroutine(Transform transf, float t)
    {
        float elapsedTime = 0f;
        Vector2 startingPos = transform.position;
        Vector2 direction = (startingPos - (Vector2)transf.position).normalized;
        while (elapsedTime < t)
        {
            transform.position = Vector2.Lerp(startingPos, transf.position, elapsedTime / t);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = transf.position;
        Instantiate(m_vfx, transf.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
