using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Facebook.Unity;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class FirebaseManager : SingletonFreeAlive<FirebaseManager>
{
    private string _googleAPI = "277892368516-h1b54mc9nkjcqokcphgkif1dtinksf90.apps.googleusercontent.com";
    private GoogleSignInConfiguration _configuration;
    private Firebase.Auth.FirebaseAuth _auth;
    private Firebase.Auth.FirebaseUser _user;
    private bool _isGoogleSignInInitialized = false;

    public System.Action LoginDoneCb;
    public System.Action RegisterDoneCb;
    public System.Action LogOutDoneCb;

    public void InitFirebase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                _auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                if (_auth.CurrentUser != null)
                    LogOut();
                
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                // Set a flag here to indicate whether Firebase is ready to use by your app.
                
                //Auto Signin
                // _auth.StateChanged += AuthStateChanged;
                // AuthStateChanged(this, null);
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        //
        if (!FB.IsInitialized) FB.Init(InitCallback, OnHideUnity);
        else FB.ActivateApp();
    }

    public void LogOut()
    {
        _auth.SignOut();
        _user = null;
        GameContext.Instance.ClearData();
        LogOutDoneCb?.Invoke();
    }

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
                
                ShowErrorMessage(task);
                
                Debug.Log("Faulted " + task.Exception);
            }
            else
            {
                Credential credential =
                    Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>) task).Result.IdToken,
                        null);
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
                        signInCompleted.SetResult(((Task<FirebaseUser>) authTask).Result);
                        _user = _auth.CurrentUser;
                        Debug.LogFormat("User signed in successfully: {0} ({1})",
                            _user.DisplayName, _user.UserId);

                        GameContext.Instance.UserModel.UserId = _user.UserId;
                        GameContext.Instance.UserModel.UserDisplayName = _user.DisplayName;
                        GameContext.Instance.UserModel.UserEmail = _user.Email;
                        GameContext.Instance.UserModel.UserUrlAvatar = _user.PhotoUrl.ToString();

                        LoginDoneCb?.Invoke();
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
                ShowErrorMessage(task);
                
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

            LoginDoneCb?.Invoke();
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

    #region Login Normal

    public void RegisterUser(string email, string password, string username)
    {
        _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                ShowErrorMessage(task);
                
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
            Toast.ShowUp("Register Successfully");
            RegisterDoneCb?.Invoke();
            UpdateUserProfile(username);
        });
    }

    public void LoginWithEmail(string email, string password)
    {
        _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                ShowErrorMessage(task);
                
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

            LoginDoneCb?.Invoke();
        });
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (_auth.CurrentUser != _user)
        {
            bool signedIn = _user != _auth.CurrentUser && _auth.CurrentUser != null
                                                       && _auth.CurrentUser.IsValid();
            if (!signedIn && _user != null)
                Debug.Log("Signed out " + _user.DisplayName + " - " + _user.UserId);

            _user = _auth.CurrentUser;
            if (signedIn)
                Debug.Log("Signed in " + _user.DisplayName + " - " + _user.UserId);
        }
    }

    public void ForgetPasswordSubmit(string email)
    {
        _auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                ShowErrorMessage(task);
                return;
            }
            
            Toast.ShowUp("Successfully to send email for reset password!");
        });
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _auth.StateChanged -= AuthStateChanged;
        _auth = null;
    }

    #endregion

    #region Action

    private void UpdateUserProfile(string userName)
    {
        Firebase.Auth.FirebaseUser user = _auth.CurrentUser;
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = userName,
                PhotoUrl = new System.Uri("https://dummyimage.com/300"),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }
            });
        }
    }


    private void ShowErrorMessage(Task task)
    {
        string strErr = "";
        foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
        {
            if (exception is not FirebaseException firebaseEx) return;
            var errorCode = (AuthError)firebaseEx.ErrorCode;
            strErr += GetErrorMessage(errorCode);
        }

        Toast.ShowUp(strErr);
    }
    private string GetErrorMessage(AuthError errorCode)
    {
        var message = errorCode switch
        {
            AuthError.AccountExistsWithDifferentCredentials => "The account already exists with different credentials",
            AuthError.MissingPassword => "Password is missing",
            AuthError.WeakPassword => "The password is weak",
            AuthError.WrongPassword => "The password is wrong",
            AuthError.EmailAlreadyInUse => "The account with that email already exists",
            AuthError.InvalidEmail => "Invalid email",
            AuthError.MissingEmail => "Email is required",
            _ => "An error occurred. Please recheck email or password"
        };
        return message;
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