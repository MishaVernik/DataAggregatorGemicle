namespace DataAggregatorGemicle.Utils
{
    public static class ClientCodeGenerator
    {
        public static string Generate(string firstName, string lastName, string organizationName)
        {
            string part1 = !string.IsNullOrEmpty(firstName)
                ? new string(firstName.Skip(1).Take(3).Reverse().ToArray()).ToUpper()
                : String.Empty;

            string part2 = !string.IsNullOrEmpty(lastName)
                ? new string(lastName.Skip(1).Take(3).Reverse().ToArray()).ToUpper()
                : String.Empty;

            string part3 = organizationName.Length > 0
                ? string.Concat(organizationName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(word => word[0])).ToUpper()
                : String.Empty;

            return $"{part1}-{part2}-{part3}";
        }
    }
}
