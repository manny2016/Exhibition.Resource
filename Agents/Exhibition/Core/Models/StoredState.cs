


namespace Exhibition.Agent.Show.Models
{
    using System.IO;
    using Exhibition.Core;
    public class StoredState
    {
        const string FileName = "StoredState.inf";
        public void Save()
        {

            File.Delete(FileName);
            using (var stream = new FileStream(FileName, FileMode.Create))
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(this.SerializeToJson());
                writer.Flush();
            }
        }
        public static StoredState Instance
        {
            get
            {
                if (!File.Exists(FileName)) return new StoredState() { Last = null };
                return File.ReadAllText(FileName).DeserializeToObject<StoredState>();
            }
        }
        public OperationContext Last { get; set; }
    }
}
