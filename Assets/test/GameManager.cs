using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerSO player;
    public Player play;
    private void Start()
    {
        play.Init(player, 2);
    }
}
