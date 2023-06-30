using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUnitTools
{
    public class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if(_instance == null) 
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<T>();
                    go.name = typeof(T).ToString();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }
        
    }
}

