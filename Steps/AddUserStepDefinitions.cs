using System;
using System.IO;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using NUnit.Framework;
using System.Runtime.CompilerServices;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Interactions;
using System.Runtime.InteropServices;
using OpenQA.Selenium.DevTools.V119.CSS;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Collections.ObjectModel;
using OpenQA.Selenium.DevTools.V119.Network;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.DevTools.V121.FedCm;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System.Threading.Tasks;
using System.Drawing;
using WebDriverManager.Services.Impl;


namespace specflowTesting1.Steps
{
    [Binding]
    public class AddUserStepDefinitions
    {
       
     // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef
     private IWebDriver driver;
     private WebDriverWait wait;
     private readonly ScenarioContext scenarioContext;
     private string loginUsername = "Admin";
     private string loginPassword = "admin123";
     private string default_headless_option = "headless";
     //Get environment variable fom the YML file. Since tests run in parallel on different browsers
     private string firstname = Environment.GetEnvironmentVariable("FIRSTNAME") ?? "Aaa111";
     private string lastname = Environment.GetEnvironmentVariable("LASTNAME") ?? "Aaa111";
     private string username = Environment.GetEnvironmentVariable("USERNAME") ?? "johndoe";
     private string password = Environment.GetEnvironmentVariable("PASSWORD") ?? "password111";
     private string browserType = Environment.GetEnvironmentVariable("BROWSER") ?? "edge";

     //runs before each scenario.
     [BeforeScenario]
     public void Setup()
     {
          // Initialize your WebDriver and WebDriverWait
          driver = InitializeDriver();
          
          //Increase the browser window size to ensure all elements visible
          driver.Manage().Window.Size = new Size(1920, 1080);

          //Add a 20 second wait for finding driver elements in case the browser is running slowly
          wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

          //Show the variables we are testing with
          Console.WriteLine($"Variables used: {firstname}, {lastname}, {username}, {password}");

     }

     //Setup a scenario context in case we need to use it.
       public AddUserStepDefinitions(ScenarioContext scenarioContext)
       {
          this.scenarioContext = scenarioContext;
       }

     //Initialize the browser driver based on whichever browser we are using
     private IWebDriver InitializeDriver()
     {
          //Get headless options variable from the environment (this is set in the dotnet.yml file)
          //If the variable doesn't return anything, use nothing
          var headlessOption = Environment.GetEnvironmentVariable("browserOptions") ?? default_headless_option;

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
               return new ChromeDriver(options);

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
               return new EdgeDriver(options);

          }
          // If no browser options found, throw an error
          else
          {
               
               throw new NotSupportedException($"Browser type '{browserType}' is not supported.");
          }
     }

     [Given(@"the user is on the login page '(.*)'")]
       public void UserIsOnTheLoginPage(string webUrl)
       {
            // Open the URL
            driver.Navigate().GoToUrl(webUrl); 
       }

     [Given(@"the user enters the username '(.*)' and password '(.*)'")]
       public void UserEntersTheUsernameAndPassword(string username, string password)
       {

          // Find and fill in the username and password input fields and press Enter
          IWebElement usernameInput = wait.Until(ExpectedConditions.ElementExists(By.Name("username"))); 
          IWebElement passwordInput = wait.Until(ExpectedConditions.ElementExists(By.Name("password"))); 

          usernameInput.SendKeys(username); 
          passwordInput.SendKeys(password + Keys.Enter); 

       }

       [Given(@"the user should be logged in successfully")]
       public void CheckUserIsLoggedIn()
       {
          //If the login was successfull, check that the dashboard element exists.
          try
          {
               //Find dashboard element
               IWebElement dashboardElement = wait.Until(ExpectedConditions.ElementExists(By.ClassName("oxd-topbar-header-breadcrumb-module")));
               Assert.IsNotNull(dashboardElement, "Dashboard page did not load successfully"); 

               //Create a IsLoggedIn scenario context and set it to true.
               scenarioContext["IsLoggedIn"] = true;
               Console.WriteLine("Logged In Successfully");
          }
          catch
          {
               //If the dashboard Element is not found (or does not exist)
               scenarioContext["IsLoggedIn"] = false;
               Console.WriteLine("Logged In Unsuccessful");
               throw new Exception("Dashboard page did not load successfully"); 
          }
          
       }

