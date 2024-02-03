Feature: Add User to OrangeHR

Background: Log In
    Given the user is on the login page 'https://opensource-demo.orangehrmlive.com/'
    And the user enters the username 'Admin' and password 'admin123'
    And the user should be logged in successfully

Scenario: Test 1: Navigate to EmployeeList
    Given the user clicks on PIM
    When the user clicks EmployeeList
    Then Verify the EmployeeList Page is Loaded

Scenario: Test 2: Verify user can add a new employee in the Add Employee page
    Given the user clicks on PIM
    When the user clicks EmployeeList
    Then Verify the EmployeeList Page is Loaded
    When the user clicks the Add button
    And Enters the employee first name and last name
    And the user toggles create login details button
    And the user clicks the Save button
    Then validate the 'Required' field are highlighted
    When user fills mandatory fields username and password
    And validate errors are gone
    And the user clicks the Save button
    Then the snackbar shows 'Successfully Saved'

Scenario: Test 3: Verify employee is seen on the list
    Given the user clicks on PIM
    When the user clicks EmployeeList
    Then Verify the EmployeeList Page is Loaded
    When the user enters the first name and last name
    And the user clicks Search button
    Then verify the employee first name is 'displayed' in the search table

Scenario: Test 4: Verify employee details table can be sorted by name
    Given the user clicks on PIM
    When the user clicks EmployeeList
    Then Verify the EmployeeList Page is Loaded
    Then verify the 'First (& Middle) Name' coloumn is in 'ascending' order
    When the user changes the 'Last Name' coloumn to 'descending' order
    Then verify the 'Last Name' coloumn is in 'descending' order

Scenario: Test 5: Verify adding and deleting attachments to employee record
    Given the user clicks on PIM
    When the user clicks EmployeeList
    Then Verify the EmployeeList Page is Loaded
    When the user enters the first name and last name
    And the user clicks Search button
    Then verify the employee first name is 'displayed' in the search table
    When double click on the employee name in the search table
    Then Verify the first name and last name is same as the selected name
    When the user clicks add button in the Attachments
    And clicks Browse button attached the file '<filename>'
    And clicks the last Save button
    Then Verify the file '<filename>' is 'displayed' in the Attachment table
    When the user Deletes the attached file '<filename>'
    Then Verify the file '<filename>' is 'not displayed' in the Attachment table    

Examples:
    | filename     |
    | testfile.txt |
    
Scenario: Test 6: Verify employee is deleted
    Given the user clicks on PIM
    When the user clicks EmployeeList
    Then Verify the EmployeeList Page is Loaded
    When the user enters the first name and last name
    And the user clicks Search button
    Then verify the employee first name is 'displayed' in the search table
    When the user deletes the employee
    Then verify the employee first name is 'not displayed' in the search table

