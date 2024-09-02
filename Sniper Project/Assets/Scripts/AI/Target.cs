using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float Health;
    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        Health = maxHealth;
    }

    public void TakeDamage(float Damage)
    {
        Health -= Damage;
        
        if(Health <= 0)
        {
            //Destroy(this.gameObject);
        }
    }
}
