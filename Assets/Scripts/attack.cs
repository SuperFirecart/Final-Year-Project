using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    public static GameObject bulletClone;
    public static void shootAttack(string attackType, GameObject Bullet, int damagePower, Transform pos){
        bulletClone = Instantiate(Bullet, pos.transform.position, Quaternion.identity) as GameObject;
        bulletClone.AddComponent<FireBullet>();

        FireBullet BulletStats = bulletClone.GetComponent<FireBullet>();
        BulletStats.damage = damagePower;
        
        bulletClone.AddComponent<Rigidbody>();
        bulletClone.transform.rotation = pos.transform.rotation;
        Rigidbody instBulletRigidbody = bulletClone.GetComponent<Rigidbody>();
        instBulletRigidbody.AddForce(bulletClone.transform.forward * 1500);
        Destroy(bulletClone, 1.1f);
    }
}
