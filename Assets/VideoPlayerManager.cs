using System.Collections;
using System.IO;
using UnityEngine.Video;
using UnityEngine;


public class VideoPlayerManager : MonoBehaviour 
{
	[SerializeField]
	private VideoPlayer player1;
	[SerializeField]
	private VideoPlayer player2;
	[SerializeField]
	private MeshFilter fitRefereceQuad;
	
	private Vector3 fitQuadScale;
	private Vector3 floodQuadScale;
	private string videoPath;
	private string pathFolderName = "MultiDisplayVideos";
	private string[] videoList;
	private bool singleVideoMode;

	private VideoPlayer currentPlayer;
	private VideoPlayer queuedPlayer;

	// Use this for initialization
	void Start () 
	{
		fitQuadScale = fitRefereceQuad.transform.localScale;
		floodQuadScale = player1.transform.localScale;
		player1.transform.localScale = Vector3.zero;
		player2.transform.localScale = Vector3.zero;
		fitRefereceQuad.transform.localScale = Vector3.zero;
		
		// Subscribe to the LoopPointReached event. Which lets us know when a video has ended.
		player1.loopPointReached += OnLoopPointReached;
		player2.loopPointReached += OnLoopPointReached;

		currentPlayer = player1;
		queuedPlayer = player2;
		currentPlayer.isLooping = false;
		queuedPlayer.isLooping = false;
		currentPlayer.playOnAwake = false;
		queuedPlayer.playOnAwake = false;

		LoadVideos();
	}

	private void OnLoopPointReached(VideoPlayer source)
	{
		NextVideo();
		StartCoroutine(Play());
	}

	private IEnumerator Play()
	{
		Vector3 targetScale = (currentPlayer.url.ToLower().Contains("_fit")) ? fitQuadScale : floodQuadScale;
		queuedPlayer.transform.Scale(Vector3.zero, .5f);
		currentPlayer.transform.Scale(targetScale, .5f);
		while(!currentPlayer.isPrepared)
		{
			Debug.Log("Awaiting preparedness");
			yield return new WaitForEndOfFrame();
		}
		Debug.Log("Playing");
		currentPlayer.Play();
	}

	private void LoadVideos()
	{
		videoPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\" + pathFolderName;
		if (!Directory.Exists(videoPath))
		{
			Directory.CreateDirectory(videoPath);
			Debug.Log("No video path found. Created: " + videoPath);
			return;
		}
		videoList = Directory.GetFiles(videoPath);
		System.Array.Sort(videoList, (x,y) => System.String.Compare(x, y));

		if (videoList.Length == 0)
		{
			Debug.Log("No videos found. Add files to " + videoPath);
			return;
		}

		if (videoList.Length == 1)
		{
			currentPlayer.url = videoList[0];
			currentPlayer.Prepare();
			singleVideoMode = true;
		}
		else
		{
			singleVideoMode = false;
			currentPlayer.url = videoList[0];
			currentPlayer.Prepare();
			queuedPlayer.url = videoList[1];
			queuedPlayer.Prepare();
			videoIndex = 2;
		}
		StartCoroutine(Play());
	}

	int videoIndex = 0;
	private void NextVideo()
	{
		if (!singleVideoMode)
		{
			videoIndex++;
			if (videoIndex > videoList.Length - 1)
			{
				videoIndex = 0;
			}

			if (currentPlayer.GetInstanceID() == player1.GetInstanceID())
			{
				currentPlayer = player2;
				queuedPlayer = player1;
			}
			else
			{
				currentPlayer = player1;
				queuedPlayer = player2;
			}

			queuedPlayer.url = videoList[videoIndex];
			queuedPlayer.Prepare();
			queuedPlayer.frame = 1;
			Debug.Log("VideoIndex = " + videoIndex);
		}
	}
}