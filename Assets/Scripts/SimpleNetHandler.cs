using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Newtonsoft.Json;
using Assets.Scripts;

public class SimpleNetHandler : MonoBehaviour
{
    public GameObject Player;
    public GameObject OtherPlayers;

    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    private HandleIncomingMessages handleIncomingMessages;
    private PlayerData _PlayerData;
    private float _LastSentTime = 0;
    private float _TimeTillNextSend = 1;
    private string _JsonString = "";
    private bool _IsConnected = false;
    private Rigidbody2D _PlayerRigidBody;
    private bool _ReceivedId = false;
    // Use this for initialization 	

    private void Awake()
    {

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
    }
    void Start()
    {
        Player.SetActive(false);
        _PlayerRigidBody = Player.GetComponent<Rigidbody2D>();
        _PlayerData = new PlayerData();
        handleIncomingMessages = OtherPlayers.GetComponent<HandleIncomingMessages>();
        ConnectToTcpServer();
    }
    // Update is called once per frame
    void Update()
    {
        if (_IsConnected)
        {
            Player.SetActive(true);
        }
        if (Time.time > _LastSentTime + _TimeTillNextSend)
        {
            _PlayerData.pos = _PlayerRigidBody.position;
            _PlayerData.vel = _PlayerRigidBody.velocity;
            _PlayerData.angvel = _PlayerRigidBody.angularVelocity;
            _PlayerData.rotation = _PlayerRigidBody.rotation;
            _JsonString = JsonConvert.SerializeObject(_PlayerData);
            SendPosition();
            _LastSentTime = Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        socketConnection.Close();
        clientReceiveThread.Abort();
    }
    /// <summary> 	
    /// Setup socket connection. 	
    /// </summary> 	
    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }
    /// <summary> 	
    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    /// </summary>     
    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient("localhost", 8080);
            _IsConnected = true;
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 						
                        string serverMessage = Encoding.ASCII.GetString(incommingData);
                        if (!_ReceivedId && serverMessage.StartsWith("start:"))
                        {
                            if (int.TryParse(serverMessage.Substring(6, serverMessage.Length-6), out int id))
                            {
                                handleIncomingMessages.MyPlayerId = id;
                            }
                        }
                        else
                        {
                            handleIncomingMessages.ReceiveMessage(serverMessage);
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
    /// <summary> 	
    /// Send message to server using socket connection. 	
    /// </summary> 	
    private void SendPosition()
    {
        if (socketConnection == null || !socketConnection.Connected)
        {
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                // Convert string message to byte array.                 
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(_JsonString);
                // Write byte array to socketConnection stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}
