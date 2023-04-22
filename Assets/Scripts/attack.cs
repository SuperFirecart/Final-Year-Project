using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class attack : MonoBehaviour
{
    public Material mat;
    public void shootAttack(Vector3 startPos, GameObject end){
        System.Random rnd = new System.Random();
        int randX = rnd.Next(-20, 21);
        int randZ = rnd.Next(-20, 21);
        GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.transform.position = new Vector3(startPos.x + randX, startPos.y, startPos.z + randZ);
        bullet.transform.rotation = Quaternion.LookRotation((end.transform.position - bullet.transform.position).normalized);
        bullet.GetComponent<Renderer>().material = mat;
        FireBullet bullScript = bullet.AddComponent<FireBullet>();
        bullScript.setDestination(end);
        bullScript.setMaterial(mat);

        bullet.AddComponent<Rigidbody>();
        Rigidbody instBulletRigidbody = bullet.GetComponent<Rigidbody>();
        instBulletRigidbody.AddForce(bullet.transform.forward * 1000);


        // Destroy(bulletClone, 1.1f);
    }
}
