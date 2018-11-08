Feature: TC03_ValidatingAPIFunctionalities
	
@api
Scenario Outline: 01-Validating ClearStdCheck API when the payment status is 14
	Given I have "ClearStdCheck" api
	And I have request json with invalid '<payment number>' and '<amount>'
	When I send a "Post" request
	Then the response should be "400"
	Examples: 
	| payment number | amount |
	| 4187000084     | 505.00 |
