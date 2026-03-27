namespace Winfocus.LMS.Domain.Enums
{
    /// <summary>
    /// Represents the type of ID proof for a teacher.
    /// </summary>
    public enum IdProofType
    {
        /// <summary>Aadhaar card.</summary>
        Aadhaar = 1,

        /// <summary>Passport.</summary>
        Passport = 2,

        /// <summary>Driver's license.</summary>
        DriversLicense = 3,

        /// <summary>Voter ID card.</summary>
        VoterID = 4,
    }
}