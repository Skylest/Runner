using UnityEngine;

namespace Assets.Scripts.Player
{
    public class AnimatorController : MonoBehaviour
    {
        private Animator animator;
        
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void AnimateJump()
        {
            animator.SetTrigger("Jump");
            animator.ResetTrigger("Land");
        }
        
        public void AnimateLand()
        {
            animator.SetTrigger("Land");
        }
       
        public void AnimateRun() 
        {
            animator.SetFloat("z", 1f);
        }
        
        public void AnimateSprint()
        {
            animator.SetFloat("z", 2f);
        }
        
        public void AnimateStay()
        {
            animator.SetFloat("z", 0f);
        }
        
        public void AnimateSomersault()
        {
            animator.SetTrigger("Somersault");
        }
    }
}