       [Given(@"the user clicks on PIM")]
       public void UserClicksOnPIM()
       {
          //Check if the login was successful
          if (scenarioContext.ContainsKey("IsLoggedIn") && (bool)scenarioContext["IsLoggedIn"])
          {
               //Find the PIM option on the sidebar
               IWebElement PIM = driver.FindElement(By.CssSelector("a.oxd-main-menu-item[href='/web/index.php/pim/viewPimModule']"));
               //Click on Pim
               PIM.Click();
          }
          else
          {
               //If the log in was not successful, throw an exception with the relevant message
               throw new Exception("User is not logged in. Cannot click on PIM.");
          }
       }

       [When(@"the user clicks EmployeeList")]
       public void UserClicksEmployeeList()
       {
          // Wait for the EmployeeList option to be visible and click it
          IWebElement employeeListItem = wait.Until(ExpectedConditions.ElementExists(By.XPath("//a[text()='Employee List']")));
          employeeListItem.Click();
       }

       [Then(@"Verify the EmployeeList Page is Loaded")]
       public void VerifyEmployeeListPage()
       {
          try
          {
               // Wait until the URL contains "viewEmployeeList"
               wait.Until(driver => driver.Url.Contains("viewEmployeeList"));
               Console.WriteLine("Employee List page is loaded");
          }
          catch (WebDriverTimeoutException)
          {
               // Handle timeout exception
               throw new Exception("Employee List page did not load successfully");
          }
       }

     [When (@"the user clicks the Add button")]
     public void UserClicksAddEmployeeButton()
          {
               // Find the button element by its class name
                IWebElement addButton = wait.Until(ExpectedConditions.ElementExists(By.XPath("//i[@class='oxd-icon bi-plus oxd-button-icon']")));
                addButton.Click();

          }

     [When(@"Enters the employee first name and last name")]
     public void WhenEnterstheemployeefirstnameFirstNameandlastnameLastName()
     {
          //Enter the employee’s name
          IWebElement firstName_element = wait.Until(ExpectedConditions.ElementExists(By.Name("firstName")));
          IWebElement lastName_element = wait.Until(ExpectedConditions.ElementExists(By.Name("lastName")));

          firstName_element.SendKeys(firstname);
          lastName_element.SendKeys(lastname);

     }

     [When (@"the user toggles create login details button")]
     public void UserTogglesCreateLoginDetailsButton()
          {
               // Find the checkbox element by its class name inside span and click it
               IWebElement checkbox = driver.FindElement(By.XPath("//span[contains(@class, 'oxd-switch-input--active')]"));
               checkbox.Click();
          }

     [When (@"the user clicks the Save button")]
     public void UserClicksSaveEmployeeButton()
          {
               IWebElement saveButton = driver.FindElement(By.CssSelector("button[type='submit']"));
               saveButton.Click();
          }

     private IWebElement FindFieldGroup(string fieldName)
     {
        // Find the element with a given text
        IWebElement fieldGroup = wait.Until(ExpectedConditions.ElementExists(By.XPath($"//div[label[text()='{fieldName}']]")));
        //Return the parent of that element
        return fieldGroup.FindElement(By.XPath("./parent::div"));
     }
     private string FindErrorElement(string fieldName)
     {
        // Find the parent element with the given field name
        IWebElement fieldGroup = FindFieldGroup(fieldName);
     
        // Locate the span element within the parent element to find the error Message
        try
        {
          IWebElement errorMessage = fieldGroup.FindElement(By.XPath(".//span[contains(@class, 'oxd-input-field-error-message')]"));
          return errorMessage.Text;
        }
        catch
        {
          return string.Empty;
        }
        
     }     
     private IWebElement FindInputElement(string fieldName)
     {
        // Find the parent element containing the label and span
        IWebElement fieldGroup = FindFieldGroup(fieldName);
     
        // Locate the span element within the parent element
        return fieldGroup.FindElement(By.XPath(".//input[contains(@class, 'oxd-input')]"));
     }     

