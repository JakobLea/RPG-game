using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject Melee;
    bool isAttacking = false;
    float atkDuration = 0.3f;
    float atkTimer = 0f;

    [SerializeField] private Animator anim;
    private PlayerController playerController;

    void Start()
    {
        // Assuming the PlayerController script is attached to the same GameObject
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMeleeTimer();

        if (Input.GetKeyUp(KeyCode.J) || Input.GetMouseButton(0))
        {
            // Determine which animation to play based on the last movement direction
            if (playerController.lastMoveDirection.x == 0 && playerController.lastMoveDirection.y == 0)
            {
                // Player is not moving, choose an appropriate default animation or leave it empty
                anim.SetTrigger("Attack");
            }
            else
            {
                // Player is moving, determine the direction
                float horizontal = Mathf.Abs(playerController.lastMoveDirection.x);
                float vertical = Mathf.Abs(playerController.lastMoveDirection.y);

                if (horizontal > vertical)
                {
                    // Horizontal movement is more significant, play left or right attack animation
                    anim.SetTrigger("Attack");
                }
                else
                {
                    // Vertical movement is more significant, play up or down attack animation
                    if (playerController.lastMoveDirection.y > 0)
                    {
                        anim.SetTrigger("AttackUp");
                    }
                    else
                    {
                        anim.SetTrigger("AttackDown");
                    }
                }
            }

            StartCoroutine(AttackWithDelay());
        }
    }

    IEnumerator AttackWithDelay()
    {
        yield return new WaitForSeconds(0.45f); // Adjust the delay as needed

        OnAttack();
    }

    void OnAttack()
    {
        if (!isAttacking)
        {
            Melee.SetActive(true);
            isAttacking = true;
            // Call animator later
        }
    }

    void CheckMeleeTimer()
    {
        if (isAttacking)
        {
            atkTimer += Time.deltaTime;
            if (atkTimer >= atkDuration)
            {
                atkTimer = 0;
                isAttacking = false;
                Melee.SetActive(false);
            }
        }
    }
}
