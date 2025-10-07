using OllamaSharp;
using System.Text;

namespace ConsoleOllamaDemo;

public static class ExemploOllamaSharp
{
    public static async Task Executar()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // Inicializa o cliente Ollama
        var ollama = new OllamaApiClient("http://localhost:11434");
        ollama.SelectedModel = "llama3.1";

        Console.WriteLine("🤖 Chat ConsoleOllama (OllamaSharp + Llama 3.1 + Contexto Dinâmico)");
        Console.WriteLine("Digite 'sair' para encerrar.\n");

        // Histórico da conversa
        var chatHistory = new List<(string Role, string Message)>();

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("🧑‍💻 Você: ");
            Console.ResetColor();

            var pergunta = Console.ReadLine();
            if (string.Equals(pergunta, "sair", StringComparison.OrdinalIgnoreCase))
                break;

            if (string.IsNullOrWhiteSpace(pergunta))
                continue;

            // Verifica se é uma função local (chamada direta)
            var respostaLocal = await ExecutarFuncoesLocais(pergunta);
            if (respostaLocal is not null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"🤖 IA (função local): {respostaLocal}\n");
                Console.ResetColor();
                continue;
            }

            // 🔹 Novo passo: obter contexto do sistema
            var contextoSistema = await ObterContextoSistema(pergunta);

            chatHistory.Add((Role: "user", Message: pergunta));

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n💭 Pensando...\n");
            Console.ResetColor();

            // 🔹 Monta o prompt com o contexto + histórico
            var prompt = new StringBuilder();
            prompt.AppendLine("### CONTEXTO DO SISTEMA ###");
            prompt.AppendLine(contextoSistema);
            prompt.AppendLine("\n### HISTÓRICO DE CONVERSA ###");
            foreach (var (Role, Message) in chatHistory)
                prompt.AppendLine($"{Role}: {Message}");
            prompt.AppendLine("\nResponda de forma clara e contextualizada ao sistema descrito acima.");

            // Coleta resposta (stream)
            var respostaBuilder = new StringBuilder();

            await foreach (var stream in ollama.GenerateAsync(prompt.ToString()))
            {
                if (!string.IsNullOrEmpty(stream?.Response))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(stream.Response);
                    respostaBuilder.Append(stream.Response);
                    Console.ResetColor();
                }
            }

            Console.WriteLine("\n");

            var respostaFinal = respostaBuilder.ToString().Trim();
            chatHistory.Add((Role: "assistant", Message: respostaFinal));
        }

        Console.WriteLine("👋 Encerrado com sucesso!");
    }

    // ===================================================
    // Simulação de funções locais (C#)
    // ===================================================
    private static async Task<string?> ExecutarFuncoesLocais(string pergunta)
    {
        pergunta = pergunta.ToLowerInvariant();

        if (pergunta.Contains("contrato"))
            return await new ContratosFunctions().ConsultarContrato("445");

        if (pergunta.Contains("saldo") && pergunta.Contains("cliente"))
            return await new ClientesFunctions().ConsultarSaldo("123");

        if (pergunta.Contains("nota") && (pergunta.Contains("emitir") || pergunta.Contains("gerar")))
            return await new NotasFiscaisFunctions().EmitirNotaFiscal("200");

        return null;
    }

    // ===================================================
    // 🔹 Simulação de "API" de contexto dinâmico
    // ===================================================
    private static async Task<string> ObterContextoSistema(string pergunta)
    {
        await Task.Delay(100); // simula latência de rede

        if (pergunta.Contains("nota"))
        {
            return """
            No sistema SICON, o módulo de Notas Fiscais é responsável por registrar a prestação de serviços
            vinculada a contratos ativos. Cada nota é emitida após a autorização de desconto e enviada ao órgão contratante.
            """;
        }
        else if (pergunta.Contains("contrato"))
        {
            return """
            No sistema SICON, o módulo de Contratos gerencia os vínculos entre a empresa e os órgãos públicos.
            Cada contrato contém dados do órgão, prazo, valores e status de vigência.
            """;
        }
        else if (pergunta.Contains("saldo"))
        {
            return """
            No sistema SICON, o saldo representa o valor disponível para novas consignações em folha,
            calculado com base nos descontos e limites autorizados.
            """;
        }

        // fallback genérico
        return """
        Você é um assistente do sistema SICON, especializado em auxiliar usuários
        sobre contratos, clientes, notas fiscais, relatórios e integrações com órgãos públicos.
        """;
    }
}

// ===================================================
// Classes simulando lógica de negócio
// ===================================================
public class ContratosFunctions
{
    public async Task<string> ConsultarContrato(string numeroContrato)
    {
        await Task.Delay(200);
        return $"Contrato nº {numeroContrato} está ativo e vinculado ao cliente João da Silva desde 2023.";
    }
}

public class ClientesFunctions
{
    public async Task<string> ConsultarSaldo(string idCliente)
    {
        await Task.Delay(200);
        return $"O saldo disponível do cliente {idCliente} é de R$ 732,50.";
    }
}

public class NotasFiscaisFunctions
{
    public async Task<string> EmitirNotaFiscal(string idCliente)
    {
        await Task.Delay(200);
        return $"Nota fiscal emitida com sucesso para o cliente {idCliente}. Número: NF-2025-00123.";
    }
}
