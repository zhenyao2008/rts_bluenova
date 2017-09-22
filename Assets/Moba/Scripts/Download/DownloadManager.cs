using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using CSV;
using UnityEngine.Events;

public class DownloadManager : SingleMonoBehaviour<DownloadManager>
{

	public int maxDownloadCount = 5;
	public int totalDownloadSize = 0;
	public int totalDownloadedSize = 0;
	public bool isDownloading = false;
	public UnityAction onDownloadDone;

	protected override void Awake ()
	{
		base.Awake ();
	}

	void Start ()
	{
		DownloadVersionCSV ();
	}

	List<VersionCSV> mVersions;
	int mDownloadingCount = 0;

	void DownloadVersionCSV ()
	{
		StartCoroutine (_StartDownloadCSV ());
	}

	void FiltVersionList ()
	{
		List<VersionCSV> filtedVersionList = new List<VersionCSV> ();
		for (int i = 0; i < mVersions.Count; i++) {
			VersionCSV versionCSV = mVersions [i];
			string path = PathConstant.CLIENT_ASSETBUNDLES_PATH + "/" + versionCSV.FileName;
			if (FileManager.Exists (path)) {
				string hashCode = FileManager.GetFileHash (path);
				if (hashCode.Trim () != versionCSV.HashCode.Trim ()) {
					filtedVersionList.Add (versionCSV);
					totalDownloadSize += versionCSV.FileSize;
				}
			} else {
				filtedVersionList.Add (versionCSV);
				totalDownloadSize += versionCSV.FileSize;
			}
		}
		mVersions = filtedVersionList;
	}

	private IEnumerator _StartDownloadCSV ()
	{
		Debug.Log ("_StartDownloadCSV".AliceblueColor ());
		string URL = PathConstant.SERVER_VERSION_CSV;
		Debug.Log (URL);
		var www = new WWW (URL);
		yield return www;
		if (www.isDone && string.IsNullOrEmpty (www.error)) {
			var stream = new MemoryStream (www.bytes);
			var reader = new StreamReader (stream);
			CsvContext mCsvContext = new CsvContext ();
			IEnumerable<VersionCSV> list = mCsvContext.Read<VersionCSV> (reader);
			mVersions = new List<VersionCSV> (list);
			FiltVersionList ();
			StartCoroutine (_DownloadAssets ());
		} else {
			Debug.Log (www.error);
		}
		www.Dispose ();
		www = null;
	}

	IEnumerator _DownloadAssets ()
	{
		Debug.Log ("_DownloadAssets".AliceblueColor ());
		isDownloading = true;
		while (true) {
			if (mVersions.Count == 0 && mDownloadingCount == 0) {
				Debug.Log ("Download Done!".AliceblueColor ());
				if (onDownloadDone != null)
					onDownloadDone ();
				yield break;
			}
			if (mDownloadingCount < maxDownloadCount && mVersions.Count > 0) {
				GameObject go = new GameObject ("Downloader");
				Downloader downloader = go.AddComponent<Downloader> ();
				mDownloadingCount++;
				downloader.StartDownload (mVersions [0].FileName, () => {
					mDownloadingCount--;
				});
				mVersions.RemoveAt (0);
			}
			yield return null;
		}
	}

}
