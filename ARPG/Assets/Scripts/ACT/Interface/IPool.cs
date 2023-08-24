using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池生成/回收接口
/// </summary>
public interface IPool
{
    /// <summary>
    /// 无参实例化
    /// </summary>
    void SpawnObject();
    /// <summary>
    /// 有参实例化
    /// </summary>
    /// <param name="user"></param>
    void SpawnObject(Transform user);
    /// <summary>
    /// 回收
    /// </summary>
    void RecycleObject();
}
