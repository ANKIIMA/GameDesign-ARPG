using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolItemBase : MonoBehaviour,IPool
{
    protected Transform user;
    protected AudioSource _audioSource;
    [SerializeField, Header("Setting")] protected float maxDelayTime;
    protected bool isRecycle;
    

    protected virtual void Awake()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    protected virtual void Update()
    {
        RecycleObject();
    }



    #region 接口

    public virtual void SpawnObject()
    {
        
    }

    public virtual void SpawnObject(Transform user)
    {
        this.user = user;
        isRecycle = false;
        GameObjectPoolSystem.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(maxDelayTime,
            () => isRecycle = true,false);
    }

    public virtual void RecycleObject()
    {
        if(isRecycle)
            GameObjectPoolSystem.Instance.RecyleGameObject(this.gameObject);
    }
    
    #endregion
    
    
    public Transform GetUser() => user;
}
