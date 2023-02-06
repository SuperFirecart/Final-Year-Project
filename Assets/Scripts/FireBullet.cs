using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    Collider m_ObjectCollider;
    public int damage;

    void Start()
    {
        m_ObjectCollider = GetComponent<Collider>();
        gameObject.AddComponent<ConfigurableJoint>();
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        m_ObjectCollider.isTrigger = true;
    }

    public void setDamage(int damagePower){
        damage = damagePower;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = GameObject.Find(other.gameObject.name).GetComponent<Enemy>();
            if (enemy.health - this.damage <= 0)
            {
                Destroy(other.gameObject, 0f);
                print("Enemy Killed");
            }
            else
            {
                enemy.health -= this.damage;
                enemy.setHealthText();
            }
        }
    }
}
