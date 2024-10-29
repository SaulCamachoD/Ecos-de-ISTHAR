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
}
