using FutTech.Models;

namespace FutTech.Services;

public sealed class DemoComunicadoService
{
    private readonly List<Comunicado> _comunicados =
    [
        new()
        {
            Id = 1,
            Titulo = "Treino cancelado amanhã 04/07/2026",
            Conteudo = "Motivos de saúde.",
            PublicadoEm = new DateOnly(2026, 7, 3),
            PublicadoAs = new TimeOnly(19, 13),
            Autor = "Carlos Admin",
            Categoria = "Geral",
            Destacado = true
        }
    ];

    public IReadOnlyList<Comunicado> ListarComunicados()
    {
        return _comunicados
            .Where(comunicado => comunicado.Ativo)
            .OrderByDescending(comunicado => comunicado.PublicadoEm)
            .ThenByDescending(comunicado => comunicado.PublicadoAs)
            .ToList();
    }

    public Comunicado CadastrarComunicado(
        string titulo,
        string conteudo,
        string categoria,
        string autor)
    {
        var agora = DateTime.Now;
        var comunicado = new Comunicado
        {
            Id = _comunicados.Count == 0 ? 1 : _comunicados.Max(item => item.Id) + 1,
            Titulo = titulo.Trim(),
            Conteudo = conteudo.Trim(),
            PublicadoEm = DateOnly.FromDateTime(agora),
            PublicadoAs = TimeOnly.FromDateTime(agora),
            Autor = autor.Trim(),
            Categoria = string.IsNullOrWhiteSpace(categoria) ? "Geral" : categoria.Trim(),
            Destacado = false,
            Ativo = true
        };

        _comunicados.Add(comunicado);
        return comunicado;
    }

    public bool RemoverComunicado(int comunicadoId)
    {
        var comunicado = _comunicados.FirstOrDefault(item => item.Id == comunicadoId && item.Ativo);

        if (comunicado is null)
        {
            return false;
        }

        comunicado.Ativo = false;
        return true;
    }
}
