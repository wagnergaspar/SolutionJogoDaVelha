using System.Runtime.Serialization;

namespace JogoDaVelha2.Models
{
    public class Jogo
    {
        public Guid Id { get; set; }

        public bool StatusDaJogada { get; set; }

        public string IdUser { get; set; }

        public char[,] Matriz { get; set; }

        public char JogadorAtual { get; set; }

        public char Ganhador { get; set; }

        public string Mensagem { get; set; }

        public DateTime Data { get; set; }

        public int TamanhoLista { get; set; }

        public Jogo(String user)
        {
            Id = Guid.NewGuid();
            StatusDaJogada = false;
            IdUser = user;
            Matriz = new char[3, 3];
            JogadorAtual = 'x';
            Ganhador = '-';
            Mensagem = "O jogador X inicia. Bom jogo!";
            Data = DateTime.Now;
        }

        public void InicializarJogo()
        {
            int i, j;

            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    Matriz[i, j] = '-';
                }
            }
        }

        public void ProximoJogador()
        {
            if (JogadorAtual == 'x')
                JogadorAtual = '0';
            else
                JogadorAtual = 'x';
            Mensagem = "Agora é sua vez jogador " + JogadorAtual;
        }

        public bool CoordenadasValidas(int linha, int coluna)
        {
            if (linha >= 0 && linha <= 2 && coluna >= 0 && coluna <= 2)
            {
                if (Matriz[linha, coluna] == '-')
                {
                    return true;
                }
            }
            return false;
        }

        public void Jogar(int linha, int coluna)
        {
            Matriz[linha, coluna] = JogadorAtual;
        }

        public bool Ganhou()
        {
            int i, j, ganhou = 0;

            #region Diagonal Principal

            if (Matriz[0, 0] != '-' & Matriz[0, 0] == JogadorAtual & Matriz[1, 1] == JogadorAtual & Matriz[2, 2] == JogadorAtual)
                return true;

            #endregion

            #region Diagonal secundaria

            if (Matriz[0, 2] != '-' & Matriz[0, 2] == JogadorAtual & Matriz[1, 1] == JogadorAtual & Matriz[2, 0] == JogadorAtual)
                return true;

            #endregion

            #region Linhas

            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    if (Matriz[i, j] == JogadorAtual)
                        ganhou++;
                }
                if (ganhou == 3)
                    return true;
                ganhou = 0;
            }

            #endregion

            #region Colunas

            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    if (Matriz[j, i] == JogadorAtual)
                        ganhou++;
                }
                if (ganhou == 3)
                    return true;
                ganhou = 0;
            }

            #endregion

            return false;
        }

        public bool Empatou()
        {
            int i, j, vazias = 0;

            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    if (Matriz[i, j] == '-')
                        vazias++;
                }
            }

            if (vazias == 0)
                return true;
            return false;
        }

        public JogoSerializar JogoParaJogoSerializar()
        {
            var jogo = new JogoSerializar();

            jogo.Id = this.Id;
            jogo.StatusDaJogada = this.StatusDaJogada;
            jogo.IdUser = this.IdUser;
            jogo.JogadorAtual = this.JogadorAtual;
            jogo.Ganhador = this.Ganhador;
            jogo.Mensagem = this.Mensagem;
            jogo.TamanhoLista = this.TamanhoLista;

            jogo.Vetor[0] = this.Matriz[0, 0];
            jogo.Vetor[1] = this.Matriz[0, 1];
            jogo.Vetor[2] = this.Matriz[0, 2];

            jogo.Vetor[3] = this.Matriz[1, 0];
            jogo.Vetor[4] = this.Matriz[1, 1];
            jogo.Vetor[5] = this.Matriz[1, 2];

            jogo.Vetor[6] = this.Matriz[2, 0];
            jogo.Vetor[7] = this.Matriz[2, 1];
            jogo.Vetor[8] = this.Matriz[2, 2];

            return jogo;
        }

        public bool PodeExcluir()
        {
            TimeSpan intervalo = DateTime.Now - this.Data;
            int horas = intervalo.Hours;

            // exclui jogos com mais de 1 hora de criação
            if (horas >= 1)
            {
                return true;
            }
            return false;
        }
    }
}