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
			var a = new Parameter("a");
			var b = new Parameter("b");
			var c = new Parameter("c");
			var signature = new Signature(a, b, c);
			var invocation = signature.CreateInvocation("value1,123,Sunday");

			this
				.Given(s => Task.FromResult(0))
				.Then(s => invocation.Arguments["a"].Value.Should().Be("value1", null))
				.Then(s => invocation.Arguments["b"].AsInt32().Should().Be(123, null))
				.Then(s => invocation.Arguments["c"].AsEnum<DayOfWeek>().Should().Be(DayOfWeek.Sunday, null))
				.BDDfy();
		}
	}
}