using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollowCharacter : UIFollow
{
    public BasicCharacter Character;
    public Image HealthBar;

    private void Update()
    {
        // Calling the update method of the UI follow
        UpdateUIFollow();

        // Condition to check if health update is required
        if (HealthBar.fillAmount != Character.GetHealthPercentage())
        {
            // Updating the health bar fill amount
            HealthBar.fillAmount = Character.GetHealthPercentage();
        }
    }
}
