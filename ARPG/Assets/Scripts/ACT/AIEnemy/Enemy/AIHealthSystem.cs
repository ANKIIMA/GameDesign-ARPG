using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACT.Health;
using Newtonsoft.Json;
using System.IO;
using Unity.Collections;

public class AIHealthSystem : BasicHealthModel
    {
    [SerializeField] private GameObject GreatSword;

    private void Start()
    {
        //开始先禁用血条
        uiManagement.EnemyHealthBar.gameObject.transform.parent.parent.gameObject.SetActive(false);
        //加载属性配置
        ReadConfig();
    }

    private void LateUpdate()
        {
            OnHitAnimationRotation(); 
        }

        /// <summary>
        /// 接受伤害
        /// </summary>
        /// <param name="damager">伤害值</param>
        /// <param name="hitAnimationName">受伤动画</param>
        /// <param name="attacker">攻击者</param>
        public override void TakeDamager(float damager, string hitAnimationName, Transform attacker)
        {
        base.TakeDamager(damager, hitAnimationName, attacker);
        if(attacker.TryGetComponent<yueduHealthController>(out yueduHealthController healthController))
        {
            damager = healthController.GetAttackDamage();
        }
        healthValue -= damager;
        uiManagement.OnEnemyHealthValueChange(this.CalHealthValuePercentage());

        //生命值归零死亡
        if(healthValue <= 0)
        {
            UpdateDieAnimation();
        }
        }

        private void OnHitAnimationRotation()
        {
            if(animator.CheckAnimationTag("Hit"))
            {
                transform.rotation = transform.LockOnTarget(currentAttacker, transform, 50f);
            }
        }

    /// <summary>
    /// 死亡动画回调函数
    /// </summary>
    private void OnDieEvent()
    {
        Instantiate(GreatSword, this.transform.position, Quaternion.identity);
        uiManagement.EnemyHealthBar.gameObject.transform.parent.parent.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void ReadConfig()
    {
        //json路径
        string filePath = "Assets/Resources/Data/Config_AI.json";
        //如果没找到就返回
        if(File.Exists(filePath) == false) 
        {
            Debug.Log("File not Found");
            return;
        }


        //读取文件为字符串
        string json = File.ReadAllText(filePath);

        //如果字符串不为空
        if(json.Length > 0)
        {
            //反序列化JSON数组对象
            List<AIConfig> roots = JsonConvert.DeserializeObject<List<AIConfig>>(json);


            return;
        }

        return;
    }
}