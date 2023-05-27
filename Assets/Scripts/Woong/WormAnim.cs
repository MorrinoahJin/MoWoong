using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormAnim : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    private float timer = 0f;
    private float animationInterval = 10f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= animationInterval)
        {
            PlayAnimation();
            timer = 0f;
        }
    }

    private void PlayAnimation()
    {
        // 애니메이션을 실행하는 코드를 여기에 추가합니다.
        animator.SetTrigger("earthWorm");
    }
}
