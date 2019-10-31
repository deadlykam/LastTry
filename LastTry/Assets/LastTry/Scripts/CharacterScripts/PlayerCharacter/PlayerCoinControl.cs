using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoinControl : PlayerCombatControl
{
    [Header("PlayerCoinControl Properties")]
    public Color CoinGainedColour;
    public Color CoinLostColour;
    private int _coins; // Default must be 0
    public int Coins { get { return _coins; } }
    
    private Queue<CoinInfo> _requestProcesses = new Queue<CoinInfo>();
    private bool _isProcessing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerCoinControl(); // Calling the update method
    }

    /// <summary>
    /// Initializing PlayerCoinControl by setting up the attributes.
    /// </summary>
    protected override void InitializeStartUp()
    {
        base.InitializeStartUp();

        _coins = 100; // Default must be 0

        // Updating the player's coin in the UI
        UIInGameUIController.Instance.SetPlayerCoins(_coins);
    }

    /// <summary>
    /// This method processes the request to show coin effect.
    /// </summary>
    /// <param name="coinInfo">The CoinInfo from which to take the
    ///                        info from to show the coin effect,
    ///                        of type CoinInfo</param>
    private void ProcessCoinRequest(CoinInfo coinInfo)
    {
        // Requesting a font effect
        UIDamageFontManager.Instance
            .RequestDamageFont(transform.position,
                               coinInfo.Amount >= 0 ? 
                               coinInfo.Amount : coinInfo.Amount * -1,
                               coinInfo.FontColour);

        _coins += coinInfo.Amount; // Adding the amount to the coin,
                                   // The amount is both + or -

        // Updating the the player coin value in the UI
        UIInGameUIController.Instance.SetPlayerCoins(_coins);

        _isProcessing = false; // Processing is done
    }

    /// <summary>
    /// This method updates the logic in PlayerCoinControl
    /// </summary>
    protected void UpdatePlayerCoinControl()
    {
        // Condition for processing a request
        if(_requestProcesses.Count != 0 && !_isProcessing)
        {
            _isProcessing = true; // Starting the processing
            ProcessCoinRequest(_requestProcesses.Dequeue());
        }
    }

    /// <summary>
    /// This method checks if there are enough coins for the given amount.
    /// </summary>
    /// <param name="amount">The amount to check if enough coins are available,
    ///                      of type int</param>
    /// <returns>True means enough coins are available, false otherwise,
    ///          of type bool</returns>
    public virtual bool IsEnoughCoins(int amount) { return _coins >= amount; }

    /// <summary>
    /// This method adds a request to add coins to the player's total coin.
    /// </summary>
    /// <param name="amount">The amount of coins to add, of type int</param>
    public virtual void AddCoin(int amount)
    {
        _requestProcesses.Enqueue(new CoinInfo(amount, CoinGainedColour));
    }

    /// <summary>
    /// This method adds a request to remove coins from the player's total coin.
    /// </summary>
    /// <param name="amount">The amount of coins to remove, of type int</param>
    public virtual void Buy(int amount)
    {
        _requestProcesses.Enqueue(new CoinInfo(-amount, CoinLostColour));
    }
}

public struct CoinInfo
{
    public int Amount;
    public Color FontColour;

    /// <summary>
    /// This constructor initializes the struck.
    /// </summary>
    /// <param name="amount">The amount of coins, of type int</param>
    /// <param name="fontColour">The colour of the text, of type Color</param>
    public CoinInfo(int amount, Color fontColour)
    {
        Amount = amount;
        FontColour = fontColour;
    }
}