using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System;

public class NetworkClientManager : MonoBehaviour
{
    public static NetworkClientManager instance = null;

    [SerializeField]
    InputField ServerIP;

    int PlayerID = -1, CurrentMinigame = -1;

    bool Scene1 = true;

    [SerializeField]
    GameObject[] PlayersImages;

    [SerializeField]
    GameObject[] MiniGames;

    [SerializeField]
    GameObject Scene1UI, Scene2UI, ConnectButton, Victory, Defeat;

    NetworkClient client;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        client = new NetworkClient();
    }

    public void ConnectToServer()
    {
        client.Connect(ServerIP.text, 25000);

        client.RegisterHandler(888, ServerMessageReceiver);

        if (client.isConnected) ServerIP.text = "Connected";
        else ServerIP.text = "Connecting";
    }

    private void ServerMessageReceiver(NetworkMessage NetMsg)
    {
        StringMessage msg = new StringMessage();
        msg.value = NetMsg.ReadMessage<StringMessage>().value;
        string[] deltas = msg.value.Split('|');
        switch (deltas[0])
        {
            case "Player":
                int ToSetID;
                if (Int32.TryParse(deltas[1], out ToSetID))
                {
                    PlayerID = ToSetID;
                    PlayersImages[PlayerID - 1].SetActive(true);
                    ConnectButton.SetActive(false);
                    Scene2UI.transform.GetChild(0).GetChild(PlayerID).gameObject.SetActive(true);
                    SoundManager.GetInstance().PlaySound(0);
                }
                break;
            case "Start":
                SoundManager.GetInstance().PlaySound(1);
                Scene1UI.SetActive(true);
                Scene2UI.SetActive(false);
                PlayersImages[PlayerID - 1].SetActive(false);
                Scene1 = true;
                break;
            case "Scene1":
                SoundManager.GetInstance().SetRandom(true);
                DeactivateMinigames();
                Scene1UI.SetActive(true);
                Scene2UI.SetActive(false);
                Scene1 = true;
                break;
            case "Scene2":
                SoundManager.GetInstance().SetRandom(true);
                DeactivateMinigames();
                Scene1UI.SetActive(false);
                Scene2UI.SetActive(true);
                Scene1 = false;
                break;
            case "Fallen":
                SoundManager.GetInstance().SetRandom(false);
                SoundManager.GetInstance().PlaySound(2);
                CurrentMinigame = UnityEngine.Random.Range(0, MiniGames.Length);
                MiniGames[CurrentMinigame].SetActive(true);
                MiniGames[CurrentMinigame].GetComponent<MinigameScript>().Restart();
                Scene1UI.SetActive(false);
                Scene2UI.SetActive(false);
                break;
            case "GotUp":
                SoundManager.GetInstance().SetRandom(true);
                SoundManager.GetInstance().PlaySound(3);
                DeactivateMinigames();
                if (Scene1) Scene1UI.SetActive(true);
                else Scene2UI.SetActive(true);
                break;
            case "Won":
                SoundManager.GetInstance().PlaySound(6);
                DeactivateMinigames();
                Scene1UI.SetActive(false);
                Scene2UI.SetActive(false);
                Victory.SetActive(true);
                break;
            case "Lost":
                SoundManager.GetInstance().PlaySound(7);
                DeactivateMinigames();
                Scene1UI.SetActive(false);
                Scene2UI.SetActive(false);
                Defeat.SetActive(true);
                break;
            default:
                Debug.Log("Message");
                break;
        }
    }

    void DeactivateMinigames()
    {
        for (int i = 0; i < MiniGames.Length; i++)
        {
            MiniGames[i].SetActive(false);
            MiniGames[i].GetComponent<MinigameScript>().Finish();
        }
    }
    
    public void SendJoystickInfo(float HorDelta, float VerDelta)
    {
        if (client.isConnected)
        {
            Vector2 normalizer = new Vector2(HorDelta, VerDelta).normalized;
            StringMessage msg = new StringMessage();
            msg.value = "AnAx|" + normalizer.x + "|" + normalizer.y;
            client.Send(888, msg);

        }
    }

    public void SendButtonInfo(string ButtonPressed)
    {
        if (client.isConnected)
        {
            StringMessage msg = new StringMessage();
            msg.value = "Butt|" + ButtonPressed;
            client.Send(888, msg);
        }
    }

    public void SendGyroscopeInfo(string GyroInfo)
    {
        if (client.isConnected)
        {   
            // Delta.x <<0 tilted to the right, 0>> to the left, *(-1f) to mirror, <<0.25 tilted forwards, 0.25>> tilted backwards, -0.25 to center in 0
            // In the Server receive (-0.25,0.25), give a min treshold of 0.1 and a max value of 0.1 (more or less tilted)
            StringMessage msg = new StringMessage();
            msg.value = "Gyro|" + GyroInfo;
            client.Send(888, msg);

        }
    }

    public void SendGetUp()
    {
        if (client.isConnected)
        {
            StringMessage msg = new StringMessage();
            msg.value = "GetUp";
            client.Send(888, msg);

        }
    }

    public int ID()
    {
        return PlayerID;
    }

}
