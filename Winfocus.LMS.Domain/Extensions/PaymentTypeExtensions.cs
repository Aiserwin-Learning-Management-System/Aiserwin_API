namespace Winfocus.LMS.Domain.Extensions
{
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Provides calculation helpers for <see cref="PaymentType"/>.
    /// </summary>
    public static class PaymentTypeExtensions
    {
        /// <summary>
        /// Gets the number of installments generated per year for this payment type.
        /// </summary>
        /// <param name="paymentType">The payment type.</param>
        /// <returns>Installments per year.</returns>
        public static int GetInstallmentsPerYear(this PaymentType paymentType) =>
            paymentType switch
            {
                PaymentType.Full => 1,
                PaymentType.Yearly => 1,
                PaymentType.Bimonthly => 6,
                PaymentType.Quarterly => 4,
                _ => 1
            };

        /// <summary>
        /// Gets the number of months between consecutive installments.
        /// </summary>
        /// <param name="paymentType">The payment type.</param>
        /// <returns>Months between installments. 0 for Full payment.</returns>
        public static int GetMonthsBetween(this PaymentType paymentType) =>
            paymentType switch
            {
                PaymentType.Full => 0,
                PaymentType.Yearly => 0,
                PaymentType.Bimonthly => 2,
                PaymentType.Quarterly => 3,
                _ => 0
            };

        /// <summary>
        /// Calculates total installments for a given duration.
        /// </summary>
        /// <param name="paymentType">The payment type.</param>
        /// <param name="durationYears">Course duration in years.</param>
        /// <returns>Total number of installments.</returns>
        public static int GetTotalInstallments(this PaymentType paymentType, int durationYears) =>
            paymentType == PaymentType.Full
                ? 1
                : paymentType.GetInstallmentsPerYear() * durationYears;
    }
}
