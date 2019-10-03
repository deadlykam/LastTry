using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyCharacter
{
    private bool _hasDealtDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeStartUp();
    }

    // Update is called once per frame
    void Update()
    {
        // Calling the update of BasicCharacter
        UpdateBasicCharacter();

        // Calling the update of BasicAnimation
        UpdateBasicAnimation();

        // Calling the update of EnemyCharacter
        UpdateEnemyCharacter();

        UpdateEnemyMelee();
    }

    /// <summary>
    /// This method initializes the enemy melee at the start up in EnemyMelee.
    /// </summary>
    protected override void InitializeStartUp()
    {
        base.InitializeStartUp();
    }

    /// <summary>
    /// This method gets a new attack information from the EnemyCharacter and 
    /// resets all attacking attributes.
    /// </summary>
    protected override void GetNewAttackAnimation()
    {
        base.GetNewAttackAnimation();
        _hasDealtDamage = false;
    }

    /// <summary>
    /// This method is the update method of EnemyMelee.
    /// </summary>
    protected void UpdateEnemyMelee()
    {
        if(!IsDead && !IsHurt) // Condition for when the enemy is not hurt or dead
        {
            if (IsReachedTarget && !IsAttacking) // Reached and attacking player
            {
                /*// Condition for getting a new attacking animation
                // and attack information
                if (!IsAttacking)
                {
                    GetNewAttackAnimation();
                    PlayAttackAnimation(CurrentCombatInfo.AttackAnimation);
                }*/

                // Condition for starting to attack the player
                if (IsPlayerInWeaponRange)
                {
                    GetNewAttackAnimation();
                    PlayAttackAnimation(CurrentCombatInfo.AttackAnimation);
                }
            }

            if (IsAttacking) // Condition for attacking
            {
                // Counting the attack animation timer
                AttackTimer = (AttackTimer + Time.deltaTime)
                               >= CurrentCombatInfo.TotalTime ?
                               -1 : AttackTimer + Time.deltaTime;
                
                // Condition for dealing damage to the player
                if (AttackTimer >= CurrentCombatInfo.ProcessTime &&
                    !_hasDealtDamage)
                {
                    // Todo: Make sure the damage is taken from the weapon
                    //       and not from a fixed value. Fixed value is
                    //       only for testing.
                    // Condition to make the player take damage
                    if (IsPlayerInWeaponRange) Manager.Player.TakeDamage(5);
                    _hasDealtDamage = true;
                }
            }
        }
    }

    /// <summary>
    /// This method makes the melee enemy take damage and resets any attack variables.
    /// </summary>
    /// <param name="amount"></param>
    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        _hasDealtDamage = false;
    }
}
