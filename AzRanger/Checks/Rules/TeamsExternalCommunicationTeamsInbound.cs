﻿using AzRanger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzRanger.Checks.Rules
{
    [RuleMeta("TeamsExternalCommunicationInbound", ScopeEnum.Teams, MaturityLevel.Mature, "https://admin.teams.microsoft.com/company-wide-settings/external-communications")]
    [RuleInfo("Teams inbound communication with users outside your organization is allowed", "Allowing user to communicate outside the organization increases the risk of phishing and data leakage.", 7, "https://danielchronlund.com/2021/02/22/manage-teams-external-access-for-allowed-domains-using-powershell-and-teams-approvals/", null, @"Go to the Portal Url and check the setting under ""Teams accounts not managed by an organization"".")]
    class TeamsExternalCommunicationInbound : BaseCheck
    {
        public override CheckResult Audit(Tenant tenant)
        {
            if(tenant.TeamsSettings.TenantFederationSettings.AllowTeamsConsumer == true)
            {
                if (tenant.TeamsSettings.TenantFederationSettings.AllowTeamsConsumerInbound == false)
                {
                    return CheckResult.NoFinding;
                }
            }
            else
            {
                return CheckResult.NoFinding;
            }
            return CheckResult.Finding;
        }
    }
}
