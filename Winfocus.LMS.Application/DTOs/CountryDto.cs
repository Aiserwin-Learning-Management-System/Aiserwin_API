namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Country response DTO.
    /// </summary>
    public sealed record CountryDto(
        Guid id,
        string name,
        IReadOnlyList<CentreDto> centres
    );
}
