using System;
using System.Collections.Generic;
using System.Linq;
using Should.Fluent;
using Specs.TestEntities;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Specs.TablesAndAssist
{
    [Binding]
    public class TableSteps
    {
        private const string SEARCH_RESULT_KEY = "Result";

        [Given("I have the following persons")]
        public void IHaveTheFollowingPersons(Table personsTable)
        {
            List<Person> persons = personsTable.Rows
                .Select(row =>
                        new Person
                            {
                                Name = row["Name"],
                                BirthDate = DateTime.Parse(row["Birth date"]),
                                Style = (Style) Enum.Parse(typeof (Style), row["Style"])
                            }).ToList();

            ScenarioContext.Current.Set(persons);
        }

        [When("I search for (.*)")]
        public void ISearchFor(string nameToSearchFor)
        {
            var list = ScenarioContext.Current.Get<List<Person>>();

            List<Person> result = list.Where(x => x.Name == nameToSearchFor).ToList();

            ScenarioContext.Current.Set(result, SEARCH_RESULT_KEY);
        }

        [Then("the following person should be returned")]
        public void ThenTheResultShouldBeReturned(Table resultTable)
        {
            var searchResult = ScenarioContext.Current.Get<List<Person>>(SEARCH_RESULT_KEY);

            resultTable.Rows.Count.Should().Equal(1);
            resultTable.Rows[0]["Name"].Should().Equal(searchResult[0].Name);
        }


        [Given("I have the following persons using assist")]
        public void IHaveTheFollowingPersonsAssist(Table personsTable)
        {
            List<Person> persons = personsTable.CreateSet<Person>().ToList();
            ScenarioContext.Current.Set(persons);
        }

        [Then("the following person should be returned using assist")]
        public void ThenTheResultShouldBeReturnedAssist(Table resultTable)
        {
            Person person = resultTable.CreateSet<Person>().ToList()[0];

            var searchResult = ScenarioContext.Current.Get<List<Person>>(SEARCH_RESULT_KEY);

            searchResult.Should().Contain.Item(person);
        }


        [When(@"I fill out the form like this")]
        public void FillOutTheForm(Table formData)
        {
            var person = formData.CreateInstance<Person>();
            var personList = new List<Person> {person};
            ScenarioContext.Current.Set(personList, SEARCH_RESULT_KEY);
        }

        [Given(@"this wide table")]
        public void GivenThisWideTable(Table table)
        {
            Console.WriteLine("TESTING: Yeah, what about the wide table? {0}", table.ToString());
        }
    }
}