using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStar.Lib.Networking;

namespace SharpStar.Lib.DataTypes
{
    public class Json
    {
        private Json()
        {
        }

        public object Value { get; private set; }

        public Json(object value)
        {
            if (!(value == null ||
                value is float ||
                value is bool ||
                value is int ||
                value is string ||
                value is Json[] ||
                value is JsonDict))
            {
                throw new InvalidCastException(string.Format("Json is unable to represent {0}.", value.GetType()));
            }
            Value = value;
        }

        public static Json FromStream(IStarboundStream stream)
        {
            var json = new Json();
            var type = stream.ReadUInt8();
            switch (type)
            {
                case 0:
                    json.Value = null;
                    break;
                case 1:
                    json.Value = stream.ReadSingle();
                    break;
                case 2:
                    json.Value = stream.ReadBoolean();
                    break;
                case 3:
                    json.Value = stream.ReadInt32();
                    break;
                case 4:
                    json.Value = stream.ReadString();
                    break;
                case 5:
                    var array = new Json[stream.ReadVLQ()];
                    for (int i = 0; i < array.Length; i++)
                        array[i] = FromStream(stream);
                    json.Value = array;
                    break;
                case 6:
                    var dict = new JsonDict();
                    var length = stream.ReadVLQ();
                     while (length-- > 0)
                    {
                        dict[stream.ReadString()] = FromStream(stream);
                    }

                    json.Value = dict;
                    break;
                case 7:
                    var dict1 = new JsonDict();
                    var length2 = stream.ReadVLQ();
                    while (length2-- > 0)
                    {
                        dict1[stream.ReadString()] = FromStream(stream);
                    }

                    json.Value = dict1;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown Json type: 0x{0:X2}", type));
            }
            return json;
        }

        public void WriteTo(IStarboundStream stream)
        {
            if (Value == null)
            {
                stream.WriteUInt8(0);
            }
            else if (Value is float)
            {
                stream.WriteInt8(1);
                stream.WriteSingle((float)Value);
            }
            else if (Value is bool)
            {
                stream.WriteInt8(2);
                stream.WriteBoolean((bool)Value);
            }
            else if (Value is int)
            {
                stream.WriteInt8(3);
                stream.WriteInt32((int)Value);
            }
            else if (Value is string)
            {
                stream.WriteInt8(4);
                stream.WriteString((string)Value);
            }
            else if (Value is Json[])
            {
                stream.WriteInt8(5);
                var array = (Json[])Value;
                stream.WriteVLQ((ulong)array.Length);
                for (var i = 0; i < array.Length; i++)
                    array[i].WriteTo(stream);
            }
            else if (Value is JsonDict)
            {
                stream.WriteInt8(7);
                var dict = (JsonDict)Value;
                stream.WriteVLQ((ulong)dict.Count);
                foreach (var kvp in dict)
                {
                    stream.WriteString(kvp.Key);
                    kvp.Value.WriteTo(stream);
                }
            }
        }
    }

    public class JsonObject
    {

    }
}
