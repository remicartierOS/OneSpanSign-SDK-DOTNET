using System;
using System.IO;
using Silanis.ESL.SDK;
using Silanis.ESL.SDK.Builder;
/**
 * Created by whou on 08/12/14.
 */
namespace SDK.Examples
{
    public class ProxyConfigurationExample : SDKSample
    {
        private bool allowAllSSLCertificates = true;

        private ProxyConfiguration httpProxyConfiguration, httpProxyWithCredentialsConfiguration;

        public EslClient eslClient, eslClientWithHttpProxy, eslClientWithHttpProxyHasCredentials;
        public PackageId packageId1, packageId2;
        public DocumentPackage package1, package2;

        public static void Main (string[] args)
        {
            new ProxyConfigurationExample(Props.GetInstance()).Run();
        }

        public ProxyConfigurationExample( Props props ) : this(props.Get("api.key"), props.Get("api.url")) {
        }

        public ProxyConfigurationExample(string apiKey, string apiUrl) : base(apiKey, apiUrl)
        {
            httpProxyConfiguration = ProxyConfigurationBuilder.NewProxyConfiguration()
                .WithHttpHost(proxyHost)
                    .WithHttpPort(proxyPort)
                    .Build();

            httpProxyWithCredentialsConfiguration = ProxyConfigurationBuilder.NewProxyConfiguration()
                .WithHttpHost(proxyWithCredentialsHost)
                    .WithHttpPort(proxyWithCredentialsPort)
                    .WithCredentials(proxyUserName, proxyPassword)
                    .Build();

            eslClientWithHttpProxy = new EslClient(apiKey, apiUrl, allowAllSSLCertificates, httpProxyConfiguration);
            eslClientWithHttpProxyHasCredentials = new EslClient(apiKey, apiUrl, allowAllSSLCertificates, httpProxyWithCredentialsConfiguration);
        }

        
        override public void Execute()
        {
            package1 = PackageBuilder.NewPackageNamed("ProxyConfigurationExample1: " + DateTime.Now)
                .DescribedAs("This is a new package1")
                    .WithSigner(SignerBuilder.NewSignerWithEmail(email1)
                                .WithCustomId("signer1")
                                .WithFirstName("John")
                                .WithLastName("Smith"))
                    .WithDocument(DocumentBuilder.NewDocumentNamed("My Document1")
                                  .FromStream(fileStream1, DocumentType.PDF)
                                  .WithSignature(SignatureBuilder.SignatureFor(email1)
                                   .OnPage(0)
                                   .AtPosition(100, 100)))
                    .Build();

            packageId1 = eslClientWithHttpProxy.CreateAndSendPackage(package1);
            eslClientWithHttpProxy.SignDocuments (packageId1);
            eslClientWithHttpProxy.SignDocuments (packageId1, "signer1");

            package2 = PackageBuilder.NewPackageNamed("ProxyConfigurationExample2: " + DateTime.Now)
                .DescribedAs("This is a new package2")
                    .WithSigner(SignerBuilder.NewSignerWithEmail(email1)
                                .WithCustomId ("signer2")
                                .WithFirstName("John")
                                .WithLastName("Smith"))
                    .WithDocument(DocumentBuilder.NewDocumentNamed("My Document2")
                                  .FromStream(fileStream2, DocumentType.PDF)
                                  .WithSignature(SignatureBuilder.SignatureFor(email1)
                                   .OnPage(0)
                                   .AtPosition(100, 100)))
                    .Build();

            packageId2 = eslClientWithHttpProxyHasCredentials.CreateAndSendPackage(package2);
            eslClientWithHttpProxyHasCredentials.SignDocuments (packageId2);
            eslClientWithHttpProxyHasCredentials.SignDocuments (packageId2, "signer2");

        }
    }
}