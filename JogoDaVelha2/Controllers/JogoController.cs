using JogoDaVelha2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JogoDaVelha2.Controllers
{
    [Authorize]
    public class JogoController : Controller
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
            Limpar();// exclui jogos criados a 1 hora ou mais

            var jogo = new Jogo(User.Claims.First().Value);
            jogo.InicializarJogo();
            jogos.Add(jogo);

            return Ok(jogo.JogoParaJogoSerializar());
        }

        [HttpGet("/jogo/buscarJogo/{guid}")]
        public IActionResult BuscarJogo(Guid guid)
        {
            Jogo? jogo = null;

            foreach (var j in jogos)
            {
                if (String.Equals(j.Id.ToString(), guid.ToString()))
                {
                    jogo = j;
                    continue;
                }
            }

            if (jogo != null)
                return Ok(jogo.JogoParaJogoSerializar());

            return NotFound();
        }

        [HttpGet("/jogo/convidarAmigo/{email}")]
        public IActionResult ConvidarAmigo(string email)
        {
            var user = _userManager.FindByEmailAsync(email);

            if(user.Result != null)
                return Ok(user);

            return NotFound();
        }

        [HttpGet("/jogo/jogar/{guid}/{linha}/{coluna}")]
        public IActionResult Jogar(Guid guid, int linha, int coluna)
        {
            Jogo? jogo = null;

            foreach (var j in jogos)
            {
                if (String.Equals(j.Id.ToString(),guid.ToString()))
                {
                    jogo = j;
                    jogo.StatusDaJogada = false;
                    if (jogo.CoordenadasValidas(linha, coluna))
                    {
                        jogo.Jogar(linha, coluna);
                        jogo.StatusDaJogada = true;

                        if (jogo.Ganhou())
                        {
                            jogo.Ganhador = jogo.JogadorAtual;
                            jogo.Mensagem = "Parabéns jogador " + jogo.JogadorAtual + ", você venceu!";
                        }
                        else if (jogo.Empatou())
                        {
                            jogo.Mensagem = "Poxa, empatou! Vocês são muito ruins!";
                            jogo.Ganhador = 'e';
                        }
                        else
                        {
                            jogo.ProximoJogador();
                        }
                    }
                    continue;
                }
            }

            if (jogo != null)
                return Ok(jogo.JogoParaJogoSerializar());

            return NotFound();
        }

        private void Limpar()
        {
            // fonte: https://makolyte.com/system-invalidoperationexception-collection-was-modified-enumeration-operation-may-not-execute/
            jogos.RemoveAll(jogo => jogo.PodeExcluir());

        }
    }
}
