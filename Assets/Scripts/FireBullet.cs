using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    Collider m_ObjectCollider;
    private GameObject dest;
    private Material mat;

    void Start()
    {
        m_ObjectCollider = GetComponent<Collider>();
        gameObject.AddComponent<ConfigurableJoint>();
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        m_ObjectCollider.isTrigger = true;
    }

    public void setDestination(GameObject destination){
        dest = destination;
    }
    public void setMaterial(Material materialToSet){
        mat = materialToSet;
    }
    void Update(){
        if (dest == null){
            enabled = false;
            Destroy(gameObject);
        }
        else if (transform.position.x <= dest.transform.position.x + 1 && transform.position.x >= dest.transform.position.x - 1){
            if (transform.position.y <= dest.transform.position.y + 1 && transform.position.y >= dest.transform.position.y - 1){
                if (transform.position.z <= dest.transform.position.z + 1 && transform.position.z >= dest.transform.position.z - 1){
                    dest.tag = "Grass";
                    dest.GetComponent<BoxCollider>().isTrigger = true;
                    dest.GetComponent<Renderer>().material = mat;
                    enabled = false;
                    Destroy(gameObject);
                }
            }
        }
    }
}
