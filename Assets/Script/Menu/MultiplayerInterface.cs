using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiplayerInterface : MonoBehaviour
{
    public Relay relayScript;
    public TextMeshProUGUI hostCodeInputField;
    public TextMeshProUGUI hostCode;
    public void TryClientConnexion()
    {
        relayScript.JoinRelay(hostCodeInputField.text, this);
    }
    public void StartingHost()
    {
        relayScript.CreateRelay(this);
    }

    public void WriteHostCode(string str)
    {
        hostCode.text = str;
    }
}
