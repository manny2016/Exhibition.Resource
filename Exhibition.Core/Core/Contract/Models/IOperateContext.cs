using System;
using System.Collections.Generic;
using System.Text;

namespace Exhibition.Core
{
    using Models = Exhibition.Core.Models;
    public interface IOperateContext
    {
        Models::Directive Directive { get; }
        DirectiveTypes Type { get; }
    }
}
