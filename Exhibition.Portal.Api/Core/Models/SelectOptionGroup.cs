using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exhibition.Portal.Api.Models
{
    public class SelectOptionGroup<T>
    {
        public string Name { get; set; }
        public T[] Items { get; set; }
    }
    //public class SelectOption
    //{
    //    public string Name { get; set; }
    //    public string Text { get; set; }
    //}
}
