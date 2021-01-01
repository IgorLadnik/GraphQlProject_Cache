using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using GraphQlHelperLib;
using GraphQlProject.Data;
using GraphQlProject.Models;
using GraphQlProject.Type;

namespace GraphQlProject.Mutation
{
    public class PersonMutation : ObjectGraphType
    {
        public PersonMutation(DbProvider<GraphQLDbContext> dbProvider)
        {
            Field<PersonOutputType>("createPersons",
                arguments: new QueryArguments(new QueryArgument<ListGraphType<PersonInputType>> { Name = "personsInput" }),
                resolve: context =>
                {
                    List<Person> persons = new();
                    List<Affiliation> affiliations = new();
                    List<Relation> relations = new();

                    foreach (var personInput in context.GetArgument<Dictionary<string, object>[]>("personsInput"))
                    {
                        Person person = new();
                        Affiliation affiliation = new();
                        Relation relation = new();

                        object v;
                        foreach (var item2 in personInput)
                        {
                            var prop = (KeyValuePair<string, object>)item2;
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
                                                case "id": affiliation.Id = (int)v; break;
                                                //case "strId": affiliation.StrId = (string)v; break;
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
                                                case "id": relation.Id = (int)v; break;
                                                //case "strId": relation.StrId = (string)v; break;
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
                                        case "id": person.Id = (int)v; break;
                                        //case "strId": person.StrId = (string)v; break;
                                        case "givenName": person.GivenName = (string)v; break;
                                        case "surname": person.Surname = (string)v; break;
                                        case "address": person.Address = (string)v; break;
                                        case "phone": person.Phone = (string)v; break;
                                        case "email": person.Email = (string)v; break;
                                        case "born": person.Born = (int)v; break;
                                    }
                                    break;
                            }
                        }

                        affiliation.PersonId = relation.P1Id = person.Id;

                        persons.Add(person);
                        affiliations.Add(affiliation);
                        relations.Add(relation);
                    }

                    var mutationResponse = dbProvider.Save(dbContext =>
                        {
                            dbContext.Persons.AddRange(persons);
                            dbContext.Affiliations.AddRange(affiliations);
                            dbContext.Relations.AddRange(relations);
                        });

                    return mutationResponse;
                });
        }
    }
}

/*
mutation RootMutation {
  personMutation {
    createPersons(
      personsInput: [{
          id: 100
          givenName: "Vasya"
          surname: "Pupkin"
          born: 1990
          phone: "111-222-333"
          email: "vpupkin@ua.com"
          address: "21, Torn Street"
          
          affiliations: [{
         	id: 100
            since: 2000
            organizationId: 4
            roleId: 1
          }]
      		
          relations: [{
            id: 100
            since: 2017
            kind: "friend"
            notes: "*!"
            p2Id: 1
          }]					   
  		}
      ]
    ) {
      status
      message
    }
  }
}
*/
