using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRobotAttack : StateMachineBehaviour
{
    private Enemy enemy;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponentInParent<Enemy>();
        PoolManager.instance.Pull(enemy.enemyProjectile,
                                    enemy.shootingPoint.position,
                                    Quaternion.LookRotation(enemy.shootingPoint.forward));
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, 
                                    Quaternion.LookRotation(enemy.target.position - enemy.transform.position), 
                                    Time.deltaTime * enemy.enemyAttackTurnSpeed );
    }

}
