using UnityEngine;
using System.Collections;

public interface IPowerUp {

    Sprite GetIcon();
    void Setup(PlayerControl player);
    void Activate();
    int GetWeight();
}
