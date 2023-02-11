namespace AmigaNet.Amos.Screens.Amal
{
    public class AmalBuilder
    {
        private List<AmalInstruction> instructions = new List<AmalInstruction>();

        public AmalBuilder Move(int horizontal, int vertical, int step)
        {
            instructions.Add(new AmalMove { Horizontal = horizontal, Vertical = vertical, Step = step });
            return this;
        }

        public AmalBuilder Jump(String label)
        {
            instructions.Add(new AmalJump { Label = label });
            return this;
        }

        public AmalBuilder Label(String name)
        {
            instructions.Add(new AmalLabel { Name = name });
            return this;
        }

        public List<AmalInstruction> Compile()
        {
            return instructions;
        }
    }
}
