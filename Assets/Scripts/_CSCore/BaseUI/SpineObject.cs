using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineObject : MonoBehaviour
{
    public SkeletonGraphic m_SkeletonGraphic;

    public GameObject m_GoMesh;

    //
    private string m_AnimationDestroy = "";

    //
    public System.Action m_OnDisapperCb;

    // Use this for initialization
    protected virtual void Start()
    {
        if (m_SkeletonGraphic != null && m_SkeletonGraphic.AnimationState != null)
        {
            m_SkeletonGraphic.AnimationState.Complete += (delegate(Spine.TrackEntry trackEntry)
            {
                //Debug.Log("Animation complete: " + trackEntry.Animation.Name);	
                string animationFinished = trackEntry.Animation.Name;
                //
                if (animationFinished.Equals(m_AnimationDestroy))
                {
                    m_AnimationDestroy = "";
                    HideObject();
                    if (m_OnDisapperCb != null)
                    {
                        m_OnDisapperCb();
                    }
                }
            });
        }
    }
    
    public void LoadSpine(string pathSpine)
    {
        SharedUtils.LoadSpineSkeletonCanvas(m_SkeletonGraphic, pathSpine);
        //
        if (m_SkeletonGraphic != null && m_SkeletonGraphic.AnimationState != null)
        {
            m_SkeletonGraphic.AnimationState.Complete += (delegate(Spine.TrackEntry trackEntry)
            {
                //Debug.Log("Animation complete: " + trackEntry.Animation.Name);	
                string animationFinished = trackEntry.Animation.Name;
                //
                if (animationFinished.Equals(m_AnimationDestroy))
                {
                    m_AnimationDestroy = "";
                    HideObject();
                    if (m_OnDisapperCb != null)
                    {
                        m_OnDisapperCb();
                    }
                }
            });
        }
    }

    public void SetRaycastTarget(bool isOpen)
    {
        m_SkeletonGraphic.raycastTarget = isOpen;
    }

    public void SetPosition(Vector2 position)
    {
        CanvasHelper.SetPosition(gameObject, position);
    }

    private float _timeScale = 1f;

    public void SetTimeScale(float timeScale)
    {
        _timeScale = timeScale;
    }

    public void SetScale(float scale)
    {
        if (m_GoMesh != null)
            m_GoMesh.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void SetScale(float scaleX, float scaleY, float scaleZ)
    {
        if (m_GoMesh != null)
            m_GoMesh.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

    public void ShowObject()
    {
        m_GoMesh.SetActive(true);
    }

    public void HideObject()
    {
        m_GoMesh.SetActive(false);
    }

    public void PlayAnimationLoop(string aniName)
    {
        m_AnimationDestroy = "";
        ShowObject();
        if (m_SkeletonGraphic.AnimationState == null)
        {
            Debug.Log("------- Error Play Spine Effect: Animation_State == null");
            HideObject();
            return;
        }

        //
        m_SkeletonGraphic.AnimationState.TimeScale = _timeScale;
        m_SkeletonGraphic.AnimationState.SetAnimation(0, aniName, true);
    }

    public void PlayAnimationOnce(string aniName, string aniLoopAfter = "")
    {
        ShowObject();
        //
        if (m_SkeletonGraphic.AnimationState == null)
        {
            Debug.Log("------- Error Play Spine Effect: Animation_State == null");
            HideObject();
            return;
        }

        //
        m_SkeletonGraphic.AnimationState.TimeScale = _timeScale;
        //
        if (string.IsNullOrEmpty(aniLoopAfter))
        {
            m_AnimationDestroy = aniName;
            m_SkeletonGraphic.AnimationState.SetAnimation(0, aniName, false);
        }
        else
        {
            m_AnimationDestroy = "";
            m_SkeletonGraphic.AnimationState.SetAnimation(0, aniName, false);
            m_SkeletonGraphic.AnimationState.AddAnimation(0, aniLoopAfter, true, 0);
        }
    }

    public void PlayAnimationOnce(List<string> lstAniName, string aniLoopAfter = "")
    {
        ShowObject();
        //
        if (m_SkeletonGraphic.AnimationState == null)
        {
            Debug.Log("------- Error Play Spine Effect: Animation_State == null");
            HideObject();
            return;
        }

        //
        m_SkeletonGraphic.AnimationState.TimeScale = _timeScale;
        //
        if (string.IsNullOrEmpty(aniLoopAfter))
        {
            m_AnimationDestroy = lstAniName[lstAniName.Count - 1];
            int count = 0;
            foreach (string aniName in lstAniName)
            {
                if (count == 0)
                    m_SkeletonGraphic.AnimationState.SetAnimation(0, aniName, false);
                else
                    m_SkeletonGraphic.AnimationState.AddAnimation(0, aniName, false, 0);
                count++;
            }
        }
        else
        {
            m_AnimationDestroy = "";
            int count = 0;
            foreach (string aniName in lstAniName)
            {
                if (count == 0)
                    m_SkeletonGraphic.AnimationState.SetAnimation(0, aniName, false);
                else
                    m_SkeletonGraphic.AnimationState.AddAnimation(0, aniName, false, 0);
                count++;
            }

            //
            m_SkeletonGraphic.AnimationState.AddAnimation(0, aniLoopAfter, true, 0);
        }
    }


    // Update is called once per frame
    void Update()
    {
    }
}