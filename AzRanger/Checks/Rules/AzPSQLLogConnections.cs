﻿using AzRanger.Models;
using AzRanger.Models.AzMgmt;

namespace AzRanger.Checks.Rules
{
    internal class AzPSQLLogConnections : BaseCheck
    {
        public override CheckResult Audit(Tenant tenant)
        {
            bool passed = true;

            foreach (Subscription sub in tenant.Subscriptions.Values)
            {
                if (sub.Resources.PostgreSQLs == null)
                {
                    SetReason("You do not have SQLServer or the user cannot see them.");
                    return CheckResult.NotApplicable;
                }
                foreach (PostgreSQLFlexibleServers server in sub.Resources.PostgreSQLs)
                {
                    foreach (PostgreSQLFlexibleServersParameters param in server.Paramters)
                    {
                        if (param.name == "log_connections")
                        {
                            if (param.properties.value == "off")
                            {
                                passed = false;
                                this.AddAffectedEntity(server);
                                break;
                            }
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
