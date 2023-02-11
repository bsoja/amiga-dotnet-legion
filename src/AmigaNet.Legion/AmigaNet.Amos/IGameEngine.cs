using AmigaNet.Types.Graphics;

namespace AmigaNet.Amos
{
    public interface IGameEngine
    {
        //ImageData LoadIff(string fileName);

        //ImageData Load(string fileName);

        void LoadTrack(string fileName);

        bool IsTrackLoop { get; set; }

        void PlayTrack();

        void StopTrack();

        void HideCursor();
        
        void ShowCursor();

        void ChangeMouseCursor(ImageData cursorImage);

        void WaitVbl();

        int GetKeyPressed();
        
        String GetInkey();

        int GetScancode();

        void ClearKey();

        int GetMousePosX();

        int GetMousePosY();

        int GetMouseKey();

        int GetMouseClick();
    }
}
