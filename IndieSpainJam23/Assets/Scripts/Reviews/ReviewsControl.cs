using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReviewsControl : MonoBehaviour
{
    public static ReviewsControl Instance { get; private set; }

    [SerializeField] private Review[] m_badReviews;
    [SerializeField] private Transform m_reviewsParent;
    [SerializeField] private GameObject m_reviewPrefab;

    private List<int> m_reviewsIDs = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        FillReviewsIDs();
    }

    private void FillReviewsIDs()
    {
        for (int i = 0; i < m_badReviews.Length; i++)
        {
            m_reviewsIDs.Add(i);
        }
    }

    public void SendReview(Sprite portrait)
    {
        int rand = Random.Range(0, m_reviewsIDs.Count);
        int reviewID = m_reviewsIDs[rand];
        m_reviewsIDs.RemoveAt(rand);
        if (m_reviewsIDs.Count == 0)
        {
            FillReviewsIDs();
        }

        GameObject reviewGameObject = Instantiate(m_reviewPrefab, m_reviewsParent);
        Review review = m_badReviews[reviewID];
        reviewGameObject.GetComponentInChildren<TMP_Text>().text = review.Text;
        Transform portraitParent = reviewGameObject.transform.Find("PortraitParent");
        if (portraitParent == null) print("no portrait parent");
        Transform portraitChild = portraitParent.Find("Portrait");
        if (portraitChild == null) print("no portrait child");
        portraitChild.GetComponent<Image>().sprite = portrait;
    }
}
