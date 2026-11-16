# 🌧️ Alerta Enchentes

**Global Solution 2026.2 — CitizenTech · FIAP · Engenharia de Software (3º ano)**

## Ficha da CitizenTech

| Campo | Conteúdo |
|---|---|
| **Nome da CitizenTech** | Alerta Enchentes |
| **Cidade e bairro** | São Paulo — SP · Brooklin |
| **Cidadão beneficiado** | Moradores, trabalhadores, população em geral e comerciantes do Brooklin afetados por chuvas intensas |
| **Problema, em uma frase** | Alagamentos no Brooklin dificultam o deslocamento e as atividades locais, e faltam informações rápidas e centralizadas sobre onde e quão grave é cada ocorrência. |
| **Como funciona** | O usuário registra/consulta ocorrências no app Windows Forms, que consome uma API REST (SOA) que persiste os dados em banco e gera alertas. Ocorrências de risco **Alto** geram alerta automaticamente. |
| **O que cada parte faz** | **C# Windows Forms:** interface, validação e sugestão de risco. **SOA/API REST:** regras de negócio, persistência (EF Core + migrations) e alertas. |
| **Link do repositório** | https://github.com/xXRICKIXx/AlertaEnchentes |
| **Indicador de impacto** | Nº de ocorrências registradas por mês e tempo entre o registro e a emissão do alerta. |
| **ODS conectados** | **ODS 13** (Ação contra a mudança global do clima) e **ODS 11** (Cidades e comunidades sustentáveis) |

--------------------------------------------------------------------------------------------------
# Integrantes do Grupo

| Nome | RM |
|---|---|
| Ícaro Henrique de Souza Calixto | RM560278 |
| Caio Costa Beraldo | RM560775 |
| Henrique Celso Moura da Paixão Filho | RM559687 |
| Victor Kenzo Mikado | RM560057 |

---------------------------------------------------------------------------------------------------


> **Pessoa real beneficiada:** _(descreva quem foi entrevistado — ex.: comerciante/morador do Brooklin — e o que ela faz hoje diante de um alagamento. Não inclua dados pessoais reais.)_

## ODS 13 — como o projeto contribui
Centraliza informações sobre alagamentos, apoia a comunicação de situações de risco e permite que cidadãos reajam com mais informação a eventos climáticos extremos. A tecnologia não elimina o problema climático, mas amplia a capacidade de adaptação local.

## Arquitetura

```
Usuário → Windows Forms (C#) --HTTP/JSON--> API REST (SOA) → Serviços → Banco de dados (SQLite via EF Core)
```

```
AlertaEnchentes
├── README.md
├── CSharp-WindowsForms
│   ├── Models      (Ocorrencia, Alerta, NivelRisco)
│   ├── Forms       (MainForm + 3 abas)
│   └── Services    (ApiClient, Validador)
└── SOA-API
    ├── Controllers (Ocorrencias, Alertas)
    ├── Models      (entidades, DTOs)
    ├── Services    (regras de negócio)
    ├── Data        (AppDbContext)
    ├── Migrations  (InitialCreate)
    └── Testes      (coleção Insomnia + .http)
```

## Como executar

Pré-requisitos: **.NET 8 SDK** e **Windows** (para o app Windows Forms).

1. **API** (primeiro):
   ```bash
   cd SOA-API
   dotnet run
   ```
   Sobe em `http://localhost:5080` (Swagger em `/swagger`). O banco `alertaenchentes.db` é criado e as migrations são aplicadas automaticamente.
2. **Aplicação desktop** (em outro terminal):
   ```bash
   cd CSharp-WindowsForms
   dotnet run
   ```
   Para outra URL de API: defina a variável de ambiente `ALERTA_ENCHENTES_API_URL` (ex.: `http://localhost:5080/`).

### Migrations (EF Core)
A migration `InitialCreate` já está no repositório. Para recriar/evoluir:
```bash
dotnet tool install --global dotnet-ef
cd SOA-API
dotnet ef migrations add NomeDaMigration
dotnet ef database update
```

## API REST

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/ocorrencias` | Lista todas as ocorrências |
| GET | `/api/ocorrencias/{id}` | Retorna uma ocorrência (404 se não existir) |
| POST | `/api/ocorrencias` | Registra ocorrência (201; 400 se inválida) |
| PATCH | `/api/ocorrencias/{id}/status` | Aberta · Em acompanhamento · Resolvida |
| GET | `/api/alertas` | Lista alertas |
| POST | `/api/alertas` | Cria alerta |

Exemplo de `POST /api/ocorrencias`:
```json
{ "local": "Brooklin", "tipo": "Alagamento", "nivelRisco": "Alto", "descricao": "Acúmulo de água na via" }
```

**Regras:** `nivelRisco` ∈ {Baixo, Médio, Alto}; descrição de 5 a 500 caracteres; risco Alto cria alerta automático.

## Banco de dados

**Ocorrencias:** Id, Local, Tipo, NivelRisco, Descricao, Data, Status  
**Alertas:** Id, Mensagem, NivelRisco, Data, Status

## Testes manuais (Insomnia)
Importe `SOA-API/Testes/Insomnia_AlertaEnchentes.json` (10 requisições, incluindo casos de erro 400/404). criada para facilitar os teste para quem for usar a api.

## Segurança (Cybersecurity)
- Acesso a dados via EF Core (consultas parametrizadas → mitiga SQL Injection).
- Validação de entrada na API (DataAnnotations + normalização) e no cliente.
- Nenhum dado pessoal real: use apenas dados simulados ou públicos.

## Tecnologias
C# · .NET 8 · Windows Forms · POO · ASP.NET Core Web API · HTTP/JSON · Entity Framework Core + Migrations · SQLite · Swagger · Insomnia

## Disciplinas integradas
| Disciplina | Responsabilidade |
|---|---|
| C# Software Development | Aplicação desktop Windows Forms |
| Arquitetura Orientada a Serviços (SOA) e Web Services | Backend, API REST e banco de dados |
