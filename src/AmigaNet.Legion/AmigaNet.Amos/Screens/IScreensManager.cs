namespace AmigaNet.Amos.Screens
{
    public interface IScreensManager
    {
        List<Screen> Screens { get; }

        Display Display { get; }

        bool UpdateDisplayRequested { get; set; }

        bool ShouldUpdateBobs { get; set; }

        bool AutoUpdateBobs { get; }
    }
}