﻿using AzRanger.Models;
using AzRanger.Models.Generic;
using AzRanger.Models.MSGraph;
using AzRanger.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzRanger.Checks.Rules
{
    [RuleMeta("AppsWithHighPrives", Scope.O365, MaturityLevel.Mature, "https://portal.azure.com/#blade/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/RegisteredApps")]
    [RuleInfo("Service principals with high priviledged roles assigned", @"A user with ""owner"" rights to the service principal or with the role ""Application administrator"" or ""Cloud application administrator"" can use this configuration to elevate its privileges.", 9, null, null, "Check if the service pricipals needs these roles or if they can be removed.")]

    class AppsWithHighPrives : BaseCheck
    {
        public override CheckResult Audit(Tenant tenant)
        {
            bool passed = true;
            foreach (DirectoryRole role in tenant.AllDirectoryRoles.Values)
            {
                if (DirectoryRoleTemplateID.HighPrivRoles.Any(role.roleTemplateId.Contains))
                {
                    foreach(AzurePrincipal id in role.GetMembers())
                    {
                        if(id.PrincipalType == AzurePrincipalType.ServicePrincipal)
                        {
                            passed = false;
                            this.AddAffectedEntity(tenant.AllServicePrincipals[id.id]);
                        }
                    }
                }
            }

            if (passed)
            {
                return CheckResult.NoFinding;
            }
            return CheckResult.Finding;
        }
    }
}
