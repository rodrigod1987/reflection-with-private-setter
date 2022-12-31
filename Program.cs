// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

Console.WriteLine("Hello, World!");
var jsonNome = "{\"nome\": \"Rodrigo\"}";
var jsonSobrenome = "{\"sobrenome\": \"Duarte\"}";
var json = "{\"nome\": \"Rodrigo\", \"sobrenome\": \"Duarte\"}";

var serializerSettings = new JsonSerializerSettings()
{
    ContractResolver = new PrivateResolver(),
    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
};

try
{
    var usuarioNome = JsonConvert.DeserializeObject<Usuario>(jsonNome, serializerSettings);
    var usuarioSobrenome = JsonConvert.DeserializeObject<Usuario>(jsonSobrenome, serializerSettings);
    var usuario = JsonConvert.DeserializeObject<Usuario>(json, serializerSettings);

    Console.WriteLine($"nome: {usuarioNome.GetNome}");
    Console.WriteLine($"sobrenome: {usuarioSobrenome.GetNome}");
    Console.WriteLine($"Nome completo: {usuario.Fullname}");
} catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

public class PrivateResolver : DefaultContractResolver
{
    public PrivateResolver()
    {
        NamingStrategy = new CamelCaseNamingStrategy
        {
            OverrideSpecifiedNames = false
        };
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var prop = base.CreateProperty(member, memberSerialization);
        if (!prop.Writable)
        {
            var property = member as PropertyInfo;
            var hasPrivateSetter = property?.GetSetMethod(true) != null;
            prop.Writable = hasPrivateSetter;
        }
        return prop;
    }
}

public class Usuario
{
    public string? Nome { get; private set; }
    public string? Sobrenome { get; private set;  }

    public string? GetNome => Nome ?? Sobrenome;
    public string? Fullname => $"{Nome} {Sobrenome}";
}