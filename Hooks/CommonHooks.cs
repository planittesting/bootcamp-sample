using Bootcamp.Config;

namespace Bootcamp.BDD.Hooks
{
    [Binding]
    public class CommonHooks
    {
        public CommonHooks()
        {
        }

        [BeforeTestRun]
        public static void Init()
        {
            CleanTestResultsDirectory();
        }

        private static void CleanTestResultsDirectory()
        {
            var testResultsDirectory = TestConfiguration.GetConfig().GetSection("Playwright:TestResultsDirectory").Value;

            if (Directory.Exists(testResultsDirectory))
                Directory.Delete(testResultsDirectory, true);

            Directory.CreateDirectory(testResultsDirectory!);
            Directory.CreateDirectory(Path.Combine(testResultsDirectory!, "traces"));
            Directory.CreateDirectory(Path.Combine(testResultsDirectory!, "videos"));
            Directory.CreateDirectory(Path.Combine(testResultsDirectory!, "screenshots"));
            Directory.CreateDirectory(Path.Combine(testResultsDirectory!, "playwright-report"));
            Directory.CreateDirectory(Path.Combine(testResultsDirectory!, "cucumber"));
            Directory.CreateDirectory(Path.Combine(testResultsDirectory!, "cucumber-report"));
        }
    }
}

