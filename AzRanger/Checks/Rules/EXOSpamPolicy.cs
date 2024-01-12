﻿using AzRanger.Models;
using AzRanger.Models.ExchangeOnline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzRanger.Checks.Rules
{
    [RuleMeta("EXOSpamPolicy", ScopeEnum.EXO, MaturityLevel.Mature, "https://security.microsoft.com/antispam")]    
    class EXOSpamPolicy : BaseCheck
    {
        public override CheckResult Audit(Tenant tenant)
        {
            foreach(HostedOutboundSpamFilterPolicy policy in tenant.ExchangeOnlineSettings.HostedOutboundSpamFilterPolicy)
            {
                if (policy.BccSuspiciousOutboundMail & policy.NotifyOutboundSpam & policy.IsDefault)
                {
                    return CheckResult.NoFinding;
                }
            }
            return CheckResult.Finding;
        }
    }
}