     [Then(@"validate the '(.*)' field are highlighted")]
     public void ThenvalidatetheRequiredfieldarehighlighted(string errorMessage)
     {
               //Find the error messages of each of the following fields
               string username_error = FindErrorElement("Username");
               string password_error = FindErrorElement("Password");
               string confirmPassword_error = FindErrorElement("Confirm Password");

               //All three messages should be the same.
               if (username_error.Equals(errorMessage) && password_error == errorMessage && confirmPassword_error == errorMessage)
               {
                    Console.WriteLine("All three Fields are Required - Pass ");
               }
               else
               {
                    Console.WriteLine("All three Field are not Required - Fail ");
                    throw new Exception("The required fields not showing required!  Fail");
               }

               //Sometimes the ID field throws an error if the id already exists.
               //Check the ID error field is blank as well or fail the test.
               string EmployeeIDError = FindErrorElement("Employee Id");
               if (EmployeeIDError.Equals(""))
               {
                    Console.WriteLine("Employee ID is good! - Pass ");
               }
               else if (EmployeeIDError.Equals("Employee Id already exists"))
               {
                    Console.WriteLine("Employee ID is good! - Pass ");
                    throw new Exception("The Employee ID is already exists, user cannot be added!  Fail");
               }
               else
               {
                    throw new Exception("The Employee ID field is throwing an error! -  Fail");
               }

     }

     [When(@"user fills mandatory fields username and password")]
     public void Whenuserfillsmandatoryfieldsusernameandpassword()
     {
          //Find the inpu text fields by looking for the tags above the text input
          IWebElement username_field = FindInputElement("Username");
          IWebElement password_field = FindInputElement("Password");
          IWebElement confirmPassword_field = FindInputElement("Confirm Password");

          //Enter username and password
          username_field.SendKeys(username);
          password_field.SendKeys(password);
          confirmPassword_field.SendKeys(password);
     }

     [When(@"validate errors are gone")]
     public void Thenvalidateerrorsaregone()
          {
               //The website can be slow to repsond sometimes and the required fields take longer than it should to disappear.
               int attempt = 0;
               int maxAttempts = 20;

               //Atempt up to 20 times to check the error messages are gone.
               while (attempt < maxAttempts)
               {
                    //Find the error messages.
                    string username_error = FindErrorElement("Username");
                    string password_error = FindErrorElement("Password");
                    string confirmPassword_error = FindErrorElement("Confirm Password");

                    //Check all error messages are blank
                    if (username_error.Equals("") && password_error.Equals("") && confirmPassword_error.Equals(""))
                    {
                         Console.WriteLine("All three Fields are good - Pass ");
                         break;
                    }
                    else
                    {
                         attempt++;
                    }
                    
               }

               // if the attempts reached max amount of attempts that means the errors messages didn't disappear.
               if (attempt == maxAttempts)
               {
                    Console.WriteLine("One of the three fields is still showing an error - Fail ");
                    throw new Exception("The required fields are not entered properly!  Fail");
               }

          }

     [Then(@"the snackbar shows '(.*)'")]
     public void Thentheemployeeisaddedsuccessfullytothelist(string snackbar_message)
     {
          //Wait until snackbar shows up. 
          IWebElement snackBar = wait.Until(ExpectedConditions.ElementExists(By.ClassName("oxd-text--toast-message")));
          Assert.IsNotNull(snackBar, "SnackBar message 'Successfully Updated' not found. Test failed.");
          
          
          
          // We should check the text of the snackbar to confirm it worked.
          int attempt = 0;
          int maxAttempts = 10;
          string snackbar_text = "";

          while (attempt < maxAttempts)
          {
               snackbar_text = snackBar.Text;
               Console.WriteLine("Snack bar message: " + snackbar_text);
               
               if (snackbar_text == snackbar_message)
               {
                    break;
               }

          }

          if (snackbar_text != snackbar_message)
          {
               throw new Exception($"The snackbar text did not show {snackbar_message}, actual text was {snackbar_text}-  Fail");
          }

     }

