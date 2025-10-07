# 🧠 ConsoleOllamaDemo

Este projeto demonstra duas formas práticas de integrar **Modelos de Linguagem (LLMs)** executados localmente com o **[Ollama](https://ollama.ai)** em uma aplicação **C# Console**:

1. **OllamaSharp** — integração direta e simples com streaming de resposta.  
2. **Semantic Kernel + Ollama** — integração mais avançada, com histórico de conversa e contexto dinâmico.

---

## 🚀 Estrutura do Projeto

```
ConsoleOllamaDemo/
├── ExemploOllamaSharp.cs        # Exemplo 1: uso direto com OllamaSharp
├── ExemploSemanticKernel.cs     # Exemplo 2: uso com Microsoft Semantic Kernel
├── Program.cs                   # Ponto de entrada para executar os exemplos
└── .gitignore                   # Configuração para ignorar arquivos desnecessários
```

---

## 🧩 Tecnologias Utilizadas

| Biblioteca | Descrição |
|-------------|------------|
| **OllamaSharp** | Cliente leve para interagir diretamente com a API local do Ollama. |
| **Microsoft.SemanticKernel** | Framework da Microsoft para orquestrar LLMs e funções de IA. |
| **.NET 8 / 9** | Plataforma base para execução do projeto. |
| **Llama 3.1 (via Ollama)** | Modelo de linguagem usado localmente para gerar respostas. |

---

## ⚙️ Pré-requisitos

Antes de rodar o projeto, garanta que você possui:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Ollama instalado](https://ollama.ai/download)
- Modelo **Llama 3.1** baixado localmente:

```
ollama pull llama3.1
```

---

## ▶️ Como Executar

### 1️⃣ Clone o repositório

```
git clone https://github.com/seuusuario/ConsoleOllamaDemo.git
cd ConsoleOllamaDemo
```

### 2️⃣ Restaure os pacotes NuGet

```
dotnet restore
```

### 3️⃣ Escolha o exemplo a executar

No arquivo `Program.cs`, defina qual exemplo deseja rodar:

```csharp
await ExemploOllamaSharp.Executar();
// ou
await ExemploSemanticKernel.Executar();
```

### 4️⃣ Execute o projeto

```
dotnet run
```

---

## 💬 Funcionamento dos Exemplos

### 🧱 Exemplo 1 — OllamaSharp
- Comunicação direta com o modelo local.
- Exibe as respostas da IA em **streaming** no console.
- Suporte a funções C# locais como `ConsultarContrato()` e `EmitirNotaFiscal()`.

### ⚙️ Exemplo 2 — Semantic Kernel + Ollama
- Integra o Ollama com o **Microsoft Semantic Kernel**.
- Permite manter **histórico de conversas**, **contexto dinâmico** e **integração com APIs**.
- Ideal para evoluir o projeto em direção a um **chat inteligente corporativo**.

---

## 🧠 Próximos Passos

- [ ] Adicionar integração real com API da sua empresa.  
- [ ] Implementar armazenamento de contexto em cache (Redis, SQLite).  
- [ ] Criar uma interface Web (Blazor, MudBlazor ou MAUI).  
- [ ] Deploy em contêiner Docker para ambiente de produção.

---

## 👨‍💻 Autor

**Reinan Neto**  
Desenvolvedor .NET | IA | Blazor | Azure | DevOps  
🔗 [LinkedIn](https://www.linkedin.com/in/reinan-guilherme) • [GitHub](https://github.com/ReinanGuilherme3)