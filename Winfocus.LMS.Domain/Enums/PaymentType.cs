namespace Winfocus.LMS.Domain.Enums
{
    /// <summary>
    /// Defines how the student pays the fee — full or in installments.
    /// </summary>
    public enum PaymentType
    {
        /// <summary>Single full payment.</summary>
        Full = 1,

        /// <summary>Installments every 2 months (6 per year).</summary>
        Bimonthly = 2,

        /// <summary>Installments every 3 months (4 per year).</summary>
        Quarterly = 3,

        /// <summary>Single yearly payment.</summary>
        Yearly = 4,
    }
}
