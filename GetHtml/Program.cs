// See https://aka.ms/new-console-template for more information
using System;

Console.WriteLine("Hello, World!");

const string url = "https://news.yahoo.co.jp/topics/it";
const string filePath = "C:\\work\\repos\\matome_phase1\\GetHtml\\log\\targetHtml.html";
using var client = new HttpClient();
Task<string> tsk = client.GetStringAsync(url);
string task_result = tsk.Result; //Task.Resultを使用 同期処理用
File.WriteAllText(filePath, task_result); //書き出し
Console.WriteLine($"HTML content saved to {filePath}");