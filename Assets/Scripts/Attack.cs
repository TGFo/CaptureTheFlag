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
    private void Update()
    {
        if(projectile.currentState == Projectile.ProjectileStates.idle)
        {
            projectileReady = true;
        }
    }
    public void AttackActor()
    {
        if (projectileReady)
        {
            projectileReady = false;
            projectile.SetState(Projectile.ProjectileStates.chaseTarget);
        }
    }
}
