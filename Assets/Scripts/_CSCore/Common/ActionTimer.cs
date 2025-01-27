using UnityEngine;
using System.Collections;

public class ActionRepeatTimer:ActionTimer
{
	public float mInterval = 0.5f;
	//
    private float mCountTime = 0f;
    //-1: infinite
    public float mDuration = -1;

	//
    private MyActionDelegate.MyAction mActionJob, mActionJobActionFinish;

    public ActionRepeatTimer(float interval, MyActionDelegate.MyAction job)
	{
		this.mActionJob = job;
		this.mInterval = interval;
        this.mDuration = -1;
	}

    public ActionRepeatTimer(float interval, float duration, MyActionDelegate.MyAction job,  
		MyActionDelegate.MyAction jobActionFinish = null)
    {
        this.mActionJob = job;
        this.mInterval = interval;
        this.mDuration = duration;
        this.mActionJobActionFinish = jobActionFinish;
    }

	public void CallTheDelegate()
	{
		if (mActionJob != null)
		{
			mActionJob ();
		}
	}

	public void ResetCounter()
	{
		mCountTime = 0f;
        mTotalTime = 0;
        mDone = false;
	}

    public void Stop()
    {
        mDone = true;
    }

    private float mTotalTime = 0;
    //
    public bool m_IgnoreTimeScale = false;

	public override void UpdateTimer(float deltaTime)
	{
        if (mDone)
        {
            return;
        }
        //
        if (m_IgnoreTimeScale)
        {
            float timeScale = Time.timeScale;
            float realTime = deltaTime / timeScale;
            //
            this.mCountTime += realTime;
            this.mTotalTime += realTime;
        }
        else
        {
            this.mCountTime += deltaTime;
            this.mTotalTime += deltaTime;
        }
        //
		if (this.mCountTime > this.mInterval) 
		{
			this.mCountTime = 0;
            if (mActionJob != null)
			    mActionJob ();
		}
        if (this.mDuration > 0 && this.mTotalTime > this.mDuration)
        {
            if (mActionJobActionFinish != null)
                mActionJobActionFinish();
            mDone = true;
        }
	}
}


public class ActionOnceTimer: ActionTimer
{
    public float mDelay = 0.5f;
    //
    private float mCountTime = 0f;
    //
    private MyActionDelegate.MyAction mActionJob;


    public ActionOnceTimer(float delay, MyActionDelegate.MyAction job)
    {
        this.mActionJob = job;
        this.mDelay = delay;
    }

    public void Reset()
    {
        mCountTime = 0;
        mDone = false;
    }

    public void Stop()
    {
        mDone = true;
    }



    public override void UpdateTimer(float deltaTime)
    {
        if (mDone)
        {
            return;
        }
        //
        this.mCountTime += deltaTime;
        if (this.mCountTime > this.mDelay) 
        {
            this.mCountTime = 0;
            this.mDone = true;
            //
            if (mActionJob != null)
            {
                //Debug.Log("-------- Time out, do the job");
                mActionJob();
            }
        }
    }
}

public abstract class ActionTimer
{
    protected bool mDone = false;

    public bool IsDone()
    {
        return mDone;
    }

    public abstract void UpdateTimer(float deltaTime);
}

public class MyActionDelegate
{
    public delegate void MyAction();

    public delegate void MyAction<T>(T arg);

    public delegate void MyAction<T1, T2>(T1 arg1, T2 arg2);

    public delegate void MyAction<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
}

