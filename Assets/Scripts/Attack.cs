using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //checks if the projectile is ready to be launched
    public bool projectileReady = true;
    //the projectile object
    public GameObject projectileObject;
    private Projectile projectile;
    private void Start()
    {
        projectile = projectileObject.GetComponent<Projectile>();
    }
    private void Update()
    {
        //checks if the projectile is currently idle setting it to ready if it is
        if(projectile.currentState == Projectile.ProjectileStates.idle)
        {
            projectileReady = true;
        }
    }
    public void AttackActor()
    {
        //checks if the projectile is ready
        if (projectileReady)
        {
            //sets the projectile to not being ready
            projectileReady = false;
            //sets the projectile's state to chase the target
            projectile.SetState(Projectile.ProjectileStates.chaseTarget);
        }
    }
}
