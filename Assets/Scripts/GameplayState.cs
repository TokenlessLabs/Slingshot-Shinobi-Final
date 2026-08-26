public static class GameplayState
{
    public static bool IsTerminal { get; private set; }
    public static bool IsPaused { get; private set; }
    public static bool IsPowerupOpen { get; private set; }

    public static void BeginTerminalState()
    {
        IsTerminal = true;
    }

    public static void Reset()
    {
        IsTerminal = false;
        IsPaused = false;
        IsPowerupOpen = false;
    }

    public static void SetPaused(bool paused)
    {
        IsPaused = paused;
    }

    public static void SetPowerupOpen(bool open)
    {
        IsPowerupOpen = open;
    }

    public static void StopGameplayAudio()
    {
        foreach (UnityEngine.AudioSource audioSource in UnityEngine.Object.FindObjectsOfType<UnityEngine.AudioSource>())
        {
            audioSource.Stop();
        }
    }

    public static void DisablePlayerGameplay()
    {
        UnityEngine.GameObject player = UnityEngine.GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            return;
        }

        Disable<PlayerMovementWithJoystick>(player);
        Disable<Dash>(player);
        Disable<PlayerShooting>(player);
        Disable<PlayerCollecting>(player);
    }

    private static void Disable<T>(UnityEngine.GameObject player) where T : UnityEngine.Behaviour
    {
        T component = player.GetComponent<T>();
        if (component != null)
        {
            component.enabled = false;
        }
    }
}