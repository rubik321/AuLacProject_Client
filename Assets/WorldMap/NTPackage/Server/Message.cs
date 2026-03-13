namespace NTPackage_old.Server{
    [System.Serializable]
    public class Message
    {
        public MessageCode MessageCode;
        public string Data;

        public Message(){}

        public Message(MessageCode messageCode, string data){
            this.MessageCode = messageCode;
            this.Data = data;
        }
    }
}
