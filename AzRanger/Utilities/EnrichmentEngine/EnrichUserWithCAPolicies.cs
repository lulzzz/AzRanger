﻿using AzRanger.Models;
using AzRanger.Models.Generic;
using AzRanger.Models.MSGraph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzRanger.Utilities.EnrichmentEngine
{
    public static class EnrichUserWithCAPolicies
    {
        public static void Enrich(Tenant tenant)
        {
            foreach (ConditionalAccessPolicy policy in tenant.CAPolicies.Values)
            {
                HashSet<Guid> usersInThePolicy = new HashSet<Guid>();
                if (policy.state.ToLower().Equals("disabled") | policy.state.ToLower().Equals("enabledforreportingbutnotenforced"))
                {
                    continue;
                }

                if (policy.conditions.users.includeUsers.Any() && policy.conditions.users.includeUsers.Length == 1)
                {
                    if (policy.conditions.users.includeUsers[0].ToString().ToLower().Equals("all"))
                    {
                        foreach (User user in tenant.Users.Values)
                        {
                            usersInThePolicy.Add(user.id);
                        }
                    }
                }

                if (policy.conditions.users.excludeUsers.Any())
                {
                    foreach (Object userId in policy.conditions.users.excludeUsers)
                    {
                        usersInThePolicy.Remove(Guid.Parse(userId.ToString()));
                    }
                }

                if (policy.conditions.users.includeRoles.Any())
                {
                    foreach (Object roleId in policy.conditions.users.includeRoles)
                    {
                        foreach (DirectoryRole role in tenant.DirectoryRoles.Values)
                        {
                            if (roleId.ToString().Equals(role.roleTemplateId))
                            {
                                foreach (AzurePrincipal user in role.activeMembers)
                                {
                                    usersInThePolicy.Add(user.id);
                                }
                                foreach (Tuple<AzurePrincipal, AzurePrincipal> user in role.activeMembersScoped)
                                {
                                    usersInThePolicy.Add(user.Item1.id);
                                }
                            }
                        }
                    }
                }
                if (policy.conditions.users.excludeRoles.Any())
                {
                    foreach (Object roleId in policy.conditions.users.excludeRoles)
                    {
                        foreach (DirectoryRole role in tenant.DirectoryRoles.Values)
                        {
                            if (roleId.ToString().Equals(role.roleTemplateId))
                            {
                                foreach (AzurePrincipal user in role.activeMembers)
                                {
                                    usersInThePolicy.Remove(user.id);
                                }
                                foreach (Tuple<AzurePrincipal, AzurePrincipal> user in role.activeMembersScoped)
                                {
                                    usersInThePolicy.Add(user.Item1.id);
                                }
                            }
                        }
                    }
                }
                if (policy.conditions.users.includeGroups.Any())
                {
                    foreach (Object groupId in policy.conditions.users.includeGroups)
                    {
                        Group group = tenant.Groups[Guid.Parse(groupId.ToString())];
                        if (group != null)
                        {
                            foreach (AzurePrincipal principal in group.members)
                            {
                                if (principal.PrincipalType == AzurePrincipalType.User)
                                {
                                    usersInThePolicy.Add(principal.id);
                                }
                            }
                        }
                    }
                }
                if (policy.conditions.users.excludeGroups.Any())
                {
                    foreach (Object groupId in policy.conditions.users.excludeGroups)
                    {
                        Group group = tenant.Groups[Guid.Parse(groupId.ToString())];
                        if (group != null)
                        {
                            foreach (AzurePrincipal principal in group.members)
                            {
                                if (principal.PrincipalType == AzurePrincipalType.User)
                                {
                                    usersInThePolicy.Remove(principal.id);
                                }
                            }
                        }
                    }
                }
                foreach(Guid userId in usersInThePolicy)
                {
                    if (tenant.Users.ContainsKey(userId))
                    {
                        User user = tenant.Users[userId];
                        if (user != null)
                        {
                            if (user.assignedCAPolicies == null)
                            {
                                user.assignedCAPolicies = new List<Guid>();
                            }
                            user.assignedCAPolicies.Add(Guid.Parse(policy.id));
                        }
                    }
                }
            }
        }
    }
}
