using JogoDaVelha2.Models;
using Microsoft.AspNetCore.Mvc;

namespace JogoDaVelha2.Controllers
{
    public class BaseController : Controller
    {
        public ObjectResult JsonReturnOnSucess(object obj)
        {
            return _JsonReturnOnSucess(obj, mensagem: null);
        }

        public ObjectResult JsonReturnOnSucess(string mensagem)
        {
            return _JsonReturnOnSucess(obj: null, mensagem);
        }

        public ObjectResult JsonReturnOnSucess(object obj, string mensagem)
        {
            return _JsonReturnOnSucess(obj, mensagem);
        }

        private ObjectResult _JsonReturnOnSucess(object obj = null, string mensagem = null)
        {
            var ret = new JsonReturnViewModel();
            ret.Ok = true;
            ret.Retorno = obj;
            ret.Mensagem = mensagem;
            return Ok(ret);
        }


        public ObjectResult JsonReturnOnError(Exception e)
        {
            var ret = new JsonReturnViewModel();
            ret.Ok = false;
            ret.Mensagem = $"Ocorreu um erro inesperado.";

            return Ok(ret);
        }
    }
}
