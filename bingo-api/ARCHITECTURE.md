# Bingo API - Arquitetura do Projeto

Este documento descreve a arquitetura do projeto Bingo API, uma aplicação ASP.NET Core organizada seguindo princípios de arquitetura limpa (Clean Architecture) e separação de responsabilidades.

## Visão Geral

A aplicação é estruturada em camadas distintas, cada uma com responsabilidades bem definidas, facilitando manutenção, testabilidade e escalabilidade.

## Estrutura de Diretórios

```
src/
├── Adapter/              # Adaptadores para integração com serviços externos
├── Application/          # Casos de uso e lógica de aplicação
├── Attribute/            # Atributos customizados
├── Configurations/       # Configurações de serviços e middlewares
├── Constants/            # Constantes utilizadas no sistema
├── Context/              # Contextos do Entity Framework (DataAccess)
├── Controllers/          # Controladores da API (endpoints)
├── DTOs/                 # Objetos de Transferência de Dados
├── Domain/               # Entidades de domínio e regras de negócio
├── Entities/             # Mapeamentos ORM (Entity Framework)
├── Enums/                # Enumerações
├── Extensions/           # Métodos de extensão e configurações de serviços
├── Factory/              # Padrões Factory para criação de objetos
├── Infrastructure/       # Implementações de infraestrutura (repositórios, serviços externos)
├── Interceptors/         # Interceptadores (ex: Auditoria, Logging)
├── Interfaces/           # Contratos (interfaces) que definem dependências
├── IoC/                  # Inversão de Controle (registro de dependências)
├── Jobs/                 # Trabalhos em background (Hangfire)
├── Mappings/             # Mapeamentos entre camadas (ex: AutoMapper)
├── Middleware/           # Middlewares customizados
├── Policys/              # Políticas de autorização e validação
├── Providers/            # Provedores de serviços (ex: autenticação)
├── Repositories/         # Implementações de repositórios
├── Services/             # Serviços de aplicação
├── Structs/              # Structs utilitários
└── SwaggerFilter/        # Filtros customizados para o Swagger
```

## Descrição das Camadas

### 1. **Domain**
Contém as entidades de domínio puro e as regras de negócio que são independentes de qualquer tecnologia externa. É o núcleo da aplicação.

### 2. **Application**
Implementa os casos de uso (use cases) da aplicação. Orquestra o fluxo de dados entre o domain e os repositórios, aplicando as regras de negócio específicas de cada operação.

### 3. **Infrastructure**
Fornece implementações técnicas para interfaces definidas nas camadas internas (Domain e Application). Inclui:
- Repositórios que implementam acesso a dados (Entity Framework, Dapper, etc.)
- Serviços de integração com sistemas externos (APIs, message queues, etc.)
- Implementações de provedores (autenticação, logging, etc.)

### 4. **Entities**
Contém as classes mapeadas pelo Entity Framework que representam as tabelas do banco de dados. Geralmente espelham as entidades de domínio, mas podem incluir anotações ORM específicas.

### 5. **Context**
Define os contextos do Entity Framework (`DbContext`) responsáveis pela sessão com o banco de dados e mapeamento objeto-relacional.

### 6. **Repositories**
Implementações específicas do padrão Repositório, muitas vezes especializando interfaces genéricas do Infrastructure.

### 7. **Services**
Serviços de aplicação que podem conter lógica de negócio que não se encaixa perfeitamente em um caso de uso único, ou que são reutilizados por múltiplos casos de uso.

### 8. **Controllers**
Camada de apresentação que expõe os endpoints da API HTTP. Recebe requisições, valida entradas, chama os casos de uso (Application) e retorna respostas apropriadas.

### 9. **DTOs**
Objetos utilizados para transferência de dados entre camadas, especialmente entre a API e os clientes externos. Evitam expor entidades de domínio diretamente.

### 10. **Mappings**
Configurações de mapeamento entre diferentes tipos de objetos (ex: Entity ↔ DTO, Domain Entity ↔ DTO), normalmente utilizando bibliotecas como AutoMapper.

### 11. **Extensions**
Métodos de extensão que adicionam funcionalidades a tipos existentes, e configurações de serviços para o container de injeção de dependência (por exemplo, `AuthenticationSetup.cs`, `SwaggerSetup.cs`).

### 12. **Configurations**
Classes que encapsulam configurações de diversos aspectos da aplicação (CORS, rate limiting, versionamento, etc.), geralmente usadas durante a inicialização em `Program.cs`.

### 13. **Middleware**
Componentes que processam requisições HTTP no pipeline do ASP.NET Core, realizando funções como logging, tratamento de exceções, headers de segurança, etc.

### 14. **Jobs**
Trabalhos em background processados por ferramentas como Hangfire (agendamento, processamento assíncrono de tarefas).

### 15. **IoC (Inversion of Control)**
Configura o container de injeção de dependência, registrando todas as interfaces e suas implementações. Geralmente contém métodos de extensão para `IServiceCollection`.

### 16. **Adapters**
Adaptadores que convertem dados ou interfaces de serviços externos para um formato consumível pela aplicação (ex: adaptadores para APIs de pagamento, blockchain, etc.).

### 17. **Utilitários e Outros**
- **Attribute**: Atributos customizados (ex: validação, autorização).
- **Constants**: Valores constantes utilizados em todo o sistema.
- **Enums**: Enumerações que representam conjuntos fixos de valores.
- **Structs**: Structs leve para dados simples.
- **Interceptors**: Interceptadores que podem modificar o comportamento de entidades ou operações (ex: auditoria no Entity Framework).
- **Providers**: Provedores que oferecem funcionalidades específicas (ex: provedor de tokens JWT).
- **Policys**: Políticas de autorização e validação customizadas.
- **Reports**: Módulos relacionados à geração de relatórios.
- **SwaggerFilter**: Filtros customizados para personalizar a documentação Swagger/OpenAPI.

## Dependências e Direção

A dependência flui de camadas externas para internas:
- Controllers dependem de Application e Infrastructure
- Application depende de Domain e Interfaces (do Infrastructure)
- Infrastructure implementa interfaces definidas em Domain ou Application
- Nenhuma camada interna deve depender de camadas externas

Exemplo de fluxo de uma requisição:
1. HTTP Request → Controller
2. Controller chama um caso de uso (Application)
3. Caso de uso aplica regras de negócio (Domain) e chama repositórios (Infrastructure)
4. Repositório acessa o banco de dados via Entity Framework (Context/Entities)
5. Retorno é mapeado para DTO e enviado ao cliente

## Tecnologias Identificadas

- **ASP.NET Core**: Framework web
- **Entity Framework Core**: ORM para acesso a dados
- **Hangfire**: Processamento de jobs em background
- **Swagger/OpenAPI**: Documentação da API
- **AutoMapper** (provavelmente): Mapeamento entre objetos
- **JWT**: Autenticação e autorização
- **Rate Limiting**: Controle de taxa de requisições
- **CORS**: Configuração de compartilhamento de recursos entre origens

## Decisões Arquiteturais Notáveis

- Separação clara entre camadas de domínio, aplicação e infraestrutura
- Uso de padrões como Repository, Factory e Strategy (via injeção de dependência)
- Configurações centralizadas em classes de extensão para manter o `Program.cs` limpo
- Tratamento centralizado de exceções e padronização de respostas de erro (via `ProblemDetailsSetup`)
- Versionamento de API implementado
- Uso de middlewares para preocupações transversais (segurança, logging, etc.)

Este documento fornece uma visão geral da arquitetura do projeto. Para detalhes específicos de cada módulo, consulte o código-fonte e a documentação inline.