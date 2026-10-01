using System.ComponentModel.DataAnnotations;

namespace FutTech.Models;

public sealed class Aluno
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do aluno é obrigatório.")]
    [StringLength(300, ErrorMessage = "O nome deve ter no máximo 300 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O responsável é obrigatório.")]
    [StringLength(300, ErrorMessage = "O responsável deve ter no máximo 300 caracteres.")]
    public string Responsavel { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
    public DateOnly? DataNascimento { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecione uma turma.")]
    public int TurmaId { get; set; }

    public bool Ativo { get; set; } = true;
}