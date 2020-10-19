﻿using D2NG.Core.D2GS.Enums;
using D2NG.Core.D2GS.Players;
using System;

namespace D2NG.Core.D2GS.Packet.Outgoing
{
    internal class PartyRequestPacket : D2gsPacket
    {
        public PartyRequestPacket(PartyRequestType requestType, Player player) :
            base(
                BuildPacket(
                    (byte)OutGoingPacket.InvitePlayer,
                    new byte[] { (byte)requestType },
                    BitConverter.GetBytes((uint)player.Id)
                )
            )
        {
        }
        public PartyRequestPacket(byte[] packet) : base(packet)
        {
        }
    }
}
