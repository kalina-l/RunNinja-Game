﻿using UnityEngine;
using System.Collections;

public abstract class Controls  {

    public enum Input
    {
        Horizontal,
        Jump,
        Action, //powerups
        Attack,  //attack
        Roll,
        Start
    };

    public static string GetControlValue(Input input, int id)
    {
        //Debug.Log(input.ToString() + " | id "+id );
        if(id == 0)
        {
            id = 1;
        }
        return input.ToString() + id.ToString();
    }

}
