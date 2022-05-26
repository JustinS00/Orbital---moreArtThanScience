using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Run : StateMachineBehaviour {

	public float speed = 1.5f;

	Transform player;
	Rigidbody2D rb;
	Zombie zombie;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		rb = animator.GetComponent<Rigidbody2D>();
		zombie = animator.GetComponent<Zombie>();

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		zombie.LookAtPlayer();

        //target is player
		Vector2 target = new Vector2(player.position.x, rb.position.y);

        //move to target
		Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
		rb.MovePosition(newPos);
        
        /* for next time if want to have attack animation
		if (Vector2.Distance(player.position, rb.position) <= attackRange) {
			animator.SetTrigger("Attack");
		}
        */
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//animator.ResetTrigger("Attack");
	}
}