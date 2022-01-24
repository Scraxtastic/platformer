using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal class OnlinePlayer
    {
        public int Id { get; set; }
        public PlayerData PlayerData { get; set; }
        public GameObject PlayerObject { get; set; }
        public Rigidbody2D Rigidbody { get; set; }

        public static implicit operator GameObject(OnlinePlayer v)
        {
            throw new NotImplementedException();
        }
    }
}
