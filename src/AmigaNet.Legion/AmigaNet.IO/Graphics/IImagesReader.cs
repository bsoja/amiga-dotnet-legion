using AmigaNet.Types.Graphics;

namespace AmigaNet.IO.Graphics
{
    public interface IImagesReader
    {
        String Name { get; }

        ImagesContainer Read(String fileName);
    }
}
