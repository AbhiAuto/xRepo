Feature: TC05_ValidatingAPIFunctionalities

@api
Scenario Outline: 01-Validating ClearStdCheck API when the payment status is 27
	Given I have "ClearStdCheck" api
	And I have request json with e-Payment Check Cleared '<payment number>' and '<amount>'
	When I send a "Post" request
	Then the response should be "400"
	And the reponse "Errors[0].Message" should contain "ValidationError - Failure performing State_Auto_Advance action for paymentID: 703301 with paymentType: Cleared with payment processing status type: e-Payment Check Cleared. Please refresh and try again."
	Examples: 
	| payment number | amount |
	| 4187000084     | 505.00 |
@api
Scenario Outline: 02-Validating ClearStdCheck API for used amount of valid payment id
	Given I have "ClearStdCheck" api
	And I have request json with valid '<payment number>' and used '<amount>'
	When I send a "Post" request
	Then the response should be "500"
	Examples: 
	| payment number | amount |
	| 4187000084     | 510.00 |