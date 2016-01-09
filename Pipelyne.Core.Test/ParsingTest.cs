namespace Pipelyne.Core.Test
{
	using System;
	using System.Threading.Tasks;
	using FluentAssertions;
	using global::Pipelyne.Core.Parsing;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestStack.BDDfy;

	[TestClass]
	public class ParsingTest
	{
		[TestMethod]
		public void ParsingWorks()
		{
			var a = new Argument("value1", new Parameter("a"));
			var b = new Argument("123", new Parameter("b"));
			var c = new Argument("Sunday", new Parameter("c"));

			this
				.Given(s => Task.FromResult(0))
				.Then(s => a.Value.Should().Be("value1", null))
				.Then(s => b.AsInt32().Should().Be(123, null))
				.Then(s => c.AsEnum<DayOfWeek>().Should().Be(DayOfWeek.Sunday, null))
				.BDDfy();
		}
	}
}