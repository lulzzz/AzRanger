﻿using AzRanger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzRanger.Checks.Rules
{
    [RuleMeta("OfficeFormsPhishingProtection", ScopeEnum.AAD, MaturityLevel.Mature, "https://admin.microsoft.com/#/Settings/Services/:/Settings/L1/OfficeForms")]    
    class OfficeFormsPhishingProtection : BaseCheck
    {
        public override CheckResult Audit(Tenant tenant)
        {
            if(tenant.TenantSettings.AdminCenterSettings.OfficeFormsSettings.InOrgFormsPhishingScanEnabled == true)
            {
                return CheckResult.NoFinding;
            }
            return CheckResult.Finding;
        }
    }
}
