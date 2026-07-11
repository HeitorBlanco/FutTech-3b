namespace FutTech.Models;

public sealed class MenuItemSistema
{
    public string Chave { get; set; } = string.Empty;
    public string Texto { get; set; } = string.Empty;
    public string Url { get; set; } = "#";
    public string IconePath { get; set; } = string.Empty;
    public bool Disponivel { get; set; } = true;
}
