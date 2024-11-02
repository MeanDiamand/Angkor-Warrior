using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFloatBehaviour : StateMachineBehaviour
{
    public string floatName;
    public bool updateOnStateEnter;
    public bool updateOnStateExit;
    public bool updateOnStateMachineEnter;
    public bool updateOnStateMachineExit;
    public float valueOnEnter;
    public float valueOnExit;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(updateOnStateEnter)
        {
            animator.SetFloat(floatName, valueOnEnter);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(updateOnStateExit)
        {
            animator.SetFloat(floatName, valueOnExit);
        }
    }

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if(updateOnStateMachineEnter)
        {
            animator.SetFloat(floatName, valueOnEnter);
        }
    }

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if(updateOnStateMachineExit)
        {
            animator.SetFloat(floatName, valueOnExit);
        }
    }
}