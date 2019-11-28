using System;
using System.IO;
using Silanis.ESL.SDK;
using Silanis.ESL.SDK.Builder;
using System.Collections.Generic;

namespace SDK.Examples
{
	public class GetPackageListExample : SDKSample
	{
        public static void Main (string[] args)
        {
            new GetPackageExample().Run();
        }

        public GetPackageListExample( Props props ) : this(props.Get("api.key"), props.Get("api.url")) {
        }

        public GetPackageListExample( String apiKey, String apiUrl ) : base( apiKey, apiUrl ) {
        }

        override public void Execute()
        {
			//Get the packages that have status COMPLETED, starting from the most recent package and getting 20 packages per page
			Page<DocumentPackage> packages = eslClient.PackageService.GetPackages (DocumentPackageStatus.COMPLETED, new PageRequest(1, 20));

			while (packages.HasNextPage())
			{
				packages = eslClient.PackageService.GetPackages (DocumentPackageStatus.COMPLETED, packages.NextRequest);
			}
		}
	}
}