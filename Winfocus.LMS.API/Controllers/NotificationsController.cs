namespace Winfocus.LMS.API.Controllers;

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Notifications;
using Winfocus.LMS.Application.Interfaces;

/// <summary>
/// API controller for managing user notifications.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public sealed class NotificationsController : BaseController
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// Gets paginated notifications for the authenticated user (newest first).
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<CommonResponse<PagedResult<NotificationDto>>>> GetAll(
        [FromQuery] PagedRequest request)
    {
        var result = await _notificationService.GetAllAsync(UserId, request);
        return Ok(result);
    }

    /// <summary>
    /// Gets unread notification count and the list of unread notifications.
    /// </summary>
    [HttpGet("unread")]
    public async Task<ActionResult<NotificationUnreadDto>> GetUnread()
    {
        var result = await _notificationService.GetUnreadAsync(UserId);
        return Ok(result);
    }

    /// <summary>
    /// Marks a single notification as read.
    /// </summary>
    [HttpPut("{id:guid}/read")]
    public async Task<ActionResult<CommonResponse<bool>>> MarkAsRead(Guid id)
    {
        var result = await _notificationService.MarkAsReadAsync(id, UserId);
        return Ok(result);
    }

    /// <summary>
    /// Marks all unread notifications as read for the authenticated user.
    /// </summary>
    [HttpPut("read-all")]
    public async Task<ActionResult<CommonResponse<bool>>> MarkAllRead()
    {
        var result = await _notificationService.MarkAllReadAsync(UserId);
        return Ok(result);
    }
}
