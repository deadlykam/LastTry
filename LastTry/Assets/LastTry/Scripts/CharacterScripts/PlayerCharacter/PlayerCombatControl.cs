using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatControl : BasicAnimation
{
    public enum CombatState { None, SetupInput, Processing, AcceptInput };

    // For checking if input is allowed to be accepted
    public bool IsAcceptInput { get { return _combatState == CombatState.AcceptInput; } }

    // For checking if the player is not in attack mode
    public bool IsMovable { get { return !_isCurrentCombatProcessing &&
                                         _combatInputs.Count == 0; } }

    private Queue<CombatAnimation> _combatInputs = new Queue<CombatAnimation>();
    private CombatState _combatState = CombatState.AcceptInput;

    private CombatAnimation _currentCombatInfo; // The current combat time information
                                           // of the animation

    private bool _isCurrentCombatProcessing; // For checking if current CombatInfo
                                             // is being processed

    private bool _isAcceptInputThreshold; // For checking if the current CombatInfo
                                          // have crossed the threshold for accepting
                                          // new inputs

    private float _currentAnimationTime; // Animation time for the current combat animation

    private List<EnemyCharacter> Enemies // For storing a list of enemies that have entered
        = new List<EnemyCharacter>();    // the weapons' range

    protected bool IsStopDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeStartUp();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerCombatControl();
    }
    
    /// <summary>
    /// This method sets up the new combat input to be processed.
    /// </summary>
    private void SetupCombatInputForProcessing()
    {
        // Condition for adding a new input to be processed
        if (_combatInputs.Count != 0 && !_isCurrentCombatProcessing
            && _combatState == CombatState.SetupInput)
        {
            _currentCombatInfo = _combatInputs.Dequeue(); // Getting the next CombatInfo
            _combatState = CombatState.Processing; // Start the processing of the input
            _isCurrentCombatProcessing = true; // Current CombatInfo is being processed
            _isAcceptInputThreshold = false; // Accepting input threshold resetted
            _currentAnimationTime = 0; // Resetting current animation timer for further use

            // Playing the attack animation
            PlayAttackAnimation(_currentCombatInfo.AttackAnimation);
        }

        // Todo: Check if else condition is require as fail safe here
    }

    /// <summary>
    /// This method processes the current CombatInfo.
    /// </summary>
    private void ProcessingCombatInfo()
    {
        // Condition for processing the curent CombatInfo
        if (_isCurrentCombatProcessing)
        {
            // The current time of the combat animation
            _currentAnimationTime += Time.deltaTime;

            // Condition for accepting a new combat input and hurting the enemies
            if(_currentAnimationTime >= _currentCombatInfo.ProcessTime &&
               !_isAcceptInputThreshold)
            {
                if(!IsStopDamage) HurtEnemiesInRange(); // Condition for hurting the enemies
                _combatState = CombatState.AcceptInput; // New combat input can be accepted
                _isAcceptInputThreshold = true; // Accepting input threshold crossed
            }

            // Condition for finishing the process of the current CombatInfo
            if(_currentAnimationTime >= _currentCombatInfo.TotalTime)
            {
                _isCurrentCombatProcessing = false; // Processing done for current 
                                                    // CombatInfo
            }
        }
    }

    /// <summary>
    /// This method hurts all the enemies within weapon range.
    /// </summary>
    private void HurtEnemiesInRange()
    {
        // Checking if any enemies are in the weapon range.
        if(Enemies.Count != 0)
        {
            // Loop for going through all the enemies in the list and hurting them
            for(int i = 0; i < Enemies.Count; i++)
            {
                // Checking if enemy is not null and hurting it
                if (Enemies[i] != null)
                    Enemies[i].TakeDamage(GetDefaultWeapon().GetDamage());
                else Enemies.Remove(Enemies[i]); // Removing null enemies
            }
        }
    }

    /// <summary>
    /// This method initializes the player combat control at the start up 
    /// in PlayerCombatControl.
    /// </summary>
    protected override void InitializeStartUp()
    {
        base.InitializeStartUp();
        _isCurrentCombatProcessing = false;
        _isAcceptInputThreshold = false;
        _currentAnimationTime = 0;
    }

    /// <summary>
    /// This method handles the player combat control update and must be called
    /// by child classes for the processing of PlayerCombatControl.
    /// </summary>
    protected void UpdatePlayerCombatControl()
    {
        SetupCombatInputForProcessing();
        ProcessingCombatInfo();
    }

    /// <summary>
    /// This method adds a combat input to the queue to be processed.
    /// </summary>
    /// <param name="animationTimeInfo">The time information of the animation,
    ///                                 of type CombatInfo</param>
    protected void AddCombatInput()
    {
        // Condition to check if input is allowed to be accepted
        if (IsAcceptInput)
        {
            // Adding a new input to the queue
            _combatInputs.Enqueue(GetAttackAnimation());

            // Starting to process the queue
            _combatState = CombatState.SetupInput;
        }
    }

    /// <summary>
    /// This method removes all the enemies in the enemy list.
    /// </summary>
    public void RemoveAllEnemies()
    {
        // Loop for removing all the enemies
        while (Enemies.Count != 0) Enemies.RemoveAt(0);
    }

    /// <summary>
    /// This method adds an enemy to the weapon range list.
    /// </summary>
    /// <param name="enemy">The enemy to add, of type EnemyCharacter</param>
    public void AddEnemyToRange(EnemyCharacter enemy)
    {
        Enemies.Add(enemy);
    }

    /// <summary>
    /// This method removes an enemy from the weapon range list.
    /// </summary>
    /// <param name="enemy">The enemy to remove, of type EnemyCharacter</param>
    public void RemoveEnemyFromRange(EnemyCharacter enemy)
    {
        Enemies.Remove(enemy);
    }
}
