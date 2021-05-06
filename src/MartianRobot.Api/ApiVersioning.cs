namespace MartianRobot.Api
{
    public sealed class ApiVersioning
    {
        private ApiVersioning() { }

        public const string _1_0 = "1.0";
        public const string _2_0 = "2.0";
        public static readonly string[] Versions = new[] { "1.0", "2.0" };
    }
}
