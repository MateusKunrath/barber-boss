using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Reports;

namespace BarberBoss.Domain.Extensions;

public static class PaymentMethodExtensions
{
    public static string PaymentMethodToString(this PaymentMethod paymentMethod)
    {
        return paymentMethod switch
        {
            PaymentMethod.Cash => ResourceReportGenerationMessages.PAYMENT_METHOD_CASH,
            PaymentMethod.CreditCard => ResourceReportGenerationMessages.PAYMENT_METHOD_CREDIT_CARD,
            PaymentMethod.DebitCard => ResourceReportGenerationMessages.PAYMENT_METHOD_DEBIT_CARD,
            PaymentMethod.TransferElectronic => ResourceReportGenerationMessages.PAYMENT_METHOD_ELECTRONIC_TRANSFER,
            _ => ResourceReportGenerationMessages.PAYMENT_METHOD_OTHER
        };
    }
}