using System;
using TechTalk.SpecFlow;

namespace specflowTesting1.Hooks
{
    [Binding]
    public class Hooks
    {
        private static bool isInitialized;

            [BeforeTestRun]
            public static void BeforeTestRun()
            {
                if (!isInitialized)
                {
                    //Initialize();
                    isInitialized = true;
                }
            }

            private static void Initialize()
            {
                // var revisionInfo = BrowserFetcher.DefaultRevision;
                // var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions());
                // var browserPath = browserFetcher.GetExecutablePath(revisionInfo);

                // Console.WriteLine($"Chrome version: {revisionInfo}");
                // Console.WriteLine($"Chrome path: {browserPath}");

                // // Use browserPath in your Selenium setup if needed
                // // e.g., set the ChromeDriver executable path
                // Environment.SetEnvironmentVariable("webdriver.chrome.driver", browserPath);

                // // Download the browser executable if not already downloaded
                // browserFetcher.DownloadAsync(revisionInfo).Wait();
            }
    }
}
