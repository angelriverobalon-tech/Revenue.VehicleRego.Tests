Feature: Get Author Details
As a user, I want to retrieve the Author's Details so that I can view the Author's information.


@API
Scenario: Author Details Retrieval
	When I request the author details
	Then the response status should be 200
	And the response JSON should contain "personal_name" with value "Sachi Rautroy"
	And the response JSON array "alternate_names" should contain "Yugashrashta Sachi Routray"
		