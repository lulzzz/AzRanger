﻿using AzRanger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzRanger.Checks.Rules
{
    [RuleMeta("TeamsExternalCommunicationFederated", ScopeEnum.Teams, MaturityLevel.Mature, "https://admin.teams.microsoft.com/company-wide-settings/external-communications")]
    [RuleInfo("Teams allow communication with arbitrary other compandy teams user", "Allowing user to communicate outside the organization increases the risk of phishing and data leakage.", 2, "https://danielchronlund.com/2021/02/22/manage-teams-external-access-for-allowed-domains-using-powershell-and-teams-approvals/", null, @"Go to the Portal Url and check the setting under ""Choose which external domains your users have access to"" either to ""Allow oonly specific external domains"" or ""Block all external domains"".")]
    class TeamsExternalCommunicationFederated : BaseCheck
    {
        public override CheckResult Audit(Tenant tenant)
        {
            if(tenant.TeamsSettings.TenantFederationSettings.AllowFederatedUsers == false)
            {              
                 return CheckResult.NoFinding;    
            }
            else
            {
                if (tenant.TeamsSettings.TenantFederationSettings.AllowedDomains.AllowedDomain != null)
                {
                    return CheckResult.NoFinding;
                }
            }
            return CheckResult.Finding;
        }
    }
}