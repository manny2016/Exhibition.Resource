

namespace Exhibition.Portal.Api.Models
{
    using Models = Exhibition.Core.Models;
    public class TerminalActionResponse : BasicResponse<Models::Terminal>
    {

    }
    public class QueryTerminalResponse : BasicResponse<Models::Terminal[]>
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
