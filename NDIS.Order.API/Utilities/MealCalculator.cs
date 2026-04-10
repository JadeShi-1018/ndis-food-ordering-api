namespace NDIS.Order.API.Utilities
{
  public static class MealCalculator
  {
    public static  int CalculateMealCount(DateTime startDate, DateTime endDate, string periodName)
    {
      if (endDate < startDate)
      {
        throw new ArgumentException("EndDate cannot be earlier than StartDate.");
      }

      var totalDays = (endDate.Date - startDate.Date).Days + 1;

      return periodName.ToLower() switch
      {
        "daily" => totalDays,

        // 每7天算1个周期（向上取整）
        "weekly" => (int)Math.Ceiling(totalDays / 7.0),
        _ => throw new ArgumentException("Unsupported period type.")
      };
    }

  }
}
