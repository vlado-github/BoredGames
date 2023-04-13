Feature: Rock-paper-scissors game with two players
  In order to resolve a dispute
  As a grown up human
  I want to be able to play rock-paper-scissors with others
  
Scenario: Player01 plays Scissors and Player02 plays Rock
  Given The game is created
  And The second player joined
  And Player "fdd28238-aecb-4a4a-a7b0-a0ab43becf32" made a move "scissors"
  When Player "229b3b08-749c-48e9-8ca9-031914f83377" makes a move "rock"
  Then Winner is player "229b3b08-749c-48e9-8ca9-031914f83377"
    