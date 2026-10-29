using System.ComponentModel.DataAnnotations;

namespace API.Endpoints;

/// <summary>
/// En Controllers, [ApiController] valida las anotaciones y responde 400 solo.
/// En Minimal API eso NO ocurre por defecto, así que se valida a mano y se devuelve el mismo formato de error.
/// ****PROFESOR: **** Investigación: .NET 10 agrega AddValidation() para Minimal API. Aquí se hace a mano
/// para ver qué hace [ApiController] por nosotros.
/// </summary>
public static class ValidacionHelper
{
    public static bool EsValido(object modelo, out Dictionary<string, string[]> errores)
    {
        var resultados = new List<ValidationResult>();
        var contexto = new ValidationContext(modelo);
        Validator.TryValidateObject(modelo, contexto, resultados, validateAllProperties: true);

        errores = resultados
            .SelectMany(r => (r.MemberNames.Any() ? r.MemberNames : new[] { string.Empty })
                .Select(m => (Campo: m, Mensaje: r.ErrorMessage ?? "Valor inválido.")))
            .GroupBy(x => x.Campo)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Mensaje).ToArray());

        return errores.Count == 0;
    }
}
