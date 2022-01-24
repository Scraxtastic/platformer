using Assets.Scripts;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandleIncomingMessages : MonoBehaviour
{
    public GameObject Prefab;
    public ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
    public int MyPlayerId { get; set; } = -1;
    private List<OnlinePlayer> _OnlinePlayers = new List<OnlinePlayer>();
    private Rigidbody2D _Rigidbody;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        HandleQueue();
    }

    public void HandleQueue()
    {
        if (queue.Count == 0) return;
        if (queue.TryDequeue(out string data))
        {
            Debug.Log(data);
            if (data.Length == 0) return;
            NetworkData netData = JsonConvert.DeserializeObject<NetworkData>(data);
            if (netData.Id == MyPlayerId) return;
            if (netData.data.Length == 0) return;
            PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(netData.data);
            bool exists = false;
            for (int i = 0; i < _OnlinePlayers.Count; i++)
            {
                if (_OnlinePlayers[i].Id == netData.Id)
                {
                    _OnlinePlayers[i].PlayerData = playerData;
                    GameObject p = _OnlinePlayers[i].PlayerObject;
                    Rigidbody2D rb = _OnlinePlayers[i].Rigidbody;
                    rb.position = playerData.pos;
                    rb.velocity = playerData.vel;
                    rb.angularVelocity = playerData.angvel;
                    rb.rotation = playerData.rotation;
                    exists = true;

                }
            }
            if (!exists)
            {
                GameObject oPlayer = Instantiate(Prefab);
                oPlayer.transform.parent = this.transform;
                OnlinePlayer connectedPlayer = new OnlinePlayer();
                connectedPlayer.Id = netData.Id;
                connectedPlayer.PlayerData = playerData;
                connectedPlayer.PlayerObject = oPlayer;
                Rigidbody2D rb = oPlayer.transform.GetComponent<Rigidbody2D>();
                connectedPlayer.Rigidbody = rb;
                _OnlinePlayers.Add(connectedPlayer);
                rb.position = playerData.pos;
                rb.velocity = playerData.vel;
                rb.angularVelocity = playerData.angvel;
            }
        }
    }

    public void ReceiveMessage(string data)
    {
        if (data.StartsWith("start"))
        {

        }
        else
        {
            queue.Enqueue(data);
        }
    }
}
