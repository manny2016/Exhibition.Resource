

namespace Exhibition.Portal.Api.Models
{
    using Exhibition.Core;
    using Models = Exhibition.Core.Models;
    public class TerminalActionResponse : BasicResponse<IBaseTerminal>
    {

    }
    public class QueryTerminalResponse : BasicResponse<IBaseTerminal>
    {

    }
    public class GeneralResponse : BasicResponse<int?>
    {
        public GeneralResponse()
        {
        }
    }
    public class QueryDirectiveResponse : BasicResponse<Models::Directive[]>
    {

    }
}
