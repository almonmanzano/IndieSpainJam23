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

    private Vector2 m_direction;
    private Vector2 m_lastPosition;

    private Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (m_lastPosition != Vector2.zero) {
            if (m_lastPosition != (Vector2)transform.position) {
                m_animator.SetBool("IsBeingAbsorbed", true);
                m_direction = (Vector2)transform.position - m_lastPosition;

                // flip sprite if direction changed
                if (m_direction.x < 0) {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                } else if (m_direction.x > 0) {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            } else {
                m_animator.SetBool("IsBeingAbsorbed", false);
            }
        }
        

        m_lastPosition = transform.position;
    }

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
        m_animator.SetTrigger("Die");
        LeaveRoom();
        Destroy(GetComponent<Collider2D>()); // Bug or feature?

        StartCoroutine(DieCoroutine(transf, m_deathTime));
    }

    public void GetAttracted(Transform transf, float suctionForce)
    {
        Vector2 direction = (transf.position - transform.position).normalized;
        transform.position += (Vector3)direction * suctionForce * Time.deltaTime;
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
