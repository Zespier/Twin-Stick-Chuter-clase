using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRobotAttack : StateMachineBehaviour
{
    private Enemy enemy;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponentInParent<Enemy>();
        // Pedimos a la pool correspondiente, instanciar un proyectil
        PoolManager.instance.Pull(enemy.enemyProjectile,
                                    enemy.shootingPoint.position,
                                    Quaternion.LookRotation(enemy.shootingPoint.forward));
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, 
                                    Quaternion.LookRotation(enemy.target.position - enemy.transform.position), 
                                    enemy.enemyAttackTurnSpeed * Time.deltaTime);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
