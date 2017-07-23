using System;
using System.IO;
using System.Collections;
//using HTTP;
using UnityEngine;
using UnitySpeechToText.Utilities;
using UnityEngine.Networking;

namespace UnitySpeechToText.Services
{
    /// <summary>
    /// Wit.ai non-streaming speech-to-text SDK.
    /// </summary>
    public class WitAiNonStreamingSpeechToTextService : NonStreamingSpeechToTextService
    {
        /// <summary>
        /// Component used to manage temporary audio files
        /// </summary>
        TempAudioFileSavingComponent m_TempAudioComponent = new TempAudioFileSavingComponent("WitAiNonStreamingAudio");
        /// <summary>
        /// Store for APIAccessToken property
        /// </summary>
        [SerializeField]
        string m_APIAccessToken;

        /// <summary>
        /// Access token for API calls
        /// </summary>
        public string APIAccessToken { set { m_APIAccessToken = value; } }

        /// <summary>
        /// Function that is called when the MonoBehaviour will be destroyed.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_TempAudioComponent.ClearTempAudioFiles();
        }

        /// <summary>
        /// Translates speech to text by making a request to the speech-to-text API.
        /// </summary>
        protected override IEnumerator TranslateRecordingToText()
        {
            m_TempAudioComponent.ClearTempAudioFiles();

            // Save recorded audio to a WAV file.
            string recordedAudioFilePath = SavWav.Save(m_TempAudioComponent.TempAudioRelativePath(), AudioRecordingManager.Instance.RecordedAudio);

			//WWW request

			string _url = Constants.WitAiSpeechToTextBaseURL + "?" +
				Constants.WitAiVersionParameterName + "=" + DateTime.Now.ToString(Constants.WitAiVersionDateFormat);
			UnityWebRequest www = new UnityWebRequest(_url, UnityWebRequest.kHttpVerbPOST);


			byte[] bytes = File.ReadAllBytes(recordedAudioFilePath);
			UploadHandlerRaw uH = new UploadHandlerRaw(bytes);
			uH.contentType = "application/json";
			www.uploadHandler = uH;
			www.downloadHandler = new DownloadHandlerBuffer();
			www.SetRequestHeader("Content-Type", "application/json");
			www.SetRequestHeader("Authorization", "Bearer " + m_APIAccessToken);

			SmartLogger.Log(DebugFlags.GoogleNonStreamingSpeechToText, "sent request");
			float startTime = Time.time;
			yield return www.Send();

			while (!www.isDone)
			{
				yield return null;
			}

			if (www.isError)
			{
				SmartLogger.Log(DebugFlags.GoogleNonStreamingSpeechToText, www.error);
			}
			else
			{
				SmartLogger.Log(DebugFlags.GoogleNonStreamingSpeechToText, "Form upload complete!");
			}
			SmartLogger.Log(DebugFlags.WitAINonStreamingSpeechToText, "response time: " + (Time.time - startTime));
			// Grab the response JSON once the request is done and parse it.
			var responseJSON = new JSONObject(www.downloadHandler.text, int.MaxValue);

			//END WWW

            // Construct a request with the WAV file and send it.
            //var request = new Request("POST", Constants.WitAiSpeechToTextBaseURL + "?" +
            //    Constants.WitAiVersionParameterName + "=" + DateTime.Now.ToString(Constants.WitAiVersionDateFormat));
            //request.headers.Add("Authorization", "Bearer " + m_APIAccessToken);
            //request.headers.Add("Content-Type", "audio/wav");
            //request.Bytes = File.ReadAllBytes(recordedAudioFilePath);
            //SmartLogger.Log(DebugFlags.WitAINonStreamingSpeechToText, "Sending request");
            //request.Send();

            //while (!request.isDone)
            //{
            //    yield return null;
            //}
            //SmartLogger.Log(DebugFlags.WitAINonStreamingSpeechToText, "response time: " + (Time.time - startTime));

            // Finally, grab the response JSON once the request is done.
            //var responseJSON = new JSONObject(request.response.Text, int.MaxValue);
            SmartLogger.Log(DebugFlags.WitAINonStreamingSpeechToText, "Received request result");
            SmartLogger.Log(DebugFlags.WitAINonStreamingSpeechToText, responseJSON.ToString());

            string errorText = WitAiSpeechToTextResponseJSONParser.GetErrorFromResponseJSON(responseJSON);
            if (errorText != null)
            {
                if (m_OnError != null)
                {
                    m_OnError(errorText);
                }
            }

            if (m_OnTextResult != null)
            {
                m_OnTextResult(WitAiSpeechToTextResponseJSONParser.GetTextResultFromResponseJSON(responseJSON));
            }

            m_TempAudioComponent.ClearTempAudioFiles();
        }
    }
}
