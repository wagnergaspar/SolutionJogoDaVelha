using JogoDaVelha2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JogoDaVelha2.Controllers
{
    [Authorize]
    public class JogoController : BaseController
    {
        private static List<Jogo> jogos = new List<Jogo>();

        private readonly UserManager<IdentityUser> _userManager;

        public JogoController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/jogo/novoJogo")]
        public IActionResult NovoJogo(int id)
        {
            var jogo = new Jogo();
            jogo.InicializarJogo();
            jogo.IdUser = User.Claims.First().Value;
            jogos.Add(jogo);

            var jogoSerializavel = jogo.JogoParaJogoSerializar();

            return JsonReturnOnSucess(jogoSerializavel);
        }

        [HttpGet("/jogo/buscarJogo/{guid}")]
        public IActionResult BuscarJogo(Guid guid)
        {
            Jogo? jogo = null;
            JogoSerializar? jogoSerializavel = null;

            foreach (var j in jogos)
            {
                if (String.Equals(j.Id.ToString(), guid.ToString()))
                {
                    jogo = j;
                    continue;
                }
            }

            if (jogo != null)
                jogoSerializavel = jogo.JogoParaJogoSerializar();

            return JsonReturnOnSucess(jogoSerializavel);
        }

        [HttpGet("/jogo/convidarAmigo/{email}")]
        public IActionResult ConvidarAmigo(string email)
        {
            var user = _userManager.FindByEmailAsync(email);

            return JsonReturnOnSucess(user);
        }

        [HttpGet("/jogo/jogar/{guid}/{linha}/{coluna}")]
        public IActionResult Jogar(Guid guid, int linha, int coluna)
        {
            Jogo? jogo = null;
            JogoSerializar? jogoSerializavel = null;

            foreach (var j in jogos)
            {
                if (String.Equals(j.Id.ToString(),guid.ToString()))
                {
                    jogo = j;
                    if (jogo.CoordenadasValidas(linha, coluna))
                    {
                        jogo.Jogar(linha, coluna);

                        if (jogo.Ganhou())
                        {
                            jogo.Ganhador = jogo.JogadorAtual;
                            jogo.Mensagem = "Parabéns jogador " + jogo.JogadorAtual + ", você venceu!";
                        }
                        else if (jogo.Empatou())
                        {
                            jogo.Mensagem = "Poxa, empatou! Vocês são muito ruins!";
                        }
                        else
                        {
                            jogo.ProximoJogador();
                        }
                    }
                    continue;
                }
            }

            if(jogo != null)
                jogoSerializavel = jogo.JogoParaJogoSerializar();

            return JsonReturnOnSucess(jogoSerializavel);
        }
    }
}
