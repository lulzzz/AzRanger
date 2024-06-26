﻿using AzRanger.Models;
using AzRanger.Models.AzMgmt;
using System;

namespace AzRanger.Checks.Rules
{
    internal class AzActLogAlertDeletePublicIPAddressRule : BaseCheck
    {
        public override CheckResult Audit(Tenant tenant)
        {
            bool passed = true;

            foreach (Subscription sub in tenant.Subscriptions.Values)
            {
                bool wantedAllertRuleExist = false;
                foreach (ActivityLogAlert alert in sub.Resources.ActivityLogAlerts)
                {
                    if (alert.location == "Global" && alert.properties.enabled == true)
                    {
                        bool scopeIsEntireSubscription = false;
                        foreach (String scope in alert.properties.scopes)
                        {
                            if (scope == sub.id)
                            {
                                scopeIsEntireSubscription = true;
                            }
                        }
                        if (scopeIsEntireSubscription)
                        {
                            foreach (ActivityLogAlertAllof allOf in alert.properties.condition.allOf)
                            {
                                if (allOf.field == "operationName" && allOf.equals.ToLower() == "microsoft.network/publicipaddresses/write")
                                {
                                    wantedAllertRuleExist = true;
                                }
                            }
                        }
                    }
                }
                if (wantedAllertRuleExist == false)
                {
                    this.AddAffectedEntity(sub);
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
