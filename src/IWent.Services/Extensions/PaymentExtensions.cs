using IWent.Services.DTO.Payments;

namespace IWent.Services.Extensions;

public static class PaymentExtensions
{
    public static PaymentStatus ToDTO(this Persistence.Entities.PaymentStatus paymentStatus)
        => paymentStatus switch
        {
            Persistence.Entities.PaymentStatus.Pending => PaymentStatus.Pending,
            Persistence.Entities.PaymentStatus.Completed => PaymentStatus.Completed,
            _ => PaymentStatus.Failed,
        };
}
