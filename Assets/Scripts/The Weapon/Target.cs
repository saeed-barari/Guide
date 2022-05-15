using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;
    [Tooltip("Only Applicable in 'Shatter' Death Type")]
    public GameObject shatteredObj;
    public enum d_type {
        debug,
        shatter,
        dropDead
    }
    [Tooltip("Debug: Deletes the Object upon Death\n\nShatter: replaces the game object with collapsing, shattered version of object upon death\n\nDrop Dead: disables AI and enables all rigidbody physics upon death")]
    public d_type deathType;

    void Update() {
        if (health <= 0) {
            if (deathType == d_type.debug) {
                Destroy(gameObject);
            }
            if (deathType == d_type.shatter) {
                Destroy(gameObject);
                Instantiate(shatteredObj, gameObject.transform.position, gameObject.transform.rotation);
            }
            if(deathType == d_type.dropDead) {
                //EnemyAI ai = gameObject.GetComponent<EnemyAI>();
                //ai.isEnabled = false;
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.None;
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }
    }
}
