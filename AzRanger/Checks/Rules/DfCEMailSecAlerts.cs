﻿using AzRanger.Models;
using AzRanger.Models.AzMgmt;
using System;
using System.Linq;

namespace AzRanger.Checks.Rules
{
    class DfCEMailSecAlerts : BaseCheck
    {
        public override CheckResult Audit(Tenant tenant)
        {
            bool passed = true;

            foreach (Subscription sub in tenant.Subscriptions.Values)
            {
                if (sub.SecurityContact[0].properties.notificationsByRole.state.Equals("On") && sub.SecurityContact[0].properties.alertNotifications.state.Equals("On"))
                {
                    if (!sub.SecurityContact[0].properties.notificationsByRole.roles.Contains("Owner"))
                    {
                        AddAffectedEntity(sub);
                        passed = false;
                    }
                }
                else
                {
                    AddAffectedEntity(sub);
                    passed = false;
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
