using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using BepInEx;
using BepInEx.Unity.IL2CPP.Utils;
using BetterOtherRoles.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace BetterOtherRoles.Eno;

public class PluginUpdater : MonoBehaviour
{
    public const string RepositoryOwner = "EnoPM";
    public const string RepositoryName = "BetterOtherRoles";
    public static PluginUpdater Instance { get; private set; }
    
    public PluginUpdater(IntPtr ptr) : base(ptr) { }
    
    private bool _busy;
    public List<GithubRelease> Releases;

    public void Awake()
    {
        if (Instance) Destroy(Instance);
        Instance = this;
        foreach (var file in Directory.GetFiles(Path.Combine(Paths.PluginPath, "BetterOtherRoles"), "*.old"))
        {
            File.Delete(file);
        }
    }

    private void Update()
    {
        if (Rewired.ReInput.players?.GetPlayer(0)?.GetButtonDown("ActionToggleUpdater") == true)
        {
            UIManager.UpdatePluginPanel?.Toggle();
        }
    }

    private void Start()
    {
        if (_busy) return;
        this.StartCoroutine(CoCheckForUpdate());
    }

    [HideFromIl2Cpp]
    private void SetLoadingText(string text)
    {
        UIManager.UpdatePluginPanel?.SetProgressInfosText(text);
    }

    [HideFromIl2Cpp]
    private void SetLoadingProgression(float progression)
    {
        UIManager.UpdatePluginPanel?.SetDownloadProgression(progression);
    }
    
    [HideFromIl2Cpp]
    private void SetLoadingActive(bool active)
    {
        UIManager.UpdatePluginPanel?.SetProgressBarActive(active);
    }
    
    [HideFromIl2Cpp]
    private void SetLoadingError(string error)
    {
        UIManager.UpdatePluginPanel?.SetProgressInfosText(Helpers.cs(Color.red, error));
        BetterOtherRolesPlugin.Logger.LogError(error);
    }

    [HideFromIl2Cpp]
    public void StartDownloadRelease(GithubRelease release)
    {
        if (_busy) return;
        this.StartCoroutine(CoDownloadRelease(release));
    }

    [HideFromIl2Cpp]
    private IEnumerator CoCheckForUpdate()
    {
        _busy = true;
        SetLoadingText("Checking for updates");
        var www = new UnityWebRequest();
        www.SetMethod(UnityWebRequest.UnityWebRequestMethod.Get);
        www.SetUrl($"https://api.github.com/repos/{RepositoryOwner}/{RepositoryName}/releases");
        www.downloadHandler = new DownloadHandlerBuffer();
        var operation = www.SendWebRequest();

        while (!operation.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        
        if (www.isNetworkError || www.isHttpError)
        {
            SetLoadingError(www.error);
            yield break;
        }

        Releases = JsonSerializer.Deserialize<List<GithubRelease>>(www.downloadHandler.text);
        www.downloadHandler.Dispose();
        www.Dispose();
        Releases.Sort(SortReleases);
        var latestRelease = Releases.FirstOrDefault();
        if (latestRelease != null && latestRelease.Version != BetterOtherRolesPlugin.Version)
        {
            UIManager.UpdatePluginPanel?.RefreshDropdown(latestRelease);
            UIManager.UpdatePluginPanel?.SetActive(true);
        }
        
        _busy = false;
    }

    [HideFromIl2Cpp]
    private IEnumerator CoDownloadRelease(GithubRelease release)
    {
        _busy = true;
        var asset = release.Assets.Find(FilterPluginAsset);
        var www = new UnityWebRequest();
        www.SetMethod(UnityWebRequest.UnityWebRequestMethod.Get);
        www.SetUrl(asset.DownloadUrl);
        www.downloadHandler = new DownloadHandlerBuffer();
        var operation = www.SendWebRequest();
        
        SetLoadingText($"Downloading {BetterOtherRolesPlugin.Name} {release.Tag}...");
        SetLoadingActive(true);
        SetLoadingProgression(0f);

        while (!operation.isDone)
        {
            SetLoadingProgression(www.downloadProgress);
            yield return new WaitForEndOfFrame();
        }
        
        SetLoadingProgression(1f);
        
        if (www.isNetworkError || www.isHttpError)
        {
            SetLoadingError(www.error);
            yield break;
        }
        
        SetLoadingText($"Installing {BetterOtherRolesPlugin.Name} {release.Tag}...");

        var filePath = Path.Combine(Paths.PluginPath, "BetterOtherRoles", asset.Name);

        if (File.Exists(filePath + ".old")) File.Delete(filePath + "old");
        if (File.Exists(filePath)) File.Move(filePath, filePath + ".old");

        var persistTask = File.WriteAllBytesAsync(filePath, www.downloadHandler.data);
        var hasError = false;
        while (!persistTask.IsCompleted)
        {
            if (persistTask.Exception != null)
            {
                hasError = true;
                SetLoadingError(persistTask.Exception.Message);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        
        www.downloadHandler.Dispose();
        www.Dispose();

        if (!hasError)
        {
            SetLoadingText(Helpers.cs(Color.green, "<b>Installation completed! Please restart your game to apply the update</b>"));
        }
        // SetLoadingActive(false);
        _busy = false;
    }

    [HideFromIl2Cpp]
    private static bool FilterLatestRelease(GithubRelease release)
    {
        return release.IsNewer(BetterOtherRolesPlugin.Version) && release.Assets.Any(FilterPluginAsset);
    }
    
    [HideFromIl2Cpp]
    private static bool FilterPluginAsset(GithubAsset asset)
    {
        return asset.Name == "BetterOtherRoles.dll";
    }

    [HideFromIl2Cpp]
    private static int SortReleases(GithubRelease a, GithubRelease b)
    {
        if (a.IsNewer(b.Version)) return -1;
        if (b.IsNewer(a.Version)) return 1;
        return 0;
    }
}

public class GithubRelease
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("tag_name")]
    public string Tag { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("draft")]
    public bool Draft { get; set; }
    
    [JsonPropertyName("prerelease")]
    public bool Prerelease { get; set; }
    
    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }
    
    [JsonPropertyName("published_at")]
    public string PublishedAt { get; set; }
    
    [JsonPropertyName("body")]
    public string Description { get; set; }
    
    [JsonPropertyName("assets")]
    public List<GithubAsset> Assets { get; set; }

    public Version Version => Version.Parse(Tag.Replace("v", string.Empty));

    public bool IsNewer(Version version)
    {
        return Version > version;
    }
}

public class GithubAsset
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("size")]
    public int Size { get; set; }
    
    [JsonPropertyName("browser_download_url")]
    public string DownloadUrl { get; set; }
    
}