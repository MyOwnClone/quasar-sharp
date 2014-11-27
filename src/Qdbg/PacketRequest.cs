using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GruntXProductions.Quasar.Debugger
{
    public class PacketRequest : IPacket
    {
        private DebugRequest request;
        private uint field0;
        private uint field1;
        private uint field2;
        private uint dataSize;
        private byte[] data;

        public DebugRequest Request
        {
            get
            {
                return this.request;
            }
        }

        public uint Argument0
        {
            get
            {
                return this.field0;
            }
        }

        public uint Argument1
        {
            get
            {
                return this.field1;
            }
        }

        public uint Argument2
        {
            get
            {
                return this.field2;
            }
        }

        public byte[] Data
        {
            get
            {
                return this.data;
            }
        }

        public PacketRequest()
        {
        }

        public PacketRequest(DebugRequest req, uint f1, uint f2, uint f3, uint dsize, byte[] data)
        {
            this.request = req;
            this.field0 = f1;
            this.field1 = f2;
            this.field2 = f3;
            this.dataSize = dsize;
            this.data = data;
        }

        public void Send(Stream str)
        {
            BinaryWriter bw = new BinaryWriter(str);
            bw.Write((byte)request);
            bw.Write(field0);
            bw.Write(field1);
            bw.Write(field2);
            bw.Write(dataSize);
            bw.Write(data);
        }

        public void Recieve(Stream str)
        {
            BinaryReader br = new BinaryReader(str);
            this.request = (DebugRequest)br.ReadByte();
            this.field0 = br.ReadUInt32();
            this.field1 = br.ReadUInt32();
            this.field2 = br.ReadUInt32();
            this.dataSize = br.ReadUInt32();
            this.data = br.ReadBytes((int)this.dataSize);
        }
    }

}
