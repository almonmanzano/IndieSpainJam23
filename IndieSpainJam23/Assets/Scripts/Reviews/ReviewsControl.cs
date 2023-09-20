using TMPro;
using UnityEngine;

public class ReviewsControl : MonoBehaviour
{
    public static ReviewsControl Instance { get; private set; }

    [SerializeField] private Review[] m_badReviews;
    [SerializeField] private Transform m_reviewsParent;
    [SerializeField] private GameObject m_reviewPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void SendReview()
    {
        GameObject reviewGameObject = Instantiate(m_reviewPrefab, m_reviewsParent);
        int rand = Random.Range(0, m_badReviews.Length);
        Review review = m_badReviews[rand];
        reviewGameObject.GetComponentInChildren<TMP_Text>().text = review.Text;
    }
}
