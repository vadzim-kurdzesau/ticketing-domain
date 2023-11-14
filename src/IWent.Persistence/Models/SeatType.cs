namespace IWent.Persistence.Models;

/// <summary>
/// The type of seats in a section.
/// </summary>
public enum SeatType
{
    /// <summary>
    /// Means each patron has their own designated seat.
    /// </summary>
    Designated,

    /// <summary>
    /// Means patrons can choose to sit anywhere within a particular section.
    /// </summary>
    GeneralAdmission
}
