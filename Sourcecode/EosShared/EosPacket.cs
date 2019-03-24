using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace EosShared
{
    [Serializable]
    public class EosPacket
    {

        /// <summary>
        /// The IP of the sender
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Pdata - Data we append to the sender
        /// </summary>
        public List<byte[]> Data { get; set; }

        /// <summary>
        /// The IP of the receiver
        /// </summary>
        public EosPacketType Type { get; set; }

        public byte[] ToBytes()
        {
            BinaryFormatter b = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            b.Serialize(ms, this);
            byte[] bytes = ms.ToArray();
            ms.Close();

            return bytes;
        }

        public EosPacket()
        {
            Data = new List<byte[]>();
        }

        public EosPacket(byte[] bytes)
        {
            BinaryFormatter b = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(bytes);

            EosPacket p = (EosPacket)b.Deserialize(ms);
            this.Data = p.Data;
            this.Sender = p.Sender;
            this.Type = p.Type;
        }

        public override string ToString()
        {
            return Sender + " " + Type;
        }
    }
}
