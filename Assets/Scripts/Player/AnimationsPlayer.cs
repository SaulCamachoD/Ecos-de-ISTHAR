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
        ActivateWallkingWall(movementsPlayer.isWalkingOnWall);
        ActivateWallkingWallRight(movementsPlayer.WallRight);
        ActivateWallkingWallLeft(movementsPlayer.Wallleft);


    }

    private void WalkAnimations()
    {
        if (!movementsPlayer.isAxesInverted)
        {
            animator.SetFloat("MoveX", movementsPlayer.directionPlayer.x);
            animator.SetFloat("MoveZ", movementsPlayer.directionPlayer.y);
        }
        else 
        {
            animator.SetFloat("MoveX", movementsPlayer.directionPlayer.x);
            animator.SetFloat("MoveZ", 0.0f);
        }
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
    private void ActivateWallkingWall(bool enter)
    {
        animator.SetBool("IsWalkinginWall", enter);
    }
    
    private void ActivateWallkingWallRight(bool enter)
    {
        animator.SetBool("WallRight", enter);
    }
    
    private void ActivateWallkingWallLeft(bool enter)
    {
        animator.SetBool("WallLeft", enter);
    }

    public void Shot()
    {
        animator.SetTrigger("Bullet1");
    }

    public void ActivateGun(bool option)
    {
        animator.SetBool("Bullet2", option);
    }

    public void ActivateDeath() 
    {
        animator.SetTrigger("Death");
    }
}