     [Then(@"verify the employee first name is '(.*)' in the search table")]
     public void ThenchecktheemployeenameAaaaainthesearchtable(string displayOption)
     {
          
          if (displayOption == "displayed")
          {
               try
               {
                    // Scroll to the bottom of the page
                    IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
                    jsExecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

                    // Take screenshot in headless mode
                    //((ITakesScreenshot)driver).GetScreenshot().SaveAsFile("headless_screenshot.png");

                    //Wait until the table cell containing the employeename is visible and exists
                    IWebElement userElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[contains(text(), '{firstname}')]")));
                    userElement = wait.Until(ExpectedConditions.ElementExists(By.XPath($"//div[contains(text(), '{firstname}')]")));
                    
                    //((ITakesScreenshot)driver).GetScreenshot().SaveAsFile("headless_screenshot.png");
                    // Assert that the employee name element is not null
                    Assert.IsNotNull(userElement, $"User '{firstname}' not found");
                    Console.WriteLine($"Employee Name '{firstname}' found on the list! - Pass");
               }
               catch
               {
                    //If employee not seen, throw error
                    //((ITakesScreenshot)driver).GetScreenshot().SaveAsFile("headless_screenshot2.png");
                    throw new Exception("Employee Name not found in the list!");
               }
          }
          else if (displayOption == "not displayed")
          {
               bool employeeNameAfterDeletion = driver.FindElements(By.XPath($"//div[contains(text(), '{firstname}')]")).Count > 0;
               //Assert.IsFalse(employeeNameAfterDeletion, $"Employee name '{first_name}'does not exist after deletion - Pass");
               if (employeeNameAfterDeletion)
               {
                    Assert.Fail($"Employee name '{firstname}' exists after deletion - Fail");
               }
               else
               {
                    Console.WriteLine($"Employee name '{firstname}' does not exist after deletion - Pass");
               }
          }

     }

     [Then(@"verify the '(.*)' coloumn is in '(.*)' order")]
     public void ThenverifytheFirstNamecoloumnisinascendingorder(string columnHeader, string columnOrder)
     {
  
          //Using the First Name parent find the sort icon and click on it
          IWebElement sortIcon = FindIconThroughHeaderElement(columnHeader);

          // Check if the sort icon indicates ascending order
          FindSortingOrder(sortIcon, columnOrder, columnHeader);

     }

     private void FindSortingOrder(IWebElement sortIcon, string columnOrder, string headerName)
     {

          //Check the class attribute of the sort icon to determine the sorting order
          string sortIconClass = sortIcon.GetAttribute("class");
          string sortingOrder;
          
          // Check if the sort icon indicates ascending order
          if (sortIconClass.Contains("bi-sort-alpha-down") )
          {
               sortingOrder = "ascending";
          }
          else if (sortIconClass.Contains("bi-sort-alpha-up"))
          {
               sortingOrder = "descending";
          }
          else
          {
               sortingOrder = "";
               throw new Exception($"The '{headerName}' coloumn order could not be found - Fail");
          }
          
          //Check if the column sorting order matches
          if (sortingOrder.Equals(columnOrder))
          {
               Console.WriteLine($"The '{headerName}' coloumn is in '{columnOrder}' order - Pass");
          }
          else
          {
               Console.WriteLine($"The '{headerName}' coloumn is in not in '{columnOrder}' order - Fail");
               throw new Exception($"The '{headerName}' coloumn is in not in '{columnOrder}' order - Fail");
          }

     }
     
     private IWebElement FindIconThroughHeaderElement(string headerName)
     {

          //find header cell (Parent) wth the given headerName
          IWebElement headerElement = wait.Until(ExpectedConditions.ElementExists(By.XPath($"//div[contains(@class, 'oxd-table-header-cell') and text()='{headerName}']")));
          //Using the header parent cell find and return the sort icon button
          return headerElement.FindElement(By.XPath(".//i[contains(@class, 'oxd-icon-button__icon')]"));

     }     

     [When(@"the user changes the '(.*)' coloumn to '(.*)' order")]
     public void WhentheuserchangestheLastNamecoloumntodescendingorder(string headerName, string columnOrder)
     {

          //Using the Last Name parent find the sort icon and click on it
          IWebElement sortIcon = FindIconThroughHeaderElement(headerName);
          sortIcon.Click();

          //Find the dropdownMenu and within that find the descending icon
          IWebElement dropdownMenu = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.--active[role='dropdown'][data-v-c9cca83c='']")));

          // set the column order as defined by clicking on the relevant option
          if (columnOrder == "ascending")
          {
               IWebElement sortingOption = dropdownMenu.FindElement(By.XPath(".//i[contains(@class, 'bi-sort-alpha-down')]"));
               sortingOption.Click(); 
          }
          else if (columnOrder == "descending")
          {
               IWebElement sortingOption = dropdownMenu.FindElement(By.XPath(".//i[contains(@class, 'bi-sort-alpha-up')]")); 
               sortingOption.Click();
          }
          else
          {
               throw new Exception($"The '{headerName}' coloumn order could not be determined - Fail");
          }

     }

[When(@"the user enters the first name and last name")]
public void WhentheuserentersthefirstnameAaaaaandlastnameAaaaaaa()
{
	//Fnd the first input element as it is the one for employee name. This bit could be done better.
     IWebElement employeeName = wait.Until(ExpectedConditions.ElementExists(By.ClassName("oxd-autocomplete-text-input"))).FindElement(By.TagName("input"));
     
     //((ITakesScreenshot)driver).GetScreenshot().SaveAsFile("headless_screenshot_input_before_entering_name.png");
     try
     {
          //Enter both first and last name
          employeeName.SendKeys(firstname + " " + lastname);
     }
     catch
     {
          //((ITakesScreenshot)driver).GetScreenshot().SaveAsFile("headless_screenshot_input_after_entering_name.png");
          throw new Exception($"The employee name could not be typed in - Fail");
     }


}

[When(@"the user clicks Search button")]
public void WhentheuserclicksSearchbutton()
{
     //Search button contains the 'submit' text.
	IWebElement saveButton = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("button[type='submit']")));
     saveButton.Click();
}

