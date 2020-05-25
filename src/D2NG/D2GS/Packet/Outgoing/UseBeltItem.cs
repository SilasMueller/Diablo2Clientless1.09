﻿using D2NG.D2GS.Items;
using System;

namespace D2NG.D2GS.Packet
{
    internal class UseBeltItem : D2gsPacket
    {
        public UseBeltItem(Item item) :
            base(
                BuildPacket(
                    (byte)OutGoingPacket.UseBeltItem,
                    BitConverter.GetBytes((uint)item.Id),
                    BitConverter.GetBytes((uint)0x00),
                    BitConverter.GetBytes((uint)0x00)
                )
            )
        {
        }
        public UseBeltItem(byte[] packet) : base(packet)
        {
        }

        public uint GetItemId()
        {
            return BitConverter.ToUInt32(Raw, 5);
        }
    }
}
