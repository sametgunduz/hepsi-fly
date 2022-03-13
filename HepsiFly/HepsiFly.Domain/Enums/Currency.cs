using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace HepsiFly.Domain.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum Currency
{
    TL,
    USD,
    EUR,
    GBP
}