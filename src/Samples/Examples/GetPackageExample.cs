using System;
using System.IO;
using Silanis.ESL.SDK;
using Silanis.ESL.SDK.Builder;

namespace SDK.Examples
{
	public class GetPackageExample : SDKSample
	{
        public static void Main (string[] args)
        {
            new GetPackageExample().Run();
        }

        override public void Execute()
        {
            DocumentPackage package = PackageBuilder.NewPackageNamed (PackageName)
					.DescribedAs ("This is a new package")
					.WithSigner(SignerBuilder.NewSignerWithEmail(email1)
					            .WithFirstName("John")
					            .WithLastName("Smith")
					            .WithCompany ("Acme Inc")
					            .WithTitle ("Managing Director"))
					.WithDocument(DocumentBuilder.NewDocumentNamed("My Document")
                                  .FromStream(fileStream1, DocumentType.PDF)
					              .WithSignature(SignatureBuilder.SignatureFor(email1)
					              		.OnPage(0)
					               		.AtPosition(500, 100)
					               		.WithField(FieldBuilder.SignatureDate()
					           				.OnPage(0)
					           				.AtPosition(500, 200))
					               		.WithField(FieldBuilder.SignerName ()
					           				.OnPage(0)
					           				.AtPosition(500, 300))
							         	.WithField(FieldBuilder.SignerTitle()
							           		.OnPage(0)
							           		.AtPosition(500, 400))
					               		.WithField (FieldBuilder.SignerCompany()
					            			.OnPage (0)
					            			.AtPosition (500, 500))))
					.Build ();

			PackageId id = eslClient.CreatePackage (package);
			eslClient.SendPackage(id);

			DocumentPackage retrievedPackage = eslClient.GetPackage (id);
            Console.WriteLine("Document retrieved = " + retrievedPackage.Id);
		}
	}
}