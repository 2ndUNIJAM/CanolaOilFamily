using UnityEngine;

public class WeeklyResultSMB : StateMachineBehaviour
{ 
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.SetActive(false);
    }
}
