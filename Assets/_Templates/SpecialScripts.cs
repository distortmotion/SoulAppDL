using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialScripts : MonoBehaviour
{
    public static string Gamename;
    public static string ThisGame;
    public static void SpecialTrivia()
    {
        if (ThisGame == "Game 1")
        {
            UnrealTournament2004.Trivia();
        }
        else if (ThisGame == "Game 2")
        {

        }
        else if (ThisGame == "Game 3")
        {

        }
        else if (ThisGame == "Game 4")
        {

        }
        else
        {
            Debug.Log("WTF ERROR");
        }
    }

    public static void SpecialUsernameRules() {
        if (ThisGame == "Game 1")
        {
            UnrealTournament2004.Special();
        }
        else if (ThisGame == "Game 2")
        {

        }else if (ThisGame == "Game 3")
        {

        }else if (ThisGame == "Game 4")
        {

        }else
        {
            Debug.Log("WTF ERROR");
        }
    }
}
