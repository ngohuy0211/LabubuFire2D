using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Facebook.Unity;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.Networking;

public class FirebaseFacebookLogin : MonoBehaviour
{
    public Text username;
    public Text email;
    public Image avatar;


    private Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    private string imageUrl;
    
    private void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void LoginWithFb()
    {
        Debug.Log("Login Called");
        var perms = new List<string>() {"public_profile", "email"};
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    private void AuthCallback(ILoginResult result)
    {
        Debug.Log(FB.IsLoggedIn);
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken.TokenString;
            
            FacebookAuth(aToken);
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    private void FacebookAuth(string accessToken)
    {
        Firebase.Auth.Credential credential =
            Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
        auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            user = result.User;
            username.text = user.DisplayName;
            email.text = user.Email;
            
            StartCoroutine(LoadImage(CheckImageUrl(user.PhotoUrl.ToString())));

        });
    }
    
    private string CheckImageUrl(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            return url;
        }
        return imageUrl;
    }

    IEnumerator LoadImage(string imageUri)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUri);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            // Use the loaded texture here
            Debug.Log("Image loaded successfully");
            avatar.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }
        else
        {
            Debug.Log("Error loading image: " + www.error);
        }
    }

    public void LogOut()
    {
        Debug.Log("LogOut Success!");
        auth.SignOut();
    }
}