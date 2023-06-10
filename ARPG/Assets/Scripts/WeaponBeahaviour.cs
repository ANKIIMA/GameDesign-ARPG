using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBeahaviour : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "Enemy")
        {   
            other.gameObject.GetComponent<EnemyController>().recieveDamage(player.GetComponent<BasicBeahaviour>().DamageValue);
        }
    }
}
