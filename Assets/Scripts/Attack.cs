using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public bool projectileReady = true;
    public GameObject projectileObject;
    private Projectile projectile;
    private void Start()
    {
        projectile = projectileObject.GetComponent<Projectile>();
    }
    public void AttackActor(Transform target)
    {
        if (projectileReady)
        {
            projectileReady = false;
            projectile.currentState = Projectile.projectileStates.chaseTarget;
        }
    }
}
