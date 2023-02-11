namespace AmigaNet.Amos.Screens.Amal
{
    class AmalAnim : AmalInstruction
    {
        public int Times { get; set; }

        public List<AmalAnimImageDelay> Images { get; set; } = new List<AmalAnimImageDelay>();

    }

    class AmalAnimImageDelay
    {
        public int Image { get; set; }

        public int Delay { get; set; }
    }
}
