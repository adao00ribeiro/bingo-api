# ARCHITECTURE.md — Bingo API (DDD)

## Visão Geral

A solução usa **Domain-Driven Design com projetos separados por camada**.
Cada camada é uma assembly `.csproj` independente — o compilador impede dependências erradas.
**Sem MediatR** — orquestração feita por services de aplicação diretos.

---

## Estrutura da Solução

```
bingo-api.sln
├── Bingo.Api/                  ← Presentation: controllers, middlewares, program.cs
├── Bingo.Application/          ← Casos de uso, DTOs, interfaces de serviço
├── Bingo.Domain/               ← Agregados, entidades, value objects, eventos, interfaces de repositório
├── Bingo.Infrastructure/       ← EF Core, repositórios, gateways externos, jobs
├── Bingo.Identity/             ← ASP.NET Identity, JWT, contexto de identidade
└── bingo-tests/                ← Testes unitários e de integração
```

---

## Dependências entre Projetos

```
Bingo.Api
    └──▶ Bingo.Application
              └──▶ Bingo.Domain
                        (zero dependências externas)

Bingo.Infrastructure
    └──▶ Bingo.Domain
    └──▶ Bingo.Application    (implementa interfaces)

Bingo.Identity
    └──▶ Bingo.Domain         (opcional, apenas User)

Bingo.Api
    └──▶ Bingo.Infrastructure  (apenas para registro no IoC)
    └──▶ Bingo.Identity
```

> **Regra de ouro:** `Bingo.Domain` não referencia NENHUM outro projeto.
> Qualquer violação disso quebra o compilador — é proposital.

---

## Projeto: `Bingo.Domain`

Núcleo da aplicação. Zero dependências de frameworks externos.

```
Bingo.Domain/
├── Payment/
│   ├── Payment.cs                    ← agregado raiz
│   ├── PaymentStatus.cs              ← enum de estados
│   ├── IPaymentRepository.cs         ← interface do repositório
│   ├── IPaymentGateway.cs            ← interface do gateway externo
│   └── Events/
│       ├── PaymentConfirmedEvent.cs
│       └── PaymentFailedEvent.cs
│
├── Room/
│   ├── Room.cs                       ← agregado raiz
│   ├── RoomSeller.cs
│   ├── Accumulated.cs
│   ├── IRoomRepository.cs
│   └── Events/
│       └── RoomCreatedEvent.cs
│
├── Round/
│   ├── Round.cs                      ← agregado raiz
│   ├── Card.cs
│   ├── CardBuy.cs
│   ├── CardWinner.cs
│   ├── Prize.cs
│   ├── IRoundRepository.cs
│   └── Events/
│       └── RoundFinishedEvent.cs
│
├── Scratch/
│   ├── ScratchGame.cs                ← agregado raiz
│   ├── ScratchGameOverride.cs
│   ├── ScratchBuy.cs
│   ├── ScratchTicket.cs
│   ├── ScratchPrize.cs
│   ├── IScratchRepository.cs
│   └── Events/
│       └── ScratchPrizeCreatedEvent.cs
│
├── Punter/
│   ├── Punter.cs                     ← agregado raiz
│   ├── Recharge.cs
│   ├── IPunterRepository.cs
│   └── Events/
│
├── Seller/
│   ├── Seller.cs                     ← agregado raiz
│   ├── ISellerRepository.cs
│   └── Events/
│
├── Blockchain/
│   ├── Network.cs                    ← agregado raiz
│   ├── Token.cs
│   ├── TokenAddress.cs
│   ├── IBlockchainRepository.cs
│   └── ICryptoGateway.cs             ← interface do gateway cripto
│
└── Shared/
    ├── Entity.cs                     ← base com Id, CreatedAt, DomainEvents
    ├── IDomainEvent.cs
    ├── Withdrawal.cs                 ← base TPH
    ├── PunterWithdrawal.cs
    ├── SellerWithdrawal.cs
    ├── OnlineHouse.cs
    ├── MediaAttachment.cs
    ├── PaymentMethod.cs
    ├── TransactionHistory.cs
    └── BotConfig.cs
```

---

## Projeto: `Bingo.Application`

Casos de uso. Orquestra o domínio. Sem lógica de negócio.

```
Bingo.Application/
├── Payment/
│   ├── PaymentService.cs
│   ├── IPaymentService.cs
│   └── DTOs/
│       ├── CreatePaymentRequestDto.cs
│       └── PaymentResponseDto.cs
│
├── Room/
│   ├── RoomService.cs
│   ├── IRoomService.cs
│   └── DTOs/
│       ├── RoomRequestDto.cs
│       └── RoomResponseDto.cs
│
├── Round/
│   ├── RoundService.cs
│   ├── IRoundService.cs
│   └── DTOs/
│
├── Scratch/
│   ├── ScratchService.cs
│   ├── IScratchService.cs
│   └── DTOs/
│
├── Punter/
│   ├── PunterService.cs
│   ├── IPunterService.cs
│   └── DTOs/
│
├── Seller/
│   ├── SellerService.cs
│   ├── ISellerService.cs
│   └── DTOs/
│
└── Shared/
    └── DTOs/
        └── PagedResponseDto.cs
```

