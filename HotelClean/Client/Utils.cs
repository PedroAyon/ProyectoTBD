namespace Client
{
    public class Utils
    {
        public static string? dateFormatted(DateOnly? dateOnly)
        {
            if (dateOnly == null) return null;
            DateOnly date = (DateOnly)dateOnly;
            return date.ToString("yyyy-MM-dd");
        }
    }
}
