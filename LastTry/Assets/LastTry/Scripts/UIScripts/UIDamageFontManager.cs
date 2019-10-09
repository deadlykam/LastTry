using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDamageFontManager : MonoBehaviour
{
    public static UIDamageFontManager Instance;

    private List<Canvas> _damageFontCanvases = new List<Canvas>();
    private Queue<DamageFontDataset> _requestDamageFont = new Queue<DamageFontDataset>();
    private Queue<int> _releaseDamageFont = new Queue<int>();

    private int _pointer = 0;

    private void Start()
    {
        // The instance will be replaced each time a new world is
        // loaded so that basic game world attributes can be used
        // again
        Instance = this;

        // Adding all the damage font to the list
        for(int i = 0; i < transform.childCount; i++)
        {
            // Setting the index of the UIDamageFont
            transform.GetChild(i).GetComponent<UIDamageFont>().Index = i;
            // Adding the damage font to the list
            _damageFontCanvases.Add(transform.GetChild(i).GetComponent<Canvas>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Checking if any damage font request available
        if(_requestDamageFont.Count != 0)
        {
            // Condition to check if damage font available
            if (IsDamageFontAvailable())
                GetDamageFont(_requestDamageFont.Dequeue()); // Starting a damage
                                                             // font effect
        }

        // Condition for releasing a damage font
        if (_releaseDamageFont.Count != 0) ReleaseDamageFont(_releaseDamageFont.Dequeue());
    }

    /// <summary>
    /// This method checks if any damage fonts are available.
    /// </summary>
    /// <returns>True means damage fonts are available, false otherwise, of type
    ///          bool</returns>
    private bool IsDamageFontAvailable()
    {
        // Incrementing pointer to get the next damage font
        _pointer = !_damageFontCanvases[_pointer].enabled ? _pointer : 
                   (_pointer + 1) >= _damageFontCanvases.Count ? 0 : _pointer + 1;

        return !_damageFontCanvases[_pointer].enabled;

    }

    /// <summary>
    /// This method starts the damage font effect.
    /// </summary>
    /// <param name="damageFont">The damage font to show, of type DamageFontDataset</param>
    private void GetDamageFont(DamageFontDataset damageFont)
    {
        _damageFontCanvases[_pointer].enabled = true;
        _damageFontCanvases[_pointer].GetComponent<UIDamageFont>().
            StartEffect(damageFont.Position, damageFont.DamageValue, damageFont.FontColour);
    }

    /// <summary>
    /// This method releases a damage font from use.
    /// </summary>
    /// <param name="index">The index of the damage font to be released,
    ///                     of type int</param>
    private void ReleaseDamageFont(int index)
    { _damageFontCanvases[index].enabled = false; }

    /// <summary>
    /// This method requests for a damage font effect.
    /// </summary>
    /// <param name="position">The position for the damage font, of type Vector3</param>
    /// <param name="damageValue">The damage value for the damage font, of type int</param>
    /// <param name="fontColour">The colour of the font, of type Color</param>
    public void RequestDamageFont(Vector3 position, int damageValue, Color fontColour)
    {
        _requestDamageFont.Enqueue(new DamageFontDataset(position, damageValue,
                                                         fontColour));
    }

    /// <summary>
    /// This method requests for a damage font to be released so that it is available
    /// to use again.
    /// </summary>
    /// <param name="index">The index of the damage font to be released, of type int</param>
    public void RequestReleaseDamageFont(int index) { _releaseDamageFont.Enqueue(index); }
}

public struct DamageFontDataset
{
    public Vector3 Position;
    public int DamageValue;
    public Color FontColour;

    /// <summary>
    /// This constructor creates the struct.
    /// </summary>
    /// <param name="position">Storing the position of the damage font,
    ///                        of type Vector3</param>
    /// <param name="damageValue">Storing the damage value of the damage font,
    ///                           of type int</param>
    /// <param name="fontColour">The colour of the font, of type Color</param>
    public DamageFontDataset(Vector3 position, int damageValue, Color fontColour)
    {
        Position = position;
        DamageValue = damageValue;
        FontColour = fontColour;
    }
}