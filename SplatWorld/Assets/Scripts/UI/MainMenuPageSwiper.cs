using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Main Menu Swipe
/// Author: Thomas Martinez
/// Swipes between landing page and profile page
/// using code from https://www.youtube.com/watch?v=rjFgThTjLso
/// </summary>

// this script can (and should) be modified for movement between more panels (for example escaping settings panel and gallery panel),
// currently its halfway hardcoded for just the profile and landing page and only detects verticle swipes
public class MainMenuPageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    // current location of panel
    private Vector3 panelLocation;

    // threshold where a drag gesture will be considered a swipe
    [SerializeField]
    float percentThreshold = 0.2f;

    // degree to which the movement eases in and out
    [SerializeField]
    float easing = 0.5f;

    public enum Pages
    {
        Landing,
        Profile
    }

    [SerializeField]
    private Pages page;

    // Start is called before the first frame update
    void Start()
    {
        panelLocation = transform.position;
        page = Pages.Landing;
    }

    /// <summary>
    /// When drag gesture begins this function is called
    /// </summary>
    /// <param name="data">drag data</param>
    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.y - data.position.y;
        if ((difference > 0 && page == Pages.Profile) || (difference < 0 && page == Pages.Landing))
        {
            transform.position = panelLocation - new Vector3(0, difference, 0);
        }
    }

    /// <summary>
    /// when drag gesture ends this function is called
    /// </summary>
    /// <param name="data"></param>
    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.y - data.position.y) / Screen.height;
        // if the percent dragged is above the threshold to trigger a screen transition
        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector3 newLocation = panelLocation;

            // if swipe is upward and the current page is the landing
            if (percentage < 0 && page != Pages.Profile)
            {
                // set target location to profile
                newLocation += new Vector3(0, Screen.height, 0);
                // set page to profile
                page = Pages.Profile;
                //profilePageActive = false;
            }
            // if swipe is downwards and the current page is the profile
            else if (percentage > 0 && page != Pages.Landing)
            {
                // set target location to landing
                newLocation += new Vector3(0, -Screen.height, 0);
                // set page to landing
                page = Pages.Landing;
                //profilePageActive = true;
            }
            // transition screen to new page
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            // set new location
            panelLocation = newLocation;
        }
        // movement threshold not met
        else
        {
            // smoothly transition back to current screen
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }

    /// <summary>
    /// Smooth moves to profile position.
    /// This function is called by button presses
    /// </summary>
    public void ScrollToProfile()
    {
        //panelLocation = new Vector2(Screen.width / 2, (Screen.height / 2) * 3);
        panelLocation += new Vector3(0, Screen.height, 0);
        StartCoroutine(SmoothMove(transform.position, new Vector2(Screen.width / 2, (Screen.height / 2) * 3), easing));
        page = Pages.Profile;
    }

    /// <summary>
    /// Smooth moves to landing position.
    /// This function is called by button presses
    /// </summary>
    public void ScrollToLanding()
    {
        //panelLocation = new Vector3(Screen.width / 2, -Screen.height, 0);
        panelLocation += new Vector3(0, -Screen.height, 0);
        StartCoroutine(SmoothMove(transform.position, new Vector2(Screen.width / 2, Screen.height / 2), easing));
        page = Pages.Landing; 
    }

    /// <summary>
    /// smoothly transitions from one position to another over a period of time
    /// </summary>
    /// <param name="startPos">starting position</param>
    /// <param name="endPos">final position</param>
    /// <param name="seconds">the amount of time spent moving in seconds</param>
    /// <returns></returns>
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
