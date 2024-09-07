Feature: Rock-paper-scissors game with two players
  In order to resolve a dispute
  As a grown up human
  I want to be able to play rock-paper-scissors with others
  
  Scenario: Player01 plays Scissors and Player02 plays Paper
    Given Game is created
    And Second player joined
    And Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" made a move "scissors"
    When Player "229b3b08-749c-48e9-8ca9-031914f83377" makes a move "paper"
    Then Winner is player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32"

  Scenario: Player01 plays Scissors and Player02 plays Rock
    Given Game is created
    And Second player joined
    And Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" made a move "scissors"
    When Player "229b3b08-749c-48e9-8ca9-031914f83377" makes a move "rock"
    Then Winner is player "229b3b08-749c-48e9-8ca9-031914f83377"

  Scenario: Player01 plays Paper and Player02 plays Rock
    Given Game is created
    And Second player joined
    And Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" made a move "paper"
    When Player "229b3b08-749c-48e9-8ca9-031914f83377" makes a move "rock"
    Then Winner is player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32"

  Scenario: Player01 plays Scissors and Player02 plays Scissors
    Given Game is created
    And Second player joined
    And Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" made a move "scissors"
    When Player "229b3b08-749c-48e9-8ca9-031914f83377" makes a move "scissors"
    Then Game is a draw

  Scenario: Player01 plays Paper and Player02 plays Paper
    Given Game is created
    And Second player joined
    And Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" made a move "paper"
    When Player "229b3b08-749c-48e9-8ca9-031914f83377" makes a move "paper"
    Then Game is a draw

  Scenario: Player01 plays Rock and Player02 plays Rock
    Given Game is created
    And Second player joined
    And Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" made a move "rock"
    When Player "229b3b08-749c-48e9-8ca9-031914f83377" makes a move "rock"
    Then Game is a draw
    