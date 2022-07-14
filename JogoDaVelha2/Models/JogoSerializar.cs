using System.Runtime.Serialization;

namespace JogoDaVelha2.Models
{
    [DataContract]
    public class JogoSerializar
    {
        [DataMember]
        public Guid Id { get; set; }

        public string IdUser { get; set; }

        [DataMember]
        public char[] Vetor { get; set; }

        [DataMember]
        public char JogadorAtual { get; set; }

        [DataMember]
        public char Ganhador { get; set; }

        [DataMember]
        public string Mensagem { get; set; }

        public JogoSerializar()
        {
            Vetor = new char[9];
        }
    }
}