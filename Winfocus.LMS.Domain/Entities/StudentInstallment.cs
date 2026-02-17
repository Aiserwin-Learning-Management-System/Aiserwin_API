using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public sealed class StudentInstallment : BaseEntity
    {
        public Guid StudentFeeSelectionId { get; private set; }

        public int InstallmentNo { get; private set; }
        public decimal Amount { get; private set; }

        public DateTime DueDate { get; private set; }
        public bool IsPaid { get; private set; }
    }
}
