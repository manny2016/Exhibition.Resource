﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Exhibition.Agent.Show;
using Exhibition.Core.Models;

namespace Exhibition.Core.Services
{
    public class OperationService : IOperationService
    {
        public string Readme()
        {
            return "this is a wcf api for control center.";
        }

        public GeneralResponse<int> Run(Directive directive)
        {
            AgentHost.TriggerDirectiveEvent(this, directive);
            return new GeneralResponse<int>()
            {
                Success = true,
                Data = 1
            };
        }

        public void Shutdown()
        {
            Application.Exit();
        }
    }
}
