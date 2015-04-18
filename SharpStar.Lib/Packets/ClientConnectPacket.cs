// SharpStar
// Copyright (C) 2014 Mitchell Kutchuk
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using SharpStar.Lib.DataTypes;
using SharpStar.Lib.Logging;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets
{
    //Credit to StarNet (https://github.com/SirCmpwn/StarNet)
    public class ClientConnectPacket : Packet
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.ClientConnect; }
        }

        public byte[] AssetDigest;
        public byte[] UUID;
        public string PlayerName;
        public string Species;
        public byte[] Shipworld;
        public ShipUpgrades ShipUpgrade;
        public string Account { get; set; }
       // public byte[] Shipupgrades;
      // 
        //public string Account;

        //public Variant Claim; //remove?
        
        public override void Read(IStarboundStream stream)
        {
           // AssetDigest = stream.ReadToEnd();
            
            AssetDigest = stream.ReadUInt8Array();
            SharpStarLogger.DefaultLogger.Info(string.Format("AsD:{0}", AssetDigest.Length));
            UUID = stream.ReadUInt8Array(16);
            SharpStarLogger.DefaultLogger.Info(string.Format("uuid:{0}", UUID.Length));
            PlayerName = stream.ReadString();
            SharpStarLogger.DefaultLogger.Info(string.Format("player name:{0}", PlayerName));
            Species = stream.ReadString();
            SharpStarLogger.DefaultLogger.Info(string.Format("species:{0}", Species));
            Shipworld = stream.ReadUInt8Array();
            SharpStarLogger.DefaultLogger.Info(string.Format("Sw:{0}", Shipworld.Length));
            ShipUpgrade = ShipUpgrades.FromStream(stream);
            Account = stream.ReadString();
            SharpStarLogger.DefaultLogger.Info(string.Format("Account:{0}", Account));
            //  Shipupgrades = stream.ReadToEnd();
            //SharpStarLogger.DefaultLogger.Info(string.Format("Sw:{0}", Shipupgrades.Length));
            // ShipUpgrade = ShipUpgrades.FromStream(stream);
            // SharpStarLogger.DefaultLogger.Info(string.Format("AsD:{0},{1},{2}", ShipUpgrade.shipLevel.Value,ShipUpgrade.maxFuel.Value,ShipUpgrade.Capabilites.Length));

            //Account = stream.ReadString();
            // SharpStarLogger.DefaultLogger.Info(string.Format("AsD:{0}UUID:{1}PN:{2}SP:{3}SHW:{4}SHU:{5}"UUID,PlayerName,Shipworld,ShipUpgrade));

            /*
            AssetDigest = stream.ReadUInt8Array();
         //   Claim = stream.ReadVariant();
            bool uuid = stream.ReadBoolean();
            if (uuid)
                UUID = stream.ReadUInt8Array(16);
            PlayerName = stream.ReadString();
            Species = stream.ReadString();
            Shipworld = stream.ReadUInt8Array();
            //todo shipupgrades

            
             */
        }

        public override void Write(IStarboundStream stream)
        {
            stream.Write(AssetDigest,0,AssetDigest.Length);
            /*
            stream.WriteUInt8Array(AssetDigest);
            stream.WriteUInt8Array(UUID);
            stream.WriteString(PlayerName);
            stream.WriteString(Species);
            stream.Write(Shipworld,0,Shipworld.Length);
             * 
             */

          //  ShipUpgrade.WriteTo(stream);
          //  ShipUpgrade.WriteTo(stream);
           // stream.WriteString(Account);
            /*
            stream.WriteUInt8Array(AssetDigest);
         //   stream.WriteVariant(Claim);
            stream.WriteBoolean(UUID != null);
            if (UUID != null)
                stream.WriteUInt8Array(UUID, false);
            stream.WriteString(PlayerName);
            stream.WriteString(Species);
            stream.WriteUInt8Array(Shipworld);
            //todo shipupgrades

            stream.WriteString(Account);
          * */
        }
    }
}