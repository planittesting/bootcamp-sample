using System.Collections;
using NUnit.Framework;

namespace Bootcamp.Test.DataProviders;

public class SubscriptionFieldDataProvider : IEnumerable<TestCaseData>
{
    public IEnumerator<TestCaseData> GetEnumerator()
    {
        yield return new TestCaseData("First Name");
        yield return new TestCaseData("Last Name");
        yield return new TestCaseData("Email");
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
