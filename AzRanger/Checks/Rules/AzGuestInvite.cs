﻿using AzRanger.Models;

namespace AzRanger.Checks.Rules
{
    [RuleMeta("AzGuestInvite", ScopeEnum.AAD, MaturityLevel.Mature, "https://entra.microsoft.com/#view/Microsoft_AAD_IAM/AllowlistPolicyBlade")]
    [CISAZ("1.16", "", CISLevel.L2, "v2.0")]
    
    internal class AzGuestInvite : BaseCheck
    {
        public override CheckResult Audit(Tenant tenant)
        {
            if(tenant.TenantSettings.AuthorizationPolicy.allowInvitesFrom == "adminsAndGuestInviters" | tenant.TenantSettings.AuthorizationPolicy.allowInvitesFrom == "none")
            {
                return CheckResult.NoFinding;
            }
            return CheckResult.Finding;
        }
    }
}
