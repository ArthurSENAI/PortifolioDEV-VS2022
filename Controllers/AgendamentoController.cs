using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortifolioDEV.Models;
using PortifolioDEV.ORM;
using PortifolioDEV.Repositorios;
using System.Diagnostics;

namespace PortifolioDEV.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly AgendamentoRepositorio _agendamentoRepositorio;        
        private BdPortfolioDevContext _context;

        public AgendamentoController(AgendamentoRepositorio agendamentoRepositorio, BdPortfolioDevContext context)
        {
            _agendamentoRepositorio = agendamentoRepositorio;           
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            var servicos = new ServicoRepositorio(_context);
            var nomeServicos = servicos.ListarNomesServicos();
            if (nomeServicos != null && nomeServicos.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = nomeServicos.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),  // O valor do item ser� o ID do usu�rio
                    Text = u.TipoServico             // O texto exibido ser� o nome do usu�rio
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.lstTipoServico = selectList;
            }

            // Buscar os usu�rios e servi�os no banco de dados
            var usuarios = _agendamentoRepositorio.ListarNomesAgendamentos();
            if (usuarios != null && usuarios.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = usuarios.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),  // O valor do item ser� o ID do usu�rio
                    Text = u.Nome             // O texto exibido ser� o nome do usu�rio
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.Usuarios = selectList;
            }

            var listaHorario = new List<SelectListItem>
            {
                 new SelectListItem { Value = "8", Text = "08:00:00" },
                 new SelectListItem { Value = "10", Text = "10:00:00" },
                 new SelectListItem { Value = "13", Text = "13:00:00" },
                 new SelectListItem { Value = "15", Text = "15:00:00" },
                 new SelectListItem { Value = "17", Text = "17:00:00" },
                 new SelectListItem { Value = "19", Text = "19:00:00" }
            };

            ViewBag.lstHorarios = listaHorario;

            // Buscar os agendamentos e incluir os nomes de Usu�rio e Servi�o
            var agendamentos = _agendamentoRepositorio.ListarAgendamentos();
            return View(agendamentos);
        }

        [Authorize]
        public IActionResult Cadastro()
        {
            var servicos = new ServicoRepositorio(_context);
            var nomeServicos = servicos.ListarNomesServicos();
            if (nomeServicos != null && nomeServicos.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = nomeServicos.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),  // O valor do item ser� o ID do usu�rio
                    Text = u.TipoServico             // O texto exibido ser� o nome do usu�rio
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.lstTipoServico = selectList;
            }

            return View();
        }

        [Authorize]
        public IActionResult Gerenciamento_Agendamento_Usuario()
        {
            var servicos = new ServicoRepositorio(_context);
            var nomeServicos = servicos.ListarNomesServicos();
            if (nomeServicos != null && nomeServicos.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = nomeServicos.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),  // O valor do item ser� o ID do usu�rio
                    Text = u.TipoServico             // O texto exibido ser� o nome do usu�rio
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.lstTipoServico = selectList;
            }

            // Buscar os usu�rios e servi�os no banco de dados
            var usuarios = _agendamentoRepositorio.ListarNomesAgendamentos();
            if (usuarios != null && usuarios.Any())
            {
                // Cria a lista de SelectListItem
                var selectList = usuarios.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),  // O valor do item ser� o ID do usu�rio
                    Text = u.Nome             // O texto exibido ser� o nome do usu�rio
                }).ToList();

                // Passa a lista para o ViewBag para ser utilizada na view
                ViewBag.Usuarios = selectList;
            }

            var listaHorario = new List<SelectListItem>
            {
                 new SelectListItem { Value = "8", Text = "08:00:00" },
                 new SelectListItem { Value = "10", Text = "10:00:00" },
                 new SelectListItem { Value = "13", Text = "13:00:00" },
                 new SelectListItem { Value = "15", Text = "15:00:00" },
                 new SelectListItem { Value = "17", Text = "17:00:00" },
                 new SelectListItem { Value = "19", Text = "19:00:00" }
            };

            ViewBag.lstHorarios = listaHorario;

            // Buscar os agendamentos e incluir os nomes de Usu�rio e Servi�o
            var agendamentos = _agendamentoRepositorio.ListarAgendamentosClientes();
            return View(agendamentos);
        }

        public IActionResult ConsultarAgendamento(string data)
        {

            var agendamento = _agendamentoRepositorio.ConsultarAgendamento(data);

            if (agendamento != null)
            {
                return Json(agendamento);
            }
            else
            {
                return NotFound();
            }

        }

        public IActionResult InserirAgendamento(DateTime dtHoraAgendamento, DateOnly dataAtendimento, TimeOnly horario, int IdUsuario, int IdServico)
        {
            try
            {
                // Chama o m�todo do reposit�rio que realiza a inser��o no banco de dados
                var resultado = _agendamentoRepositorio.InserirAgendamento(dtHoraAgendamento, dataAtendimento, horario, IdUsuario, IdServico);

                // Verifica o resultado da inser��o
                if (resultado)
                {
                    return Json(new { success = true, message = "Atendimento inserido com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao inserir o atendimento. Tente novamente." });
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado, captura e exibe o erro
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }

        public IActionResult InserirAgendamentoCliente(DateTime dtHoraAgendamento, DateOnly dataAtendimento, TimeOnly horario, int IdUsuario, int IdServico)
        {
            int IdUsuarioLocal = HttpContext.Session.GetInt32("USUARIO_ID") ?? 0;
            try
            {
                // Chama o m�todo do reposit�rio que realiza a inser��o no banco de dados
                var resultado = _agendamentoRepositorio.InserirAgendamento(dtHoraAgendamento, dataAtendimento, horario, IdUsuarioLocal, IdServico);

                // Verifica o resultado da inser��o
                if (resultado)
                {
                    return Json(new { success = true, message = "Atendimento inserido com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao inserir o atendimento. Tente novamente." });
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado, captura e exibe o erro
                return Json(new { success = false, message = "Erro ao processar a solicita��o. Detalhes: " + ex.Message });
            }
        }

        public IActionResult AlterarAgendamento(int id, string data, int servico, TimeOnly horario)
        {

            var rs = _agendamentoRepositorio.AlterarAgendamento(id, data, servico, horario);
            if (rs)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public IActionResult ExcluirAgendamento(int id)
        {

            var rs = _agendamentoRepositorio.ExcluirAgendamento(id);
            return Json(new { success = rs });

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
