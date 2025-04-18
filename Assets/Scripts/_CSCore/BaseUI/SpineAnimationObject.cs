using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class SpineAnimationObject : MonoBehaviour
{
    #region Define
    
    public SkeletonAnimation skeletonAnimation;
    public GameObject goMesh;
    
    #endregion

    #region Properties

    private string _animationDestroy = "";
    private System.Action _onDisapperCb;
    private float _timeScale = 1f;

    #endregion

    #region Core MonoBehavior
    
    protected virtual void Start()
    {
        if (skeletonAnimation != null && skeletonAnimation.AnimationState != null)
        {
            skeletonAnimation.AnimationState.Complete += (delegate(Spine.TrackEntry trackEntry)
            {
                //Debug.Log("Animation complete: " + trackEntry.Animation.Name);	
                string animationFinished = trackEntry.Animation.Name;
                //
                if (animationFinished.Equals(_animationDestroy))
                {
                    _animationDestroy = "";
                    HideObject();
                    if (_onDisapperCb != null)
                        _onDisapperCb();
                }
            });
        }
    }

    #endregion

    #region Public Method

    public void LoadSpine(string pathSpine)
    {
        ResourceHelper.LoadSkeletonAnimation(skeletonAnimation, pathSpine);
        //
        if (skeletonAnimation != null && skeletonAnimation.AnimationState != null)
        {
            skeletonAnimation.AnimationState.Complete += (delegate(Spine.TrackEntry trackEntry)
            {
                //Debug.Log("Animation complete: " + trackEntry.Animation.Name);	
                string animationFinished = trackEntry.Animation.Name;
                //
                if (animationFinished.Equals(_animationDestroy))
                {
                    _animationDestroy = "";
                    HideObject();
                    if (_onDisapperCb != null)
                        _onDisapperCb();
                }
            });
        }
    }

    public void ShowObject()
    {
        goMesh.SetActive(true);
    }

    public void HideObject()
    {
        goMesh.SetActive(false);
    }
    
    public void SetScale(float scale)
    {
        if (goMesh != null)
            goMesh.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void SetScale(float scaleX, float scaleY, float scaleZ)
    {
        if (goMesh != null)
            goMesh.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }
    
    public void SetAnimDoneCb(System.Action cb)
    {
        _onDisapperCb = cb;
    }
    
    public void SetTimeScale(float timeScale)
    {
        _timeScale = timeScale;
    }
    
    public void PlayAnimationLoop(string aniName)
    {
        _animationDestroy = "";
        ShowObject();
        if (skeletonAnimation.AnimationState == null)
        {
            Debug.Log("------- Error Play Spine Effect: Animation_State == null");
            HideObject();
            return;
        }

        //
        skeletonAnimation.AnimationState.TimeScale = _timeScale;
        skeletonAnimation.AnimationState.SetAnimation(0, aniName, true);
    }

    public void PlayAnimationOnce(string aniName, string aniLoopAfter = "")
    {
        ShowObject();
        //
        if (skeletonAnimation.AnimationState == null)
        {
            Debug.Log("------- Error Play Spine Effect: Animation_State == null");
            HideObject();
            return;
        }

        //
        skeletonAnimation.AnimationState.TimeScale = _timeScale;
        //
        if (string.IsNullOrEmpty(aniLoopAfter))
        {
            _animationDestroy = aniName;
            skeletonAnimation.AnimationState.SetAnimation(0, aniName, false);
        }
        else
        {
            _animationDestroy = "";
            skeletonAnimation.AnimationState.SetAnimation(0, aniName, false);
            skeletonAnimation.AnimationState.AddAnimation(0, aniLoopAfter, true, 0);
        }
    }
    
    public void PlayAnimationOnce(List<string> lstAniName, string aniLoopAfter = "")
    {
        ShowObject();
        //
        if (skeletonAnimation.AnimationState == null)
        {
            Debug.Log("------- Error Play Spine Effect: Animation_State == null");
            HideObject();
            return;
        }

        //
        skeletonAnimation.AnimationState.TimeScale = _timeScale;
        //
        if (string.IsNullOrEmpty(aniLoopAfter))
        {
            _animationDestroy = lstAniName[lstAniName.Count - 1];
            int count = 0;
            foreach (string aniName in lstAniName)
            {
                if (count == 0)
                    skeletonAnimation.AnimationState.SetAnimation(0, aniName, false);
                else
                    skeletonAnimation.AnimationState.AddAnimation(0, aniName, false, 0);
                count++;
            }
        }
        else
        {
            _animationDestroy = "";
            int count = 0;
            foreach (string aniName in lstAniName)
            {
                if (count == 0)
                    skeletonAnimation.AnimationState.SetAnimation(0, aniName, false);
                else
                    skeletonAnimation.AnimationState.AddAnimation(0, aniName, false, 0);
                count++;
            }

            //
            skeletonAnimation.AnimationState.AddAnimation(0, aniLoopAfter, true, 0);
        }
    }
    
    #endregion
}
