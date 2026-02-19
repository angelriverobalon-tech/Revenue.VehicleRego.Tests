Feature: Motor Vehicle Registration
As a user
I want to navigate to the Motor Vehicle Registration page
So that I can see the calculated amount for my vehicle registration

@GUI
Scenario Outline: Motor Vehicle Registration Calculation
	Given I am in the Check Motor Vehicle Stamp Duty page
	When I check online for the Motor Vehicle Registration
	And I register for a passenger vehicle
	And I calculate the '<purchasePrice>' for the vehicle
	Then I should see calculated amount for the Motor Vehicle Registration with Duty payable as '<dutyPayable>'
	And the Vehicle is a Passenger Vehicle

Examples:
	| purchasePrice | dutyPayable | isPassengerVehicle |
	|      45000.00 |     1350.00 | Yes                |