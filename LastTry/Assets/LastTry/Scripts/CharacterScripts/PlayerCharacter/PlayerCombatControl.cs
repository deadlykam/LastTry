using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatControl : BasicAnimation
{
    public enum CombatState { None, SetupInput, Processing, AcceptInput };
    
    /*[Header("Player Combat Control Properties")]*/
    //public AttackAnimationInfo[] AnimationInfos;

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

    // Start is called before the first frame update
    void Start()
    {
        InitializeStartUp();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerCombatControlUpdate();
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
            /*PlayAttackAnimation(_currentCombatInfo.Weapon); // Playing the current
                                                            // attack animation*/
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

            // Condition for accepting a new combat input
            if(_currentAnimationTime >= _currentCombatInfo.ProcessTime &&
               !_isAcceptInputThreshold)
            {
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
    /// This method initializes the player combat control at the start up 
    /// in PlayerCombatControl.
    /// </summary>
    protected override void InitializeStartUp()
    {
        base.InitializeStartUp();
        //_combatInputs = new Queue<CombatInfo>();
        //_combatState = CombatState.AcceptInput;
        _isCurrentCombatProcessing = false;
        _isAcceptInputThreshold = false;
        _currentAnimationTime = 0;
    }

    /// <summary>
    /// This method handles the player combat control update and must be called
    /// by child classes for the processing of PlayerCombatControl.
    /// </summary>
    protected void PlayerCombatControlUpdate()
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
}
