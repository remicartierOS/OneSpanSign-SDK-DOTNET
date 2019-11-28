using NUnit.Framework;
using System;
using Silanis.ESL.SDK;
using System.Collections.Generic;

namespace SDK.Examples
{
    [TestFixture()]
    public class MixingSignatureAndAcceptanceOnOnedocumentExampleTest
    {
        [Test()]
        public void VerifyResult()
        {
            Assert.Throws<EslException>(() =>
            {
                MixingSignatureAndAcceptanceOnOnedocumentExample example = new MixingSignatureAndAcceptanceOnOnedocumentExample();
                example.Run();

                DocumentPackage documentPackage = example.RetrievedPackage;

                List<Signature> signatures = documentPackage.GetDocument("First Document").Signatures;

                Assert.AreEqual(2, signatures.Count);
                Assert.AreEqual(SignatureStyle.FULL_NAME, signatures[0].Style);
                Assert.AreEqual(SignatureStyle.ACCEPTANCE, signatures[1].Style);
            });
        }
    }
}

