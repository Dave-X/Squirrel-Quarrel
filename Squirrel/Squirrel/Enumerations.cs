public enum ObstacleCategory
{
    Rock,


} // ObstacleCategory

public enum PowerupCategory
{
    Speed_Boost,
    Rapid_Shot,
    Spread_Shot,
    Explosive_Shot,
} // PowerupCategory

public enum GameStates
{
    Active, // Game is being played.
    Paused, // Show the pause screen.
    Main_Menu, 
    Game_Over, // Show stats.
    New_Round, // Random stuff.
    // center map
    // randomly generate everything
    // create a new sprite manager
    // sprite load junk
    // 
}

public enum PlayerDirection
{
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest,
}