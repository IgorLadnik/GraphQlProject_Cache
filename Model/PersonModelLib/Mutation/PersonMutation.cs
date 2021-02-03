using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.Types;
using PersonModelLib.Data;
using PersonModelLib.Models;
using PersonModelLib.Type;
using RepoInterfaceLib;

namespace PersonModelLib.Mutation
{
    public class PersonMutation : ObjectGraphType
    {
        public PersonMutation(IRepo<GraphQLDbContext> repo)
        {
            FieldAsync<PersonOutputType>("createPersons",
                arguments: new QueryArguments(new QueryArgument<ListGraphType<PersonInputType>> { Name = "personsInput" }),
                resolve: async context =>
                {
                    // Begin transaction
                    await repo.BeginTransactionAsync();

                    var persons = await repo.FetchInTransactionAsync(dbContext => dbContext.Persons.ToList());
                    List<Person> lstNew = new();
                    List<Person> lstExisting = new();

                    foreach (var personInput in context.GetArgument<Dictionary<string, object>[]>("personsInput"))
                    {
                        Person person = new();

                        object v;
                        foreach (var prop in personInput)
                        {
                            switch (prop.Key)
                            {
                                case "affiliations":
                                case "relations":
                                    break;

                                default:
                                    v = prop.Value;
                                    switch (prop.Key)
                                    {
                                        case "givenName": person.GivenName = (string)v; break;
                                        case "surname": person.Surname = (string)v; break;
                                        case "address": person.Address = (string)v; break;
                                        case "phone": person.Phone = (string)v; break;
                                        case "email": person.Email = ((string)v).ToLower(); break;
                                        case "born": person.Born = (int)v; break;
                                    }
                                    break;
                            }
                        }

                        var existiningPerson = persons.Where(p => p.Email == person.Email).FirstOrDefault();
                        if (existiningPerson == null)
                            lstNew.Add(person);
                        else
                            lstExisting.Add(existiningPerson);
                    }

                    var existingIds = lstExisting?.Select(p => p.Id)?.ToList();

                    var affiliationsToDelete = await repo.FetchInTransactionAsync(dbContext => dbContext.Affiliations.Where(a => existingIds.Contains(a.PersonId))?.ToList());
                    var relationsToDelete = await repo.FetchInTransactionAsync(dbContext => dbContext.Relations.Where(r => existingIds.Contains(r.P1Id)).ToList());

                    // Save
                    var result = await repo.SaveAsync(dbContext =>
                    {
                        dbContext.Affiliations?.RemoveRange(affiliationsToDelete);
                        dbContext.Relations?.RemoveRange(relationsToDelete);
                        dbContext.Persons?.UpdateRange(lstExisting);
                        dbContext.Persons?.AddRange(lstNew);
                    });

                    persons = await repo.FetchInTransactionAsync(dbContext => dbContext.Persons.ToList());

                    List<Affiliation> affiliations = new();
                    List<Relation> relations = new();

                    foreach (var personInput in context.GetArgument<Dictionary<string, object>[]>("personsInput"))
                    {
                        int personId = -1000;
                        Affiliation affiliation = new();
                        Relation relation = new();

                        object v;
                        foreach (var prop in personInput)
                        {
                            switch (prop.Key)
                            {
                                case "affiliations":
                                    foreach (Dictionary<string, object> dctProp in (IList<object>)prop.Value)
                                    {
                                        foreach (var key in dctProp.Keys)
                                        {
                                            v = dctProp[key];
                                            switch (key)
                                            {
                                                case "since": affiliation.Since = (int)v; break;
                                                case "organizationId": affiliation.OrganizationId = (int)v; break;
                                                case "roleId": affiliation.RoleId = (int)v; break;
                                            }
                                        }
                                    }
                                    break;

                                case "relations":
                                    foreach (Dictionary<string, object> dctProp in (IList<object>)prop.Value)
                                    {
                                        foreach (var key in dctProp.Keys)
                                        {
                                            v = dctProp[key];
                                            switch (key)
                                            {
                                                case "since": relation.Since = (int)v; break;
                                                case "kind": relation.Kind = (string)v; break;
                                                case "notes": relation.Notes = (string)v; break;
                                                case "p2Id": relation.P2Id = (int)v; break;
                                            }
                                        }
                                    }
                                    break;

                                default:
                                    v = prop.Value;
                                    switch (prop.Key)
                                    {                                       
                                        case "email": 
                                            personId = persons.Where(p => p.Email == (string)v).Select(p => p.Id).First();
                                            break;                                       
                                    }
                                    break;
                            }
                        }

                        affiliation.PersonId = relation.P1Id = personId;

                        affiliations.Add(affiliation);
                        relations.Add(relation);
                    }

                    // Save
                    result = await repo.SaveAsync(dbContext =>
                    {
                        dbContext.Affiliations.AddRange(affiliations);
                        dbContext.Relations.AddRange(relations);
                    });

                    // Commit / Rollback 
                    if (result.IsOK)
                        result = await repo.CommitAsync();

                    return result;
                });
        }
    }
}
