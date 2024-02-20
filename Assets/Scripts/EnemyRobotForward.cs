using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyRobotForward : StateMachineBehaviour
{
    private Enemy enemy;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Recuperamos la referencia al enemy propietario
        // este contendrá la información con la que trabajar
        enemy = animator.GetComponentInParent<Enemy>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Si está activo el navmesh agent y existe un objetivo
        if (enemy.nav.enabled && enemy.target != null)
        {
            // indicamos que el destino es el objetivo del agent
            enemy.nav.SetDestination(enemy.target.position);
        }
        // Verificamos si el enemigo se encuentra a la distancia de ataque
        if (!enemy.nav.pathPending && enemy.nav.remainingDistance <= enemy.attackDistance)
        {
            // cambiamos al estado de ataque
            animator.SetTrigger("Attack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reseteamos el trigger al salir del estado para evitar que se dispare la animación más de una vez 
        animator.ResetTrigger("Attack");
    }

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
