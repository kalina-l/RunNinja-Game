using UnityEngine;
using System.Collections;

public class Helper : MonoBehaviour {

    static Color[] colors = { Color.white, Color.blue, Color.red, Color.green, Color.yellow, Color.black };

    public static Color idToColor(int id)
    {
        return colors[id];
    }
}
