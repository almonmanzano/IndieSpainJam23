using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private float m_moveSpeed = 5f;
    [SerializeField] private bool m_dashIsEnabled = false;
    [SerializeField] private float m_dashSpeed = 25f;
    [SerializeField] private float m_dashDuration = 0.3f;
    [SerializeField] private float m_dashCd = 1f;
    [SerializeField] private GameObject m_dashFX;
    [SerializeField] private float m_timeBetweenStepsSFX = 0.5f;
    [SerializeField] private AudioClip[] m_stepsAudioClips;
    [SerializeField] private ParticleSystem m_movementFX;
    [SerializeField] private float m_timeBetweenMovementFX = 0.2f;

    private Rigidbody2D m_rb;
    private TrailRenderer m_trailRenderer;
    private Vector2 m_initialPosition;

    private Vector2 m_movement;
    private bool m_isDashing = false;
    private float m_dashTime = 0f;
    private float m_timeSinceDash;

    private float m_timeSinceStep = 0f;

    private AudioSource m_stepsAudioSource;

    private float m_timeSinceMovementFX = 0f;

    private void Start()
    {
        InitializeVariables();

        HotelManager.Instance.SetPlayer(gameObject);
    }

    private void InitializeVariables()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_trailRenderer = GetComponent<TrailRenderer>();
        m_timeSinceDash = m_dashCd;
        m_initialPosition = transform.position;
        m_timeSinceStep = m_timeBetweenStepsSFX;
    }

    private void Update()
    {
        if (!GameManagement.Instance.IsPlayable())
            return;

        // Audio
        UpdateAudio();

        // Movement FX
        UpdateMovementFX();

        // Movement
        UpdateMovementDirection();

        // Animation
        m_animator.SetBool("IsRunning", m_movement != Vector2.zero);

        // Dash
        UpdateDash();
    }

    private void FixedUpdate()
    {
        if (!GameManagement.Instance.IsPlayable())
        {
            m_rb.velocity = Vector2.zero;
            return;
        }

        // Movement
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        float speed = m_isDashing ? m_dashSpeed : m_moveSpeed;
        m_rb.velocity = m_movement * speed;
    }

    private void UpdateMovementDirection()
    {
        m_movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Flip
        if (!Mathf.Approximately(m_movement.x, 0f))
            transform.localScale = new Vector3(m_movement.x > 0f ? 1f : -1f, 1f, 1f);
    }

    private void UpdateDash()
    {
        if (!m_dashIsEnabled)
            return;

        if (!m_isDashing)
        {
            m_timeSinceDash += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && CanDash())
            {
                StartDash();
            }
        }
        else
        {
            m_dashTime += Time.deltaTime;

            if (m_dashTime >= m_dashDuration)
            {
                EndDash();
            }
        }
    }

    private bool CanDash()
    {
        return m_movement != Vector2.zero && m_timeSinceDash >= m_dashCd;
    }

    private void StartDash()
    {
        m_isDashing = true;
        m_dashTime = 0f;
        m_timeSinceDash = 0f;

        if (m_dashFX)
            Instantiate(m_dashFX, transform.position, transform.rotation);

        if (m_trailRenderer)
            m_trailRenderer.emitting = true;
    }

    private void EndDash()
    {
        m_isDashing = false;
        if (m_trailRenderer)
            m_trailRenderer.emitting = false;
    }

    private void UpdateMovementFX()
    {
        m_timeSinceMovementFX += Time.deltaTime;
        if (m_rb.velocity != Vector2.zero && m_timeSinceMovementFX > m_timeBetweenMovementFX)
        {
            m_timeSinceMovementFX = 0f;
            m_movementFX.Play();
        }
    }

    private void UpdateAudio()
    {
        m_timeSinceStep += Time.deltaTime;
        if (m_timeSinceStep > m_timeBetweenStepsSFX && m_rb.velocity != Vector2.zero)
        {
            m_timeSinceStep = 0f;
            PlayRandomAudio(m_stepsAudioClips);
        }
    }

    private void PlayRandomAudio(AudioClip[] clips)
    {
        int rand = Random.Range(0, clips.Length);
        m_stepsAudioSource.clip = clips[rand];
        m_stepsAudioSource.Play();
    }

    public void SetStepsAudioSource(AudioSource audioSource)
    {
        m_stepsAudioSource = audioSource;
    }

    public void Restart()
    {
        transform.position = m_initialPosition;
    }

    public void AddSpeed()
    {
        m_moveSpeed *= 1.2f;
        m_timeBetweenStepsSFX /= 1.2f;
    }

    public void AddDash()
    {
        m_dashIsEnabled = true;
    }

    public bool IsFlipped()
    {
        return m_movement.x <= 0f;
    }

    public void StopRunningAnimation()
    {
        m_animator.SetBool("IsRunning", false);
    }
}
