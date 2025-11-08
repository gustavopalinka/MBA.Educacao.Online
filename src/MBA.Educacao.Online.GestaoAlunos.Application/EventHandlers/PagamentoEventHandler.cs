using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MBA.Educacao.Online.GestaoAlunos.Domain;
using MBA.Educacao.Online.Pagamentos.Domain;
using MediatR;

namespace MBA.Educacao.Online.GestaoAlunos.Application.EventHandlers;

public class PagamentoEventHandler :
    INotificationHandler<PagamentoConfirmadoEvent>,
    INotificationHandler<PagamentoRejeitadoEvent>
{
    private readonly IAlunoRepository _alunoRepository;

    public PagamentoEventHandler(IAlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    public async Task Handle(PagamentoConfirmadoEvent notification, CancellationToken cancellationToken)
    {
        var aluno = await _alunoRepository.ObterAlunoComMatriculas(notification.AlunoId);

        if (aluno is null)
        {
            return;
        }

        var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == notification.MatriculaId);

        if (matricula is null)
        {
            return;
        }

        matricula.Ativar();
        await _alunoRepository.UnitOfWork.Commit();
    }

    public async Task Handle(PagamentoRejeitadoEvent notification, CancellationToken cancellationToken)
    {
        var aluno = await _alunoRepository.ObterAlunoComMatriculas(notification.AlunoId);

        if (aluno is null)
        {
            return;
        }

        var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == notification.MatriculaId);

        if (matricula is null)
        {
            return;
        }

        matricula.Cancelar();
        await _alunoRepository.UnitOfWork.Commit();
    }
}

