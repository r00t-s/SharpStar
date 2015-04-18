using SharpStar.Lib.Networking;

namespace SharpStar.Lib.Packets 
{
    public class ChatSendPacket : Packet 
    {
        public override byte PacketId
        {
            get { return (byte)KnownPacket.ChatSend; }
        }

        public string Text { get; set; }

        public ChatSendMode cm { get; set; }

        public override void Read(IStarboundStream stream)
        {
            
            Text = stream.ReadString();
            cm = (ChatSendMode)stream.ReadUInt8();
            
        }

        public override void Write(IStarboundStream stream)
        {
            stream.WriteString(Text);
            stream.WriteUInt8((byte)cm);
        }
    }

    public enum ChatSendMode
    {
        Broadcast,
        Local,
        Party 
    }
}
