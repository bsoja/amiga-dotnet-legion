using Microsoft.Xna.Framework.Audio;
using SharpMod;

namespace AmigaNet.Legion.DesktopApp
{
    public class XnaSoundRenderer : IRenderer, IDisposable
    {
        byte[] buf = new byte[8192];

        DynamicSoundEffectInstance _dsei;

        public XnaSoundRenderer(DynamicSoundEffectInstance dsei)
        {
            _dsei = dsei;
            _dsei.BufferNeeded += OnBufferNeeded;
        }

        public void Init()
        {
        }

        public void PlayStart()
        {
            _dsei.Play();
        }

        public void PlayStop()
        {
            _dsei.Stop();
        }

        public ModulePlayer Player { get; set; }

        private void OnBufferNeeded(object sender, EventArgs e)
        {
            if (Player != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    this.Player.GetBytes(buf, 8192);
                    _dsei.SubmitBuffer(buf);
                }
            }
            else
            {
                _dsei.SubmitBuffer(buf);
            }
        }

        public void Dispose()
        {
            Player = null;
            _dsei.BufferNeeded -= OnBufferNeeded;
            _dsei = null;
        }
    }
}
