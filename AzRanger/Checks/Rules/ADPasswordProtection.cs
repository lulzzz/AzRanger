﻿using AzRanger.Models;

namespace AzRanger.Checks.Rules
{
    [RuleMeta("ADPasswordProtection", ScopeEnum.AAD, MaturityLevel.Mature, "https://entra.microsoft.com/#view/Microsoft_AAD_IAM/AuthenticationMethodsMenuBlade/~/PasswordProtection")]
    class ADPasswordProtection : BaseCheck
    {
        public override CheckResult Audit(Tenant tenant)
        {
            if (tenant.TenantSettings.PasswordPolicy.enableBannedPasswordCheckOnPremises == true)
            {
                return CheckResult.NoFinding;
            }
            return CheckResult.Finding;
        }
    }
}
