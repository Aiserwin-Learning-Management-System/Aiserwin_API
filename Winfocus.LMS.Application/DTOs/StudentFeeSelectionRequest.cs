using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    public sealed record StudentFeeSelectionRequest(
      Guid studentId, Guid courseId, Guid feePlanId, decimal scholarshipPercent, bool isScholarshipActive, decimal seasonalPercent,
      bool isSeasonalActive, decimal manualDiscountPercent, bool isManualDiscountActive, decimal baseFee, decimal finalAmount, string paymentMode, Guid userId);
}
