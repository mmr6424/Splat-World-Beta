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

    // Start is called before the first frame update
    void Start()
    {
        panelLocation = transform.position;
    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.y - data.position.y;
        transform.position = panelLocation - new Vector3(0, difference, 0);
    }

    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.y - data.position.y) / Screen.height;
        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector3 newLocation = panelLocation;
            if (percentage < 0)
            {
                newLocation += new Vector3(0, Screen.height, 0);
                //profilePageActive = false;
            }
            else if (percentage > 0)
            {
                newLocation += new Vector3(0, -Screen.height, 0);
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

    public void SetPage()
    {
        Pages page = Pages.Profile;
        Vector3 newLocation = panelLocation;
        switch (page) {
            case Pages.Landing:
                newLocation = new Vector3(Screen.width / 2, -Screen.height, 0);
                break;
            case Pages.Profile:
                newLocation = new Vector2(Screen.width / 2, Screen.height * 2);
                break;
        }
        StartCoroutine(SmoothMove(transform.position, newLocation, easing));
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
