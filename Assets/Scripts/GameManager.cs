using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System;
using System.Net;
using static System.Net.WebRequestMethods;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI label;
    public RawImage img;

    private LocationService GPS;
    private float lat, lng;

    void Start()
    {
        GPS = Input.location;

        if (GPS.isEnabledByUser)
        {
            // GPSの測位を開始する
            GPS.Start();

            // 5秒おきにGPS情報を取得する
            InvokeRepeating("GetLocation", 0, 5);
        }
        else
        {
            // GPSにアクセスできない
            label.text = "GPS不許可";
        }

        // まずはダミー位置情報で読み込む
        lat = 35.686275f;
        lng = 139.752835f;
        StartCoroutine(GetMapImage());
    }

    //アプリ終了時に呼ばれるメソッド
    void OnDisable()
    {
        // アプリ終了時にGPSを停止する（電池節約）
        GPS.Stop();
    }

    //位置情報更新用のメソッド
    void GetLocation()
    {
        switch (GPS.status)
        {
            case LocationServiceStatus.Failed:
                // GPS取得失敗
                label.text = "取得失敗";
                CancelInvoke("GetLocation");
                break;

            case LocationServiceStatus.Initializing:
                // まだ準備中
                label.text = "取得中…";
                break;

            case LocationServiceStatus.Running:
                // GPS利用可能
                var data = GPS.lastData;
                if (lat != data.latitude || lng != data.longitude)
                {
                    // 前回と位置が違うなら地図更新
                    lat = data.latitude;
                    lng = data.longitude;
                    StartCoroutine(GetMapImage());
                }
                break;
        }
    }

    //地図を取得するメソッド
    IEnumerator GetMapImage()
    {
        label.text = string.Format("w:{0:f6}\nh:{1:f6}", lat, lng);

        // URLを生成
        //var url = $"https://staticmap.openstreetmap.de";
        //url += string.Format("?center={0},{1}", lat, lng);
        //url += "&zoom=14&size=400x300&scale=2&maptype=roadmap&sensor=true";
        //url += string.Format("&markers={0},{1}", lat, lng);
        //var url = "http://staticmap.openstreetmap.de?center=40.714728,-73.998672&zoom=14&size=865x512&maptype=mapnik";
        var url = "http://a.tile.openstreetmap.org/2/2/2.png";
        Debug.Log(url);

        //API
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        //urlに接続してデータが帰ってくるまで待機状態にする。
        yield return www.SendWebRequest();
        //エラー確認
        switch (www.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("リクエスト中");
                break;
            case UnityWebRequest.Result.Success:
                //成功
                Debug.Log(www.downloadHandler.text);
                // 画像として読み込む
                img.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                break;
        }
    }
}

