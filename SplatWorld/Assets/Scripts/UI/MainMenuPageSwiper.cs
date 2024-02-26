using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuPageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// Main Menu Swipe
    /// Swipes between landing page and profile page
    /// using code from https://www.youtube.com/watch?v=rjFgThTjLso
    /// </summary>

    private Vector3 panelLocation;
    [SerializeField]
    float percentThreshold = 0.2f;
    [SerializeField]
    float easing = 0.5f;

    public enum Pages
    {
        Landing,
        Profile
    }

    private Pages page;

    // Start is called before the first frame update
    void Start()
    {
        panelLocation = transform.position;
        page = Pages.Landing;
    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.y - data.position.y;
        if ((difference > 0 && page == Pages.Profile) || (difference < 0 && page == Pages.Landing))
        {
            transform.position = panelLocation - new Vector3(0, difference, 0);
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.y - data.position.y) / Screen.height;
        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector3 newLocation = panelLocation;
            if (percentage < 0 && page != Pages.Profile)
            {
                newLocation += new Vector3(0, Screen.height, 0);
                page = Pages.Profile;
                //profilePageActive = false;
            }
            else if (percentage > 0 && page != Pages.Landing)
            {
                newLocation += new Vector3(0, -Screen.height, 0);
                page = Pages.Landing;
                //profilePageActive = true;
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }

    public void ScrollToProfile()
    {
        panelLocation = new Vector2(Screen.width / 2, (Screen.height / 2) * 3);
        StartCoroutine(SmoothMove(transform.position, new Vector2(Screen.width / 2, (Screen.height / 2) * 3), easing));
    }

    public void ScrollToLanding()
    {
        panelLocation = new Vector3(Screen.width / 2, -Screen.height, 0);
        StartCoroutine(SmoothMove(transform.position, new Vector2(Screen.width / 2, Screen.height / 2), easing));
    }

    IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
