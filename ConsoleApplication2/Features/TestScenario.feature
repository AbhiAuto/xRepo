Feature: Validating the status change for a Check payment from Issued to Cleared based on the confirmation received from the bank

@ftp @GenerateBAI2file
Scenario: Uploading BAI2 file to FTP location
	Given FTP folder location "//sftp.avidxchange.com/Avidpaytest/Integration/FIFTHTHIRD/BAI2_AZRFSWDVS02"
	When I upload the generated BAI2 file 
	Then upload file in the FTP location should be successful

@Web
Scenario: Validating check status on AvidPay Internal portal
	Given I have logged into "AvidPay Internal" portal
	And I navigate to "Manage Payments" page 
	And I select the "customer" from the customer listbox
	And I enter "02/01/2018,10/31/2018" in the date feilds
	When I filter the result with the payment number 
	Then I should see the status of payment number is "e-Payment Check Cleared"
	
