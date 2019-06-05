



namespace Exhibition.Agent.Show.Models
{
    using Exhibition.Core;
    using Exhibition.Core.Models;
    using System.Linq;

    public class WorkingState
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ResourceTypes Type
        {
            get
            {
                if (this.Resources.All(o => o.Type == ResourceTypes.Image))
                    return ResourceTypes.Image;
                if (this.Resources.All(o => o.Type == ResourceTypes.Video))
                    return ResourceTypes.Video;
                if (this.Resources.All(o => o.Type == ResourceTypes.H5))
                    return ResourceTypes.H5;

                return ResourceTypes.NotSupported;
            }
        }

        public Window Window { get; set; }

        public int Current { get; set; }

        public Resource[] Resources { get; set; }

        public IOperate Operator { get; set; }
    }
}
