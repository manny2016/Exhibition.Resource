


namespace Exhibition.Agent.Show.Models
{
    using System.IO;
    using Exhibition.Core;
    using System.Collections.Generic;
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
        public Dictionary<int, OperationContext> Last
        {
            get; set;
        }
        private static StoredState instance;
        private static object lockObject = new object();
        public static StoredState Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            lock(lockObject)
                            {
                                if (!File.Exists(FileName)) return new StoredState() { Last = new Dictionary<int, OperationContext>() };
                                instance = File.ReadAllText(FileName).DeserializeToObject<StoredState>();
                            }                       
                        }
                    }
                }
                return instance;

            }
        }

    }
}
