using System;

namespace Silanis.ESL.SDK.Builder
{
	public class AuthenticationBuilder
	{
		public AuthenticationBuilder() {}

		public virtual Authentication Build()
		{
			Authentication result = new Authentication(AuthenticationMethod.EMAIL);
			return result;
		}
	}
}