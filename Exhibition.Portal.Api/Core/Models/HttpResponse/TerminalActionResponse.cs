

namespace Exhibition.Portal.Api.Models
{
    using Models = Exhibition.Core.Models;
    public class TerminalActionResponse : Response<Models::Terminal>
    {

    }
    public class QueryTerminalResponse : Response<Models::Terminal[]>
    {

    }
    public class GeneralResponse : Response<int?>
    {
        public GeneralResponse()
        {
        }
    }
    public class QueryDirectiveResponse : Response<Models::Directive[]>
    {

    }
}
