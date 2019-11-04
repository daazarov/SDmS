namespace SDmS.Resource.Api.Extensions
{
    public static class HelpExtensions
    {
        public static bool Between(this int current, int from, int to)
        {
            return (current >= from && current <= to);
        }
    }
}