---

## Projeto: `Bingo.Infrastructure`

Implementações externas. Referencia Domain e Application.

```
Bingo.Infrastructure/
├── Persistence/
│   ├── DataContext.cs
│   ├── Migrations/
│   ├── Mappings/                     ← IEntityTypeConfiguration por agregado
│   │   ├── PaymentMapping.cs
│   │   ├── RoomMapping.cs
│   │   └── ...
│   └── Repositories/
│       ├── PaymentRepository.cs      ← implementa IPaymentRepository
│       ├── RoomRepository.cs
│       ├── RoundRepository.cs
│       ├── ScratchRepository.cs
│       ├── PunterRepository.cs
│       └── SellerRepository.cs
│
├── Gateways/
│   ├── Pix/
│   │   └── PixGateway.cs             ← implementa IPaymentGateway
│   └── Crypto/
│       ├── EthereumGateway.cs        ← implementa ICryptoGateway
│       └── BscGateway.cs
│
├── Jobs/                             ← Hangfire jobs
│   └── PaymentReconciliationJob.cs
│
├── EventDispatcher.cs
└── IoC/
    └── InfrastructureExtensions.cs   ← registra tudo no DI
```

---

## Projeto: `Bingo.Api`

Apenas apresentação. Controllers traduzem HTTP → Application → HTTP.

```
Bingo.Api/
├── Controllers/
│   ├── Bingo/
│   ├── Scratch/
│   ├── Blockchain/
│   └── Shared/
├── Middleware/
├── Configurations/
│   ├── SwaggerConfiguration.cs
│   ├── AuthConfiguration.cs
│   └── VersioningConfiguration.cs
├── Filters/
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```

---

## Projeto: `Bingo.Identity`

Isolado para não misturar Identity com o domínio principal.

```
Bingo.Identity/
├── IdentityDataContext.cs
├── User.cs
├── IdentityService.cs
├── IIdentityService.cs
├── JwtProvider.cs
└── Policies/
    └── AuthorizationPolicies.cs
```

---

## Fluxo de uma Requisição

```
HTTP Request
    │
    ▼
Bingo.Api / Controller
    │  valida DTO de entrada
    ▼
Bingo.Application / [X]Service
    │  orquestra, sem lógica de negócio
    ├──▶ Bingo.Domain / Agregado      ← lógica de negócio aqui
    ├──▶ IRepository (interface)      ← implementado na Infrastructure
    ├──▶ IPaymentGateway (interface)  ← implementado na Infrastructure
    └──▶ Domain Events                ← disparados pelo agregado, processados após SaveChanges
    │
    ▼
DTO de resposta (Application/DTOs)
    │
    ▼
HTTP Response
```

---

## Agregados e Status de Migração

| Agregado | Entidades | Projeto | Status |
|---|---|---|---|
| `Payment` | Recharge, Withdrawal, TransactionHistory | Bingo.Domain/Payment | 🔲 Pendente |
| `Scratch` | ScratchGame, ScratchGameOverride, ScratchBuy, ScratchTicket, ScratchPrize | Bingo.Domain/Scratch | 🔲 Pendente |
| `Round` | Round, Card, CardBuy, CardWinner, Prize | Bingo.Domain/Round | 🔲 Pendente |
| `Room` | Room, RoomSeller, Accumulated | Bingo.Domain/Room | 🔲 Pendente |
| `Punter` | Punter | Bingo.Domain/Punter | 🔲 Pendente |
| `Seller` | Seller | Bingo.Domain/Seller | 🔲 Pendente |
| `Blockchain` | Network, Token, TokenAddress | Bingo.Domain/Blockchain | 🔲 Pendente |

---

## Regras de Arquitetura

- `Bingo.Domain` → **zero referências** a outros projetos da solução
- `Bingo.Application` → referencia apenas `Bingo.Domain`
- `Bingo.Infrastructure` → referencia `Bingo.Domain` + `Bingo.Application`
- `Bingo.Api` → referencia `Bingo.Application` + `Bingo.Infrastructure` + `Bingo.Identity`
- Controllers **nunca** acessam repositórios — sempre services de Application
- Lógica de negócio **nunca** em Application — sempre no agregado em Domain
- DTOs ficam em `Bingo.Application/[Agregado]/DTOs/` — nunca em Domain
- Interfaces de repositório ficam em `Bingo.Domain/[Agregado]/` — nunca em Application
- Todo novo gateway externo implementa uma interface definida em `Bingo.Domain`
