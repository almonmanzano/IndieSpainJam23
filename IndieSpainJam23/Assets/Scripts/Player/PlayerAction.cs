using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private Transform m_hand;
    [SerializeField] private float m_range = 1.5f;
    [SerializeField] private float m_suctionForce = 1f;
    [SerializeField] private float m_minDistance = 1f;
    //[SerializeField] private float m_rangeH = 1.5f;
    [SerializeField] private LayerMask m_monstersLayerMask;

    [SerializeField] private ParticleSystem m_vacuumFX1;
    [SerializeField] private ParticleSystem m_vacuumFX2;
    [SerializeField] private float m_particlesSpeed = 7f;
    [SerializeField] private float m_particlesError = 0.5f;

    private ParticleSystem.Particle[] m_vacuumParticles1;
    private ParticleSystem.Particle[] m_vacuumParticles2;

    private AudioSource m_vacuumSFX;

    private void Update()
    {
        if (!GameManagement.Instance.IsPlayable()) return;

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            // Audio
            if (!m_vacuumSFX.isPlaying) m_vacuumSFX.Play();

            m_animator.SetBool("IsAbsorbing", true);
            Collider2D[] monstersInRange = Physics2D.OverlapCircleAll(m_hand.position, m_range, m_monstersLayerMask);
            for (int i = 0; i < monstersInRange.Length; i++)
            {
                GameObject monsterObj = monstersInRange[i].gameObject;
                Monster monster = monsterObj.GetComponent<Monster>();
                if (!monster.IsAlive()) continue;

                if (Vector2.Distance(m_hand.position, monsterObj.transform.position) > m_minDistance)
                {
                    float x = m_hand.position.x - monsterObj.transform.position.x;
                    if ((x > 0f && transform.localScale.x < 0f) || (x < 0f && transform.localScale.x > 0f))
                    {
                        monster.GetAttracted(m_hand, m_suctionForce);
                    }
                }
                else
                {
                    // Destroy monster
                    monster.Die(m_hand);
                }
            }

            // Particle system
            UpdateFX(m_vacuumFX1, m_vacuumParticles1);
            UpdateFX(m_vacuumFX2, m_vacuumParticles2);
        }
        else
        {
            // Audio
            m_vacuumSFX.Stop();

            m_animator.SetBool("IsAbsorbing", false);
            m_vacuumFX1.gameObject.SetActive(false);
            m_vacuumFX2.gameObject.SetActive(false);
        }
    }

    private void UpdateFX(ParticleSystem fx, ParticleSystem.Particle[] particles)
    {
        fx.gameObject.SetActive(true);

        if (particles == null || particles.Length < fx.main.maxParticles)
        {
            particles = new ParticleSystem.Particle[fx.main.maxParticles];
        }

        int numParticles = fx.GetParticles(particles);

        for (int i = 0; i < numParticles; i++)
        {
            float y = m_hand.position.y - particles[i].position.y;
            float xmax = m_hand.position.x - fx.transform.position.x;
            float x = m_hand.position.x - particles[i].position.x;
            if ((x > 0f && transform.localScale.x > 0f) || (x < 0f && transform.localScale.x < 0f))
            {
                particles[i].position += Vector3.right * 2 * x;
                particles[i].velocity *= -1f;
            }
            float speed = m_particlesSpeed * (1f - (x / xmax));
            particles[i].position += Vector3.up * y * speed * Time.deltaTime;
            if (Vector2.Distance(m_hand.position, particles[i].position) < m_particlesError)
            {
                particles[i].position = m_hand.position;
            }
        }

        fx.SetParticles(particles, numParticles);
    }

    public void SetVacuumSFX(AudioSource audioSource)
    {
        m_vacuumSFX = audioSource;
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
