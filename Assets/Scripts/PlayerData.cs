using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    [Serializable]
    internal class PlayerData
    {
        public SerializableVector2 pos { get; set; }
        public SerializableVector2 vel { get; set; }
        public float angvel { get; set; }
        public float rotation { get; set; }
    }
}
