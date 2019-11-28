using System;
using System.IO;
using Silanis.ESL.SDK;
using Silanis.ESL.SDK.Builder;

namespace SDK.Examples
{
    public class MixingSignatureAndAcceptanceOnOnedocumentExample : SDKSample
    {
        public static void Main(string[] args)
        {
            new MixingSignatureAndAcceptanceOnOnedocumentExample().Run();
        }

        public readonly string DOCUMENT_NAME = "First Document";

        override public void Execute()
        {
            DocumentPackage superDuperPackage = PackageBuilder.NewPackageNamed(PackageName)
                .WithSettings(DocumentPackageSettingsBuilder.NewDocumentPackageSettings().WithInPerson())
                    .WithSigner(SignerBuilder.NewSignerWithEmail(email1)
                                .WithFirstName("John1")
                                .WithLastName("Smith1"))
                    .WithSigner(SignerBuilder.NewSignerWithEmail(email2)
                                .WithFirstName("John2")
                                .WithLastName("Smith2"))
                    .WithDocument(DocumentBuilder.NewDocumentNamed(DOCUMENT_NAME)
                                  .FromStream(fileStream1, DocumentType.PDF)
                                  .WithSignature(SignatureBuilder.SignatureFor(email1)
                                   .OnPage(0)
                                   .AtPosition(100, 100))
                                  .WithSignature(SignatureBuilder.AcceptanceFor(email2)))
                    .Build();

            packageId = eslClient.CreatePackage(superDuperPackage);
            eslClient.SendPackage(packageId);
            retrievedPackage = eslClient.GetPackage(packageId);
        }
    }
}

