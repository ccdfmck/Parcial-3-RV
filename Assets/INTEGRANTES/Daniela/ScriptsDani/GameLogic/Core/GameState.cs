namespace TamagotchiFactory
{
    /// <summary>Estados del flujo del juego: Menu -> Onboarding -> Playing -> Closing.</summary>
    public enum GameState
    {
        Menu,        // Menú de inicio (Rol 3)
        Onboarding,  // Tutorial (Rol 3)
        Playing,     // Partida con temporizador
        Closing      // Pantalla final (Rol 3)
    }
}
