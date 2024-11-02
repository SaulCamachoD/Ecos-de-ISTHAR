using UnityEngine;
using UnityEngine.Rendering;

public class AnimationsPlayer : MonoBehaviour
{
    public Animator animator;
    private MovementsPlayer movementsPlayer;
    void Start()
    {
        animator = GetComponent<Animator>();
        movementsPlayer = GetComponent<MovementsPlayer>();
    }

    
    void Update()
    {
        WalkAnimations();
        ActivateRun();
        ActivateJump(movementsPlayer.isJumping); 
        ActivateDash(movementsPlayer.isDashing);
        ActivateAttackMode(movementsPlayer.isAttackinMode);
        
    }

    private void WalkAnimations()
    {
        animator.SetFloat("MoveX", movementsPlayer.directionPlayer.x);
        animator.SetFloat("MoveZ", movementsPlayer.directionPlayer.y);
    }

    private void ActivateRun()
    {
        animator.SetBool("Run", movementsPlayer.isRunning);
    }

    private void ActivateJump(bool enter)
    {
        animator.SetBool("Jump", enter);
    }

    private void ActivateDash(bool enter)
    {
        animator.SetBool("Dash" , enter);
    }
    private void ActivateAttackMode(bool enter)
    {
        animator.SetBool("AttackMode" , enter);
    }

}
