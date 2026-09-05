namespace HallRentingService.WebAPI.Features.Booking.Services;

public sealed class BookingPriceModifier : IBookingPriceModifier
{
    #region Static
    private static decimal GetPriceModifier(TimeSpan timeOfDay)
    {
        if(timeOfDay >= TimeSpan.FromHours(6) && timeOfDay < TimeSpan.FromHours(9))
        {
            return 0.90M;
        }
        if(timeOfDay >= TimeSpan.FromHours(12) && timeOfDay < TimeSpan.FromHours(14))
        {
            return 1.15M;
        }
        if(timeOfDay >= TimeSpan.FromHours(18) && timeOfDay < TimeSpan.FromHours(23))
        {
            return 0.80M;
        }
        return 1M;
    }
    #endregion

    #region Interfaces
    public decimal ApplyModifiers(decimal pricePerHour, DateTime bookingStart, TimeSpan bookingDuration)
    {
        if(bookingDuration <= TimeSpan.Zero)
        {
            return 0;
        }
        DateTime bookingEnd = bookingStart.Add(bookingDuration);
        DateTime currentPeriodStart = bookingStart;
        decimal finalPrice = 0;
        while(currentPeriodStart < bookingEnd)
        {
            DateTime nextPeriodStart = currentPeriodStart.Date.AddDays(1);
            foreach(TimeSpan periodStart in new[]
            {
                TimeSpan.FromHours(6),
                TimeSpan.FromHours(9),
                TimeSpan.FromHours(12),
                TimeSpan.FromHours(14),
                TimeSpan.FromHours(18),
                TimeSpan.FromHours(23)
            })
            {
                DateTime candidatePeriodStart = currentPeriodStart.Date.Add(periodStart);
                if (candidatePeriodStart > currentPeriodStart && candidatePeriodStart < nextPeriodStart)
                {
                    nextPeriodStart = candidatePeriodStart;
                }
            }

            DateTime currentPeriodEnd = nextPeriodStart < bookingEnd ? nextPeriodStart : bookingEnd;
            decimal periodHours = (decimal)(currentPeriodEnd - currentPeriodStart).TotalHours;
            finalPrice += pricePerHour * periodHours * GetPriceModifier(currentPeriodStart.TimeOfDay);
            currentPeriodStart = currentPeriodEnd;
        }
        return finalPrice;
    }
    #endregion
}