[When(@"double click on the employee name in the search table")]
public void WhendoubleclickontheemployeenameAaaaainthesearchtable()
{
	try
     {
          IWebElement employeeFirstName = wait.Until(ExpectedConditions.ElementExists(By.XPath($"//div[contains(text(), '{firstname}')]")));
          // Create an instance of Actions class
          Actions actions = new Actions(driver);

          // Double-click on the div element
          actions.DoubleClick(employeeFirstName).Perform();
     }
     catch
     {
          throw new Exception($"The employee name could not be clicked twice - Fail");
     }


        
}

[Then(@"Verify the first name and last name is same as the selected name")]
public void ThenVerifythefirstnameAaaaaandlastnameAaaaaaaissameastheselectedname()
{
     //Check the epmployee full name is showing on the user profile page.
	string fullName = firstname + ' ' + lastname;
     //IWebElement fieldElement = wait.Until(ExpectedConditions.ElementExists(By.XPath($"//div[@class='orangehrm-edit-employee-name']")));
     
     // Use a lambda expression as a custom condition - condition being that the text of the employee contains the full name.
     IWebElement employeeFirstName = wait.Until(driver =>
     {
          // Find the element
          IWebElement element = driver.FindElement(By.XPath("//div[@class='orangehrm-edit-employee-name']"));

          // Check if the text contains the expected value
          if (element.Text.Contains(fullName))
          {
               // Return the element if the condition is met
               Console.WriteLine($"The employee name matches - Pass");
               return element;
          }

          // Return null if the condition is not met (this will make the wait continue)
          return null;
     });

     // if null returned, then employee name was either not found or didn't match.
     if (employeeFirstName == null)
     {
          Console.WriteLine($"The employee name could not be checked or matched - Fail");
          throw new Exception($"The employee name could not be checked or matched - Fail");
     }

}

[When(@"the user clicks add button in the Attachments")]
public void WhentheuserclicksaddbuttonintheAttachments()
{
	// Scroll to the bottom of the page
     IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
     jsExecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

     //Find the plus icon in the Add buon and click it.
     IWebElement addButton = driver.FindElement(By.XPath("//i[@class='oxd-icon bi-plus oxd-button-icon']"));
     addButton.Click();

}

[When(@"clicks Browse button attached the file '(.*)'")]
public void WhenclicksBrowsebuttonattachedthefile(string fileName)
{
     //get the test file full path, file kept under the current directory.
     string currentDirectory = Directory.GetCurrentDirectory();
     string filePath = Path.Combine(currentDirectory, fileName);

     // Find the input element by class name
     IWebElement fileInput = wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".oxd-file-input")));
     // Attach file by entering full file path.
     fileInput.SendKeys(filePath);
}

[When(@"clicks the last Save button")]
public void WhenClickSavebutton()
{
     // Scroll to the bottom of the page
     IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
     jsExecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

     int maxAttempts = 100;
     int attempt = 0;

     //Sometimes it takes a while to determine all the elements.
     while (attempt < maxAttempts)
     {
     //Find all the save butons with the type submit.
     ReadOnlyCollection<IWebElement> saveButtons = driver.FindElements(By.CssSelector("button[type='submit']"));
     //Count the number of buttons found
     int saveButton_counts = saveButtons.Count;

     //check there is more than one save button, our save button is the bottom one.
     if (saveButton_counts > 2)
     {
          // Double click on the last button
          IWebElement lastSaveButton = saveButtons[saveButton_counts - 1];
          //lastSaveButton.Click();

          // Create an instance of Actions class
          Actions actions = new Actions(driver);
           // Double-click on the div element
          actions.DoubleClick(lastSaveButton).Perform();

          Console.WriteLine($"The attachment is Saved - Pass");
          break; // exit the loop if successful
     }

     attempt++;
     }

     //if the max attempts are reached and no save buttons found, then the test has failed.
     if (attempt == maxAttempts)
     {
          Console.WriteLine($"The attachment could not be saved! - Fail");
          throw new Exception($"The attachment  could not be saved! - Fail");
     }
}

[Then(@"Verify the file '(.*)' is '(.*)' in the Attachment table")]
public void ThenVerifythefilenameisdisplayedintheAttachmenttable(string fileName, string displayStatus)
{
     // Assert that the user element is not null
     if (displayStatus.Equals("displayed"))
     {
          try
          {
               // Fild the element that contains a cell with text that contains the filename.
               IWebElement attachmentFile = wait.Until(ExpectedConditions.ElementExists(By.XPath($"//div[contains(text(), '{fileName}')]")));
               Assert.IsNotNull(attachmentFile, $"Attachment File not found - Fail!");
               Console.WriteLine($"attached file is {displayStatus}");
          }
          catch
          {
               Console.WriteLine($"attached file is {displayStatus} - Error");
               throw new Exception("Attachment file is not displayed! - Error");
          }

     }
     // Assert that the user element does not exist
     else if (displayStatus.Equals("not displayed"))
     {
          try
          {
               // // Fild the element that contains a cell with text that contains the filename.
               // IWebElement attachmentFile = wait.Until(ExpectedConditions.ElementExists(By.XPath($"//div[contains(text(), '{fileName}')]")));
          
               // Assert.IsNull(attachmentFile, $"Attachment File found - Fail!");

               // Check if there is an element which contains the filename as a text.  
               bool attachmentFileAfterDeletion = driver.FindElements(By.XPath($"//div[contains(text(), '{fileName}')]")).Count > 0;
               // If there isn't any then pass the test.
               Assert.IsFalse(attachmentFileAfterDeletion, $"Attachment File '{fileName}'does not exist after deletion - Pass");
               Console.WriteLine($"attached file is {displayStatus}");
          }
          catch
          {
               Console.WriteLine($"attached file is {displayStatus} - Error");
               throw new Exception("Attachment file is displayed! - Error");
          }

     }
     else
     {
          throw new Exception("Verification not understood - Error");
     }

}

[When(@"the user Deletes the attached file '(.*)'")]
public void WhentheuserDeletestheattachedfile(string fileName)
{
     try
     {
          //Currently it is the only trash button so easy to fine. But we should code it better, in case there is more than one file.
          IWebElement trashButton = wait.Until(ExpectedConditions.ElementExists(By.XPath("//i[@class='oxd-icon bi-trash']")));
          trashButton.Click();

          //((ITakesScreenshot)driver).GetScreenshot().SaveAsFile("trashbutton_clicked.png");

          // Wait for the confirmation trash button to be clickable
          IWebElement confirmTrashButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//i[@class='oxd-icon bi-trash oxd-button-icon']")));
          confirmTrashButton.Click();
          //((ITakesScreenshot)driver).GetScreenshot().SaveAsFile("confirmrashbutton_clicked.png");
     }
     catch
     {
          //((ITakesScreenshot)driver).GetScreenshot().SaveAsFile("trashbutton_error.png");
          throw new Exception("Attachment File could not be deleted - Fail!");
     }
}


[When(@"the user deletes the employee")]
public void Whentheuserdeletestheemployee()
{
     //Currently it is the only trash button so easy to fine. But we should code it better, in case there is more than one file.
     IWebElement trashButton = wait.Until(ExpectedConditions.ElementExists(By.XPath("//i[@class='oxd-icon bi-trash']")));
     trashButton.Click();

     // Wait for the confirmation trash button to be clickable
    IWebElement confirmTrashButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//i[@class='oxd-icon bi-trash oxd-button-icon']")));
    confirmTrashButton.Click();
}

     [AfterScenario]
     public void TearDown()
     {
          // Cleanup code, such as closing the browser
          Console.WriteLine("Closing driver");
          //shutdown webdriver
          driver.Quit();
     }

    }
}