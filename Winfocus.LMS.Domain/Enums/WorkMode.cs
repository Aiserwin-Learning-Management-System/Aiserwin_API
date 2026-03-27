namespace Winfocus.LMS.Domain.Enums
{
    /// <summary>
    /// Represents the work mode for a teacher.
    /// </summary>
    public enum WorkMode
    {
        /// <summary>Work from office or center.</summary>
        Offline = 1,

        /// <summary>Work remotely online.</summary>
        Online = 2,

        /// <summary>Combination of online and offline work.</summary>
        Hybrid = 3,
    }
}