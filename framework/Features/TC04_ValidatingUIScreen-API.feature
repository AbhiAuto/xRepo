Feature: TC04-Validating the status change for a Check payment from Issued to Cleared based on the confirmation received from the bank

@api
Scenario: 01-Validating ClearStdCheck API when the payment status is 14
	Given I have "ClearStdCheck" api
	And I have request json with payment number and amount
	When I send a "Post" request
	Then the response should be "200"

Scenario: 02-Validating database status change to 27
	Given I have AvidPayTransaction database
	When  I query for "PaymentProcessingStatusTypeID" status
	Then I should see the status as "27"

@Web
Scenario: 03-Validating check status on AvidPay Internal portal
	Given I have logged into "AvidPay Internal" portal
	And I navigate to "Manage Payments" page 
	And I select the "customer" from the customer listbox
	And I enter "02/01/2018,10/31/2018" in the date feilds
	When I filter the result with the payment number 
	Then I should see the status of payment number is "e-Payment Check Cleared"