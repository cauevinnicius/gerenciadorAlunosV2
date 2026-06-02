using GerenciadorAlunosV2.Models;
using GerenciadorAlunosV2.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorAlunosV2.Repositories;

public class MensalidadeRepository

{
    private readonly GerenciadorAlunosDbContext _context;

    public MensalidadeRepository(GerenciadorAlunosDbContext context)
    {
        _context = context;
    }

    public async Task LancarMensalidadeAsync(MensalidadeModel novaMensalidade)
    {
        novaMensalidade.DataVencimento = DateTime.Now.AddDays(30);
        _context.Mensalidades.Add(novaMensalidade);
        await _context.SaveChangesAsync();
    }

    public async Task RegistrarPagamentoAsync(int idMensalidade, DateTime dataPagamento)
    {
        var mensalidade = await _context.Mensalidades.FindAsync(idMensalidade);

        if (mensalidade != null)
        {
            mensalidade.Status = "pago";
            mensalidade.DataPagamento = dataPagamento;
            await _context.SaveChangesAsync();
        }
    }
    public async Task<List<MensalidadeModel>> ListarMensalidadesAsync()
    {
        return await _context.Mensalidades.ToListAsync();
    }

    // a escrita de retorno está dessa forma para fins de organização. De fato, seria posto tudo em uma única linha
    public async Task<List<MensalidadeModel>> VerificaPendenciasAsync(int alunoId)
    {
        return await _context.Mensalidades
            .Where(m => m.AlunoId == alunoId && m.Status == "pendente")
            .ToListAsync();
    }

    public async Task<bool> EditarMensalidadeAsync(MensalidadeModel mensalidadeEditada)
    {
        _context.Mensalidades.Update(mensalidadeEditada);
        return await _context.SaveChangesAsync() > 0; // salvar apenas se for maior que zero
    }

    public async Task ExcluirMensalidadeAsync(int id)
    {
        var mensalidade = await _context.Mensalidades.FindAsync(id);
        if (mensalidade != null)
        {
            _context.Mensalidades.Remove(mensalidade);
            await _context.SaveChangesAsync();
        }
    }
}