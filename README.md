# DHouse.Core

# DHouse

DHouse é uma plataforma digital para corretagem imobiliária de alto padrão em Goiânia, focada em gerenciar imóveis e serviços adicionais (avaliação e vistoria). Este repositório contém o backend do MVP, desenvolvido em C# com ASP.NET Core e RavenDB.

## Estrutura do Projeto
- `src/DHouse.Core`: Entidades e interfaces do domínio.
- `src/DHouse.Application`: Lógica de negócio e serviços.
- `src/DHouse.Infrastructure`: Integração com RavenDB e outras dependências.
- `src/DHouse.API`: Endpoints da API REST.
- `src/DHouse.Tests`: Testes unitários e de integração.

## Como Contribuir
1. Clone o repositório: `git clone https://github.com/plajiw/DHouse.git`
2. Mude para a branch de desenvolvimento: `git checkout develop`
3. Instale as dependências: `dotnet restore`
4. Execute o projeto: `dotnet run --project src/DHouse.API`

## Tecnologias
- C# / ASP.NET Core
- RavenDB
