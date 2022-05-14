using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health;
    public GameObject shatteredObj;
    public enum d_type {
        debug,
        shatter,
        dropDead
    }
    public d_type deathType;

    void Update() {
        if (health <= 0) {
            if (deathType == d_type.debug) {
                Destroy(gameObject);
            }
        }
    }
}
