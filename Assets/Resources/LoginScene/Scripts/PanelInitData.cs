using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelInitData : MonoBehaviour
{
    [SerializeField] private Slider mSliderProgress;
    [SerializeField] private Text mTextSlider;

    private SliderRunner _sliderRunner;
    private ActionRepeatTimer _timerWaitDataLoaded;
    
    private void Awake()
    {
        _sliderRunner = new SliderRunner(mSliderProgress, mTextSlider);
        _sliderRunner.SetSliderValue(0f);
        _sliderRunner.SetDurationPace(0.2f);

        if (mSliderProgress.gameObject.activeSelf)
            mSliderProgress.gameObject.SetActive(false);
    }

    public void StartLoadData()
    {
        mSliderProgress.gameObject.SetActive(true);
        StartCoroutine(DelayLoadData());
    }

    private IEnumerator CheckInitDataBeforeLoadHomeScene()
    {
        if (FirebaseManager.Instance.userDataLoaded)
        {
            _timerWaitDataLoaded = null;
            //
            yield return new WaitForSeconds(0.1f);
            //
            LoadSceneWithAsync("HomeScene");
        }
    }

    private IEnumerator DelayLoadData()
    {
        PlayerInventory.Instance.ClearInventory();
        DbManager.ClearInstance();

        FirebaseManager.Instance.userDataLoaded = false;
        
        float sliderValue = 0.1f;
        _sliderRunner.RunSliderValue(sliderValue);
        //
        float amountForDb = 0.2f;
        DbManager.GetInstance().LoadAllDBFile(this, sliderValue, amountForDb, delegate(float sliderValueRunTo)
        {
            _sliderRunner.RunSliderValue(sliderValueRunTo);
            sliderValue = sliderValueRunTo;
        });
        yield return null;

        while (true)
        {
            if (DbManager.GetInstance().HasInitDbLoadFile)
                break;
            yield return new WaitForEndOfFrame();
        }

        var thread = new System.Threading.Thread(() => LoadingThread(), 1);
        thread.Start();
        yield return null;

        //Wait to db finish
        while (true)
        {
            if (DbManager.GetInstance().HasInitDb)
                break;
            yield return new WaitForSeconds(0.2f);
        }

        thread.Abort();

        List<LoadingInitDataAction> lstAction = new List<LoadingInitDataAction>();
        lstAction.Add(new LoadingInitDataAction("Load User Data", delegate
        {
            FirebaseManager.Instance.LoadDataUser();
        }));
        sliderValue = 0.6f;

        float sleepBetweenSending = 0.2f;
        float amountForSendingData = 0.2f;
        float sliderEach = amountForSendingData / lstAction.Count;
        for (int i = 0; i < lstAction.Count; i++)
        {
            LoadingInitDataAction tmpAction = lstAction[i];
            //
            tmpAction.Action.Invoke();
            sliderValue += sliderEach;
            //
            _sliderRunner.RunSliderValue(sliderValue);
            yield return new WaitForSeconds(sleepBetweenSending);
        }

        float timeWaitForData = 15f;
        float interval = 0.2f;
        //
        float timeElapsed = 0f;

        _timerWaitDataLoaded = new ActionRepeatTimer(interval, delegate()
        {
            timeElapsed += interval;
            if (timeElapsed > timeWaitForData)
            {
                _timerWaitDataLoaded = null;
                //
                PopupMessage.ShowUp("", "Error load data!", "Try again?"
                    ,
                    delegate { StartCoroutine(DelayLoadData()); });
            }
            else
                StartCoroutine(CheckInitDataBeforeLoadHomeScene());
        });
    }


    private void LoadingThread()
    {
        float sliderValue = 0.3f;
        float amountForDb = 0.3f;
        DbManager.GetInstance().LoadAllDb2(this, sliderValue, amountForDb, delegate(float sliderValueRunTo)
        {
            _sliderRunner.RunSliderValue(sliderValueRunTo);
            sliderValue = sliderValueRunTo;
        });
    }

    private void Update()
    {
        _sliderRunner.UpdateSliderRunner(Time.deltaTime);
        if (_timerWaitDataLoaded != null)
            _timerWaitDataLoaded.UpdateTimer(Time.deltaTime);
    }

    #region LoadScene

    private void LoadSceneWithAsync(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        float sliderValue = 0.8f;
        float amountForLoadScene = 0.2f;
        //
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            float sliderProgress = Mathf.Clamp01(sliderValue + progress * amountForLoadScene);
            _sliderRunner.RunSliderValue(sliderProgress);

            if (asyncOperation.progress >= 0.9f && _sliderRunner.GetCurrentValue() >= 1f)
                asyncOperation.allowSceneActivation = true;

            yield return null;
        }
    }

    #endregion
}
