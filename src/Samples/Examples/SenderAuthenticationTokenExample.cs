using System;
using Silanis.ESL.SDK;
using Silanis.ESL.SDK.Builder;
using System.IO;

namespace SDK.Examples
{
    public class SenderAuthenticationTokenExample : SDKSample
    {
        public static void Main (string[] args)
        {
            new SenderAuthenticationTokenExample().Run();
        }

        public string SenderSessionId { get; private set; }
        
        private AuthenticationClient AuthenticationClient;

        public SenderAuthenticationTokenExample()
        {
            this.AuthenticationClient = new AuthenticationClient(webpageUrl);
        }

        override public void Execute()
        {
            DocumentPackage superDuperPackage =
                PackageBuilder.NewPackageNamed(PackageName)
                .DescribedAs("This is a package created using the eSignLive SDK")
                .ExpiresOn(DateTime.Now.AddMonths(1))
                .WithEmailMessage("This message should be delivered to all signers")
                .WithDocument(DocumentBuilder.NewDocumentNamed("First Document")
                              .FromStream(fileStream1, DocumentType.PDF)
                             )
                .Build();

            PackageId packageId = eslClient.CreatePackage(superDuperPackage);

            string senderAuthenticationToken = eslClient.AuthenticationTokenService.CreateSenderAuthenticationToken(packageId);

            SenderSessionId = AuthenticationClient.GetSessionIdForSenderAuthenticationToken(senderAuthenticationToken);
        }

    }
}

