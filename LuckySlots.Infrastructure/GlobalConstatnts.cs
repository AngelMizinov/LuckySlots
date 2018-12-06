namespace LuckySlots.Infrastructure
{
    public static class GlobalConstatnts
    {
        public const string AdministratorRoleName = "Administrator";
        public const string SupportRoleName = "Support";

        public const string AdministratorEmail = "a@a";
        public const string AdministratorPassword = "Login1!";

        // Game symbols
        public const string Apple = "apple";
        public const string Banana = "banana";
        public const string Pineapple = "pineapple";
        public const string Wildcard = "wildcard";

        // Game coefficients
        public const float AppleCoefficient = 0.4f;
        public const float BananaCoefficient = 0.6f;
        public const float PineappleCoefficient = 0.8f;
        public const float WildcardCoefficient = 0;

        // Game probabilities
        public const float AppleProbability = 0.45f;
        public const float BananaProbability = 0.35f;
        public const float PineappleProbability = 0.15f;
        public const float WildcardProbability = 0.05f;
    }
}
