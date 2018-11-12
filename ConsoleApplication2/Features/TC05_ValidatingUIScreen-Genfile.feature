Feature: TC05-Validating the status change for a Check payment from Issued to Cleared based on the confirmation received from the bank

@ftp @GenerateBAI2file
Scenario: 01-Uploading BAI2 file to FTP location
	Given FTP folder location "\\sftp.avidxchange.com\Avidpaytest\Integration\FIFTHTHIRD\BAI2_AZRFSWDVS02"
	When I upload the generated BAI2 file 
	Then upload file in the FTP location should be successful

Scenario: 02-Validating BAI2 file name updated in FTP location
	Given FTP folder location "\\sftp.avidxchange.com\Avidpaytest\Integration\FIFTHTHIRD\BAI2_AZRFSWDVS02"
	When the user waits for "70" seconds 
	Then the generated file extension should change to ".loaded"

@Web 
Scenario: 03-Validating check status on AvidPay Internal portal
	Given I have logged into "AvidPay Internal" portal
	And I navigate to "Manage Payments" page 
	And I select the "customer" from the customer listbox
	And I enter "02/01/2018,10/31/2018" in the date feilds
	When I filter the result with the payment number 
	Then I should see the status of payment number is "e-Payment Check Cleared"