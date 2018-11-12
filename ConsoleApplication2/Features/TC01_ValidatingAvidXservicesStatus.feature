Feature: TC01-ValidatingAvidXservicesStatus
	

Scenario: 01-validating the status of avidStdCheckClearing and AvidBAI2Parser services
	Given I have list of Windows services 
	When I check for the status of "AvidBAI2Parser" and "AvidStdCheckClearing"
	Then the status should be "Running"
