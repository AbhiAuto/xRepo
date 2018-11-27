Feature: TC01-ValidatingAvidXservicesStatus
	

Scenario: 01-validating the status of avidStdCheckClearing and AvidBAI2Parser services
	Given I have list of Windows services 
	When I check for the status of "AvidBAI2Parser" and "AvidStdCheckClearing"
	Then the status should be "Running"

@ftp @GenerateBAI2file
Scenario: 02-validating Payment Amount in the generated BAI2 File
	Given I have genrocket generated BAI2 File
	When I open the BAI2 file and review Payment Amount
	Then Payment Amount should be greater than zero