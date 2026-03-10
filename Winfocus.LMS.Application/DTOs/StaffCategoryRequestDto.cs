namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a staff_category.
    /// </summary>
    public sealed record StaffCategoryRequestDto(string name, string description, Guid userId);
}
