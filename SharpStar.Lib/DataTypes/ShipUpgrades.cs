using System;
using System.Collections.Generic;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.DataTypes
{
    public class ShipUpgrades
    {
        public Json shipLevel { get; set; }
        public Json maxFuel { get; set; }
        //public List<string> capabilities{ get; set; }

        public static List<String> shipUpgrades { get; set; }
        

        public byte[] Capabilites { get; set; }
        
        public ShipUpgrades()
        {
            
        }

        public static ShipUpgrades FromStream(IStarboundStream stream)
        {
            var su = new ShipUpgrades();
            su.shipLevel = Json.FromStream(stream);
            su.maxFuel = Json.FromStream(stream);
            su.Capabilites = stream.ReadUInt8Array();
            
            string temp = null; //todo this is really stupid. fixme
            foreach (var s in su.Capabilites)
            {
                temp += s;
            }

            //su.bah = stream.WriteVariant()
            //  su.testingSet = stream.ReadHashSet();
                /*
            foreach (string s in test)
            {
            }
                 */
            return su;
        }

        public void WriteTo(IStarboundStream stream)
        {
           shipLevel.WriteTo(stream);
           maxFuel.WriteTo(stream);
           
           //stream.WriteUInt8Array(Capabilites);
           //stream.WriteHashSet(testingSet);
        }
    }
}
