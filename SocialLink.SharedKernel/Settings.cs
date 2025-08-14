using Azure.Core;
using Azure.Identity;

namespace SocialLink.SharedKernel;

public static class Settings
{
	// TokenCredential

	private static readonly Lock TokenCredentialLocker = new();

	public static TokenCredential TokenCredential
	{
		get
		{
			if (_tokenCredential == null)
				lock (TokenCredentialLocker)
					_tokenCredential ??= new DefaultAzureCredential();

			return _tokenCredential;
		}
	}

	private static TokenCredential _tokenCredential = null;

	public static string AzureStorageName => "devsocialmediastorage";

	public static string GetBlobStorageUrl(string storageName, string path = "") => $"https://{storageName}.blob.core.windows.net/{path}";
}