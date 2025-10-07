using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;

namespace ConsoleOllamaDemo;

public static class ExemploSemanticKernel
{
    public static async Task Executar()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // ======================================================
        // 🧠 Configura Kernel + Ollama (local em localhost:11434)
        // ======================================================
        var builder = Kernel.CreateBuilder();
        builder.Services.AddOllamaChatCompletion("llama3.1", new Uri("http://localhost:11434"));
        var kernel = builder.Build();

        // Obtém o serviço de chat
        var chat = kernel.GetRequiredService<IChatCompletionService>();

        Console.WriteLine("🤖 Chat ConsoleKernel (Semantic Kernel + Ollama + Contexto Dinâmico)");
        Console.WriteLine("Digite 'sair' para encerrar.\n");

        // Histórico da conversa
        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("""
        Você é um assistente do sistema SICON, especializado em auxiliar usuários
        sobre contratos, clientes, notas fiscais, relatórios e integrações com órgãos públicos.
        Responda sempre de forma clara e contextualizada ao sistema descrito.
        """);

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

            // ==================================================
            // 🧩 Verifica se é uma função local (C#)
            // ==================================================
            var respostaLocal = await ExecutarFuncoesLocais(pergunta);
            if (respostaLocal is not null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"🤖 IA (função local): {respostaLocal}\n");
                Console.ResetColor();
                continue;
            }

            // ==================================================
            // 🔹 Obter contexto dinâmico (simulação de API)
            // ==================================================
            var contextoSistema = await ObterContextoSistema(pergunta);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n💭 Pensando...\n");
            Console.ResetColor();

            // ==================================================
            // 🔹 Montar prompt completo
            // ==================================================
            var contextoPrompt = new StringBuilder();
            contextoPrompt.AppendLine("### CONTEXTO DO SISTEMA ###");
            contextoPrompt.AppendLine(contextoSistema);
            contextoPrompt.AppendLine("\n### HISTÓRICO DE CONVERSA ###");

            foreach (var msg in chatHistory)
                contextoPrompt.AppendLine($"{msg.Role}: {msg.Content}");

            contextoPrompt.AppendLine($"\nUsuário: {pergunta}");

            chatHistory.AddUserMessage(pergunta);

            // ==================================================
            // 🔹 Gera resposta usando Semantic Kernel
            // ==================================================
            var responseBuilder = new StringBuilder();

            await foreach (var chunk in chat.GetStreamingChatMessageContentsAsync(chatHistory, kernel: kernel))
            {
                if (!string.IsNullOrEmpty(chunk.Content))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(chunk.Content);
                    responseBuilder.Append(chunk.Content);
                    Console.ResetColor();
                }
            }

            Console.WriteLine("\n");

            var respostaFinal = responseBuilder.ToString().Trim();
            chatHistory.AddAssistantMessage(respostaFinal);
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

        return """
        Você é um assistente do sistema SICON, especializado em auxiliar usuários
        sobre contratos, clientes, notas fiscais, relatórios e integrações com órgãos públicos.
        """;
    }
}
