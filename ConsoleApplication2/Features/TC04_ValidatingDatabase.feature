Feature: TC04_ValidatingDatabase
	
@ftp @GenerateBAI2file
Scenario: 01-Uploading BAI2 file to FTP location with paymentid status 14
	Given FTP folder location "\\sftp.avidxchange.com\Avidpaytest\Integration\FIFTHTHIRD\BAI2_AZRFSWDVS02\"
	When I upload the generated BAI2 file
	Then upload file in the FTP location should be successful

Scenario: 02-Validating BAI2 file name updated in FTP location
	Given FTP folder location "\\sftp.avidxchange.com\Avidpaytest\Integration\FIFTHTHIRD\BAI2_AZRFSWDVS02\"
	When the user waits for "70" seconds 
	Then the generated file extension should change to ".loaded"

Scenario: 03-Validating database status change to 27
	Given I have AvidPayTransaction database
	When  I query for "PaymentProcessingStatusTypeID" status
	Then I should see the status as "27"
