using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player CurrentPlayer;


    private void Awake()
    {
        CurrentPlayer = FindObjectOfType<Player>();
    }
}
