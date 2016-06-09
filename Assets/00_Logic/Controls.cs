using UnityEngine;
using System.Collections;

public abstract class Controls  {

    public enum Input
    {
        Horizontal,
        Vertical,
        Action, //powerups
        Attack  //attack
    };

    public static string GetControlValue(Input input, int id)
    {
        return input.ToString() + id.ToString();
    }

}
