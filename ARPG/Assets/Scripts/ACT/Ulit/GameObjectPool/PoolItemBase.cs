using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolItemBase : MonoBehaviour,IPool
{
    //使用者
    protected Transform user;
    //音源
    protected AudioSource _audioSource;
    //最大延迟时间
    [SerializeField, Header("Setting")] protected float maxDelayTime;
    //是否被回收
    protected bool isRecycle;
    
    /// <summary>
    /// 获取AudioSource
    /// </summary>
    protected virtual void Awake()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
    }
    /// <summary>
    /// 每帧判断是否回收
    /// </summary>
    protected virtual void Update()
    {
        RecycleObject();
    }



    #region 接口
    /// <summary>
    /// 生成对象
    /// </summary>
    public virtual void SpawnObject()
    {
        
    }
    /// <summary>
    /// 通过
    /// </summary>
    /// <param name="user"></param>
    public virtual void SpawnObject(Transform user)
    {
        //设置user
        this.user = user;
        //没有被回收
        isRecycle = false;
        //创建计时器，计时时间为maxDelayTime，isRecycle的lamda表达式作为回调函数，计时器设置为Done结束计时
        //计时结束时回调isRecycle为真
        GameObjectPoolSystem.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(maxDelayTime,
            () => isRecycle = true,false);
    }

    public virtual void RecycleObject()
    {
        //如果isRecycle为真，回收当前对象
        if(isRecycle)
            GameObjectPoolSystem.Instance.RecyleGameObject(this.gameObject);
    }
    
    #endregion
    
    
    public Transform GetUser() => user;
}
