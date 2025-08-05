using UnityEngine;
using System.Collections;

public class SocialManager : MonoBehaviour {

    public string rateUsURL;
    public string moreGamesURL;

    string subject = "Subject text";
    string body;

    public void OnAndroidTextSharingClick()
    {
        body = "This is a great game! Download it soon " + rateUsURL;
        StartCoroutine(ShareAndroidText());
    }

    IEnumerator ShareAndroidText()
    {
        yield return new WaitForEndOfFrame();
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
        currentActivity.Call("startActivity", jChooser);
    }


    public void RateUs()
    {

        Application.OpenURL(rateUsURL);
    }

    public void MoreGames()
    {
        Application.OpenURL(moreGamesURL);
    }


}
