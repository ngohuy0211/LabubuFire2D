using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Facebook.Unity;
using Firebase.Auth;
using Firebase.Extensions;

public class FirebaseManager : SingletonFreeAlive<FirebaseManager>
{
    private string _googleAPI = "277892368516-h1b54mc9nkjcqokcphgkif1dtinksf90.apps.googleusercontent.com";
    private GoogleSignInConfiguration _configuration;
    private Firebase.Auth.FirebaseAuth _auth;
    private Firebase.Auth.FirebaseUser _user;
    private bool _isGoogleSignInInitialized = false;

    public System.Action<UserModel> LoginDoneCb;
    
    protected override void OnAwake()
    {
        InitFirebase();
        //
        if (!FB.IsInitialized) FB.Init(InitCallback, OnHideUnity);
        else FB.ActivateApp();
    }
    
    private void InitFirebase() => _auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    public void LogOut() => _auth.SignOut();
    
    #region Login Google

    public void LoginWithGoogle()
    {
        if (!_isGoogleSignInInitialized)
        {
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                RequestIdToken = true,
                WebClientId = _googleAPI,
                RequestEmail = true
            };

            _isGoogleSignInInitialized = true;
        }
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            WebClientId = _googleAPI
        };
        GoogleSignIn.Configuration.RequestEmail = true;

        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
        signIn.ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                signInCompleted.SetCanceled();
                Debug.Log("Cancelled");
            }
            else if (task.IsFaulted)
            {
                signInCompleted.SetException(task.Exception);

                Debug.Log("Faulted " + task.Exception);
            }
            else
            {
                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                _auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
                {
                    if (authTask.IsCanceled)
                    {
                        signInCompleted.SetCanceled();
                    }
                    else if (authTask.IsFaulted)
                    {
                        signInCompleted.SetException(authTask.Exception);
                        Debug.Log("Faulted In Auth " + task.Exception);
                    }
                    else
                    {
                        signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
                        _user = _auth.CurrentUser;
                        Debug.LogFormat("User signed in successfully: {0} ({1})",
                            _user.DisplayName, _user.UserId);
                        
                        GameContext.Instance.UserModel.UserId = _user.UserId;
                        GameContext.Instance.UserModel.UserDisplayName = _user.DisplayName;
                        GameContext.Instance.UserModel.UserEmail = _user.Email;
                        GameContext.Instance.UserModel.UserUrlAvatar = _user.PhotoUrl.ToString();

                    }
                });
            }
        });
    }

    #endregion

    #region Login Facebook
    
    public void LoginWithFb()
    {
        Debug.Log("Login Facebook Called");
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
        _auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
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
            _user = result.User;

            GameContext.Instance.UserModel.UserId = _user.UserId;
            GameContext.Instance.UserModel.UserDisplayName = _user.DisplayName;
            GameContext.Instance.UserModel.UserEmail = _user.Email;
            GameContext.Instance.UserModel.UserUrlAvatar = _user.PhotoUrl.ToString();
        });
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

    #endregion
    
    #region Load Avatar

    public void LoadImage(Image img, string imageUri)
    {
        StartCoroutine(DelayLoadImage(img, imageUri));
    }

    IEnumerator DelayLoadImage(Image img, string imageUri)
    {
        string newUrl = !string.IsNullOrEmpty(imageUri) ? imageUri : "";
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(newUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            // Use the loaded texture here
            Debug.Log("Image loaded successfully");
            img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }
        else
        {
            Debug.Log("Error loading image: " + www.error);
        }
    }    

    #endregion
}
