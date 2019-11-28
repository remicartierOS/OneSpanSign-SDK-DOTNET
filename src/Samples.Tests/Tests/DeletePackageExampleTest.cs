using NUnit.Framework;
using System;
using Silanis.ESL.SDK;

namespace SDK.Examples
{
    [TestFixture()]
    public class DeletePackageExampleTest
    {
        [Test()]
        public void VerifyResult()
        {
            Assert.Throws<EslServerException>(() =>
            {
                DeletePackageExample example = new DeletePackageExample();
                example.Run();

                Assert.IsNull(example.RetrievedPackage);
            });
        }
    }
}

