using NUnit.Framework;
using System;
using Silanis.ESL.SDK.Internal;

namespace SDK.Examples
{
    [TestFixture()]
    public class SigningRedirectForSignerExampleTest
    {
        [Test()]
        public void VerifyResult()
        {
            SigningRedirectForSignerExample example = new SigningRedirectForSignerExample();
            example.Run();

            Assert.IsNotEmpty(example.GeneratedLinkToSigningForSigner);
        }
    }
}

