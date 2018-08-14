using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManagerScript : MonoBehaviour {

//#if UNITY_IOS
//    private string gameId = "2739227";
//#elif UNITY_ANDROID
//    private string gameId = "2739537";
//#endif

    public float InitialShowAdPropability = 0.5f;

    private GameManagerScript MyGameManager = null;

    // Use this for initialization
    void Awake() {
        MyGameManager = GameManagerScript.Instance;
    }


    public void ShowRewardedAd()
    {
        float r = Random.Range(0.1f, 1f);
        if (r < 0.5f)
        {
            if (Advertisement.IsReady("video"))
            {//video ,rewardedVideo
                ShowOptions options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show("video", options);
            }
            else
                MyGameManager.PlayActions();
        }
        else
            MyGameManager.PlayActions();
    }


    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Well done!");
                break;
            case ShowResult.Skipped:
                Debug.Log("BAD BAD BAD BOY!!");
                break;
            case ShowResult.Failed:
                Debug.Log("Damn it!");
                break;
        }

        MyGameManager.PlayActions();
    }


}
