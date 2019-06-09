using System;
using System.Collections.Generic;
using System.Text;

namespace Exhibition.Core.Models
{
    public class ResourceEqualityComparer : IEqualityComparer<Resource>
    {
        public static ResourceEqualityComparer OrdinalIgnoreCase
        {
            get
            {
                return new ResourceEqualityComparer();
            }
        }

        public bool Equals(Resource x, Resource y)
        {
            return x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase)
                && x.FullName.Equals(y.Name, StringComparison.OrdinalIgnoreCase)
                && x.Type.Equals(y.Type)
                && x.Workspace.Equals(y.Workspace, StringComparison.OrdinalIgnoreCase)
                && x.Sorting.Equals(y.Sorting);
        }

        public int GetHashCode(Resource obj)
        {
            return obj.SerializeToJson().GetHashCode();
        }
    }
}
