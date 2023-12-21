namespace RedisLua.Infrastructure.Extensions
{
    public static class StringExtension
    {
        public static string GetValidateJsonResult(this string input)
        {
            return input
                .Replace("\"{", "{")
                .Replace("}\"", "}")
                .Replace("\\", string.Empty)
                .Replace("{}", "[]");
        }
    }
}
