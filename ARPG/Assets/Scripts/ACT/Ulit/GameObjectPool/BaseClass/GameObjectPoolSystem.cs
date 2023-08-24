using System.Collections;
using System.Collections.Generic;
using MyUnitTools;
using UnityEngine;

/// <summary>
/// 单例管理对象池
/// </summary>
public class GameObjectPoolSystem : SingletonBase<GameObjectPoolSystem>
{
    //对象池列表
    [SerializeField,Header("预制体")] private List<GameObjectAssets> _assetsList = new List<GameObjectAssets>();
    //对象池父类 分类管理
    [SerializeField] private Transform poolObjectParent;
    //对象字典
    private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        //初始化对象池
        InitPool();
    }

    private void Start()
    {

    }

    private void InitPool()
    {
        //列表为空返回
        if (_assetsList.Count == 0) return;


        //首先遍历外面配置的资源
        for (int i = 0; i < _assetsList.Count; i++)
        {
            //检查列表第一个元素的内容是否已经在池子里面了，没有的话就创建一个
            if (!pools.ContainsKey(_assetsList[i].assetsName))
            {
                //字典新增元素
                pools.Add(_assetsList[i].assetsName, new Queue<GameObject>());
                //如果没有添加预制件就返回
                if (_assetsList[i].prefab.Length == 0) return;

                //创建完毕后，遍历这个对象的总数，比如总算5，那么就创建5个，然后存进字典
                for (int j = 0; j < _assetsList[i].count; j++)
                {
                    //实例化，统计有几个预制件，然后每次随机选其中一个实例化，可能是为了某个需求
                    //这里只有一个预制件，所以无所谓
                    GameObject temp_Gameobject = Instantiate(_assetsList[i].prefab[Random.Range(0, _assetsList[i].prefab.Length)]);
                    //设置父类
                    temp_Gameobject.transform.SetParent(poolObjectParent);
                    //位置归零
                    temp_Gameobject.transform.position = Vector3.zero;
                    //方向归零
                    temp_Gameobject.transform.rotation = Quaternion.identity;
                    //入队字典中对应的队列
                    pools[_assetsList[i].assetsName].Enqueue(temp_Gameobject);
                    //启用状态设置为false等待调用
                    temp_Gameobject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// 拿出来一个
    /// </summary>
    /// <param name="objectName">对象名称</param>
    /// <returns>对象</returns>
    public GameObject TakeGameObject(string objectName)
    {
        //如果字典中不存在指定名称的对象就返回
        if (!pools.ContainsKey(objectName)) return null;
        //出队一个对应字典中队列的预制体
        GameObject dequeueObject = pools[objectName].Dequeue();
        //重新入队
        pools[objectName].Enqueue(dequeueObject);
        //启用
        dequeueObject.SetActive(true);
        //返回该对象
        return dequeueObject;
    }

    /// <summary>
    /// 拿出来一个
    /// </summary>
    /// <param name="objectName">对象名称</param>
    /// <param name="position">对象位置</param>
    /// <param name="rotation">对象旋转</param>
    public void TakeGameobject(string objectName, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(objectName)) return;

        GameObject dequeueObject = pools[objectName].Dequeue();
        pools[objectName].Enqueue(dequeueObject);
        dequeueObject.SetActive(true);
        //设置位置和旋转
        dequeueObject.transform.position = position;
        dequeueObject.transform.rotation = rotation;
        //调用对象重写的生成方法
        dequeueObject.GetComponent<IPool>().SpawnObject();
    }

    /// <summary>
    /// 拿出来一个
    /// </summary>
    /// <param name="objectName">对象名称</param>
    /// <param name="position">对象位置</param>
    /// <param name="rotation">对象旋转</param>
    /// <param name="user">对象user</param>
    public void TakeGameobject(string objectName, Vector3 position, Quaternion rotation,Transform user)
    {
        if (!pools.ContainsKey(objectName)) return;

        GameObject dequeueObject = pools[objectName].Dequeue();
        pools[objectName].Enqueue(dequeueObject);
        dequeueObject.SetActive(true);
        dequeueObject.transform.position = position;
        dequeueObject.transform.rotation = rotation;
        //调用对象重写的生成方法
        dequeueObject.GetComponent<IPool>().SpawnObject(user);
    }
    
    /// <summary>
    /// 回收对象
    /// </summary>
    /// <param name="gameObject"></param>
    public void RecyleGameObject(GameObject gameObject)
    {
        //重置位置，旋转
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        //禁用
        gameObject.SetActive(false);
    }

    [System.Serializable]
    private class GameObjectAssets
    {
        //资产名称
        public string assetsName;
        //数量
        public int count;
        //预制件
        public GameObject[] prefab;
    }
}
