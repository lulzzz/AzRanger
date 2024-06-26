﻿using AzRanger.Models;
using AzRanger.Models.AzMgmt;

namespace AzRanger.Checks.Rules
{
    internal class AzAutoprovisioning : BaseCheck
    {
        public override CheckResult Audit(Tenant tenant)
        {
            bool passed = true;
            foreach (Subscription sub in tenant.Subscriptions.Values)
            {
                foreach (AutoProvisioningSettings settings in sub.AutoProvisioningSettings)
                {
                    if (settings.name == "default")
                    {
                        if (settings.properties.autoProvision == "Off")
                        {
                            passed = false;
                            this.AddAffectedEntity(sub);
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
