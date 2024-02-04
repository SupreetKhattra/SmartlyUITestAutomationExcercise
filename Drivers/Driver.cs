using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using System.Drawing;


namespace specflowTesting1.Drivers
{
    public class Driver
    {

        private static IWebDriver _webDriver;
        //Get environment variable fom the YML file. Since tests run in parallel on different browsers
        private static string browserType = Environment.GetEnvironmentVariable("BROWSER") ?? "chrome";

        public static IWebDriver WebDriver
        {
            get
            {
                if (_webDriver == null)
                {
                    InitializeWebDriver();
                    // Additional configuration if needed
                    _webDriver.Manage().Window.Maximize();
                    //Increase the browser window size to ensure all elements visible
                    _webDriver.Manage().Window.Size = new Size(1920, 1080);

                }
                return _webDriver;
            }
        }
        

        private static void InitializeWebDriver()
        {
            //Get headless options variable from the environment (this is set in the dotnet.yml file)
            //If the variable doesn't return anything, use nothing
            var headlessOption = Environment.GetEnvironmentVariable("browserOptions") ?? "";

            Console.WriteLine("Browser type is: "); 
            Console.WriteLine(browserType);

            // If browser type is chrome, initialize chrome webdriver
            if (browserType.Equals("chrome", StringComparison.OrdinalIgnoreCase))
            {
                //setup chrome options
                var options = new ChromeOptions();
                //Start maximized
                options.AddArgument("start-maximized");

                //if the headless option is set in the dotnet.yml file, add it to the options
                if (headlessOption != null && headlessOption == "headless")
                {
                    Console.WriteLine("Browser is: headless"); 
                    options.AddArgument("headless");
                }

                // Initialize ChromeDriver with the options
                Console.WriteLine("Setting up chrome webdriver..."); 
                _webDriver = new ChromeDriver(options);

            }
            //Else if it's edge, initialize edge webdriver
            else if (browserType.Equals("edge", StringComparison.OrdinalIgnoreCase))
            {
                //setup edge options
                var options = new EdgeOptions();
                options.AddArgument("start-maximized");

                //if the headless option is set in the dotnet.yml file, add it to the options
                if (headlessOption != null && headlessOption == "headless")
                {
                    Console.WriteLine("Browser is: headless"); 
                    options.AddArgument("headless");
                }

                // Initialize EdgeDriver with the options
                Console.WriteLine("Setting up edge webdriver..."); 
                _webDriver = new EdgeDriver(options);

            }
            // If no browser options found, throw an error
            else
            {
                
                throw new NotSupportedException($"Browser type '{browserType}' is not supported.");
            }

        }

        public static void QuitWebDriver()
        {
            if (_webDriver != null)
            {
                _webDriver.Quit();
                _webDriver = null;
                Console.WriteLine("driver closed");
            }
        }
    }
}

