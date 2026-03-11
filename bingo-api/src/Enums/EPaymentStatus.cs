namespace bingo_api.src.Enums;

public enum EPaymentStatus
{
    CREATED = 0,            // Recharge criada, gateway ainda não chamado

    WAITING_PAYMENT = 1,    // QR gerado / aguardando pagamento

    WAITING_CONFIRMATION = 2, // Crypto enviada mas aguardando confirmações

    CONFIRMED = 3,          // Gateway confirmou pagamento

    CREDITED = 4,           // Saldo do punter já foi atualizado

    EXPIRED = 5,            // QR expirou

    FAILED = 6,             // Erro no processamento

    REJECTED = 7,           // Recusado pelo gateway

    CANCELED = 8            // Cancelado manualmente
}
