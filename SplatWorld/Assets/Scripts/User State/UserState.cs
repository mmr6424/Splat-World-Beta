using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserState : MonoBehaviour
{
    private UserInfo player;
    private List<CrewInfo> crews;
    private List<TagInfo> tags;
    private Stack<string> requestData;

    // for userinfo
    private Texture t;
    private Texture2D texture;

    //
    // getters and setters
    //
        
    public UserInfo Player
    {
        set
        {
            player = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // when a user logs in, this is used to get all the info
    public void LoadInfo()
    {
        player = JsonUtility.FromJson<UserInfo>(requestData.Pop());
    }

    // whenever we get request data, put it on top of the stack to be 
    // dealt with later
    public void PushRequestData(string data)
    {
        requestData.Push(data);
    }

    public void DownloadHeader()
    {
        StartCoroutine(GetHeader_Coroutine());
    }

    // starts get user pfp coroutine
    public void DownloadProfilePic()
    {
        StartCoroutine(GetPfp_Coroutine());
    }

    // gets user's pfp from server.
    IEnumerator GetPfp_Coroutine()
    {
        string url = player.DownloadPfpLink();

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url, false);
        www.SetRequestHeader("Accept", "image/*");
        yield return www.SendWebRequest();

        switch (www.result)
        {
            case UnityWebRequest.Result.Success:
                t = DownloadHandlerTexture.GetContent(www);

                if (t == null)
                {
                    Debug.Log("Profile Pic null");
                    break;
                }

                texture = t as Texture2D;

                player.ProfilePic.GetComponent<Image>().sprite = Sprite.Create(
                    texture,
                    new Rect(
                        0,
                        0,
                        texture.width,
                        texture.height
                    ),
                    new Vector2(0.5f, 0.5f));

                // add mask
                SpriteMask mask = player.ProfilePic.GetComponent<SpriteMask>();

                break;
            default:
                Debug.Log(www.error);
                break;
        }
    }

    // gets user's header from server
    IEnumerator GetHeader_Coroutine()
    {
        string url = player.DownloadHeaderLink();

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url, false);
        www.SetRequestHeader("Accept", "image/*");
        yield return www.SendWebRequest();

        switch (www.result)
        {
            case UnityWebRequest.Result.Success:
                t = DownloadHandlerTexture.GetContent(www);

                if (t == null)
                {
                    Debug.Log("Header image null");
                    break;
                }

                texture = t as Texture2D;

                player.Header.GetComponent<Image>().sprite = Sprite.Create(
                    texture,
                    new Rect(
                        0,
                        0,
                        texture.width,
                        texture.height
                    ),
                    new Vector2(0.5f, 0.5f));

                // add mask
                SpriteMask mask = player.Header.GetComponent<SpriteMask>();

                break;
            default:
                Debug.Log(www.error);
                break;
        }
    }

    //IEnumerator GetCrewMembers_Coroutine()
    //{
        
    //}
}