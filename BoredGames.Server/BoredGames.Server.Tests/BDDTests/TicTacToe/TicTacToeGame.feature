Feature: Tic-Tac-Toe game with two players
  In order to pass some time
  As a grown up human
  I want to be able to play tic-tac-toe with others
  
  Scenario: Player01 plays X and Player02 plays O and Player01 wins with pattern 7
    Given Game is created
    And Second player joined
    And Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" made a move "x" at row 0 and column 0
    And Player "229b3b08-749c-48e9-8ca9-031914f83377" made a move "o" at row 0 and column 2
    And Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" made a move "x" at row 1 and column 1
    And Player "229b3b08-749c-48e9-8ca9-031914f83377" made a move "o" at row 1 and column 2
    When Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" makes a move "x" at row 2 and column 2
    Then Winner is player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32"

  Scenario: Player01 plays X and Player02 plays O and the game is a Draw
    Given Game is created
    And Second player joined
    And Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" made a move "x" at row 0 and column 0
    And Player "229b3b08-749c-48e9-8ca9-031914f83377" made a move "o" at row 0 and column 2
    And Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" made a move "x" at row 1 and column 1
    And Player "229b3b08-749c-48e9-8ca9-031914f83377" made a move "o" at row 2 and column 2
    And Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" made a move "x" at row 1 and column 2
    And Player "229b3b08-749c-48e9-8ca9-031914f83377" made a move "o" at row 1 and column 0
    And Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" made a move "x" at row 0 and column 1
    And Player "229b3b08-749c-48e9-8ca9-031914f83377" made a move "o" at row 2 and column 1
    When Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" makes a move "x" at row 2 and column 0
    Then Game is a draw
    