
namespace Exhibition.Core.Models
{
    public class MediaPlayerTerminal : ITerminal<MedaiPlayerSettings>
    {
        public MedaiPlayerSettings Settings { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TerminalTypes Type { get { return TerminalTypes.MediaPlayer; } }

        public string GetSettings()
        {
            return this.Settings.SerializeToJson();
        }
    }
}
