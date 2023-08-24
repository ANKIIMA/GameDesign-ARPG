using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //计时器
    private float timer;
    //内置泛型委托，没有返回值
    private Action action;
    //计时器是否结束
    private bool timeIsDone;

    /// <summary>
    /// 每帧更新
    /// </summary>
    private void Update()
    {
        OnUpdate();
        RecycleObject();
    }


   
    /// <summary>
    /// 更新计时器
    /// </summary>
    private void OnUpdate()
    {
        //如果计时器没有启用，返回
        if (!this.gameObject.activeSelf) return;

        //如果计时器大于0且没有计时结束
        if (timer > 0 && !timeIsDone)
        {
            //计时
            timer -= Time.deltaTime;
            //如果减少到小于0结束计时
            if (timer < 0)
            {
                action?.Invoke();
                timeIsDone = true; 
            }
        }
    }

    /// <summary>
    /// 创建计时器
    /// </summary>
    /// <param name="timer">计时时间</param>
    /// <param name="cllBackAction">回调函数</param>
    public void CreateTime(float timer, Action cllBackAction, bool timeIsDone = false) 
    {
        //设置计时器时间
        this.timer = timer;
        //设置回调函数
        this.action = cllBackAction;
        //开始计时
        this.timeIsDone = timeIsDone;
    }

    //生成对象
    public void SpawnObject()
    {
        
    }

    //回收计时器
    public void RecycleObject()
    {
        //如果完成计时
        if (timeIsDone) 
        {
            //委托取消
            action = null;
            //回收对象
            GameObjectPoolSystem.Instance.RecyleGameObject(this.gameObject);
        }
    }

    public void SpawnObject(Transform user)
    {
        
    }
}
