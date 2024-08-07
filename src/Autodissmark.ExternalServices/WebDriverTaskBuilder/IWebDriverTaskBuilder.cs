﻿namespace Autodissmark.ExternalServices.WebDriverTaskBuilder;

public interface IWebDriverTaskBuilder
{
    Task SetLink(string link, int defaultWaitInSeconds, string? downloadDirectory = null);
    Task Click(string xPath);
    Task Click(string xPath, int afterClickDelay, int times);
    Task ClickIfThere(string xPath);
    Task InputText(string xPath, string text, string? markerXPath = null, int? afterMarkerApearDelay = null);
    Task ClearInput(string xPath);
    Task<string> OutputText(string xPath, int totalApearDelay, string? markerXPath = null, int? afterMarkerApearDelay = null);
    Task WaitForDownladingFile(string downloadDirectory, string searchPattern = "*.*");
    byte[] OutputFirstByPatternDownladedFile(string downloadDirectory, string searchPattern = "*.*");
    void Close();
    
}