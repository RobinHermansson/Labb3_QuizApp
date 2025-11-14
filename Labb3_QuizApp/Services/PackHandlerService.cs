using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using Labb3_QuizApp.Models;

namespace Labb3_QuizApp.Services;

class PackHandlerService
{
    private string _appName = "QuizApp_Labb3";
    private string _questionPacksName = "QuestionPacks.json";
    private string _localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private string _fullPath;
    private string _fullPathWithFileName;

    public PackHandlerService()
    {
        _fullPath = Path.Combine(_localFolder, _appName);
        _fullPathWithFileName = Path.Combine(_fullPath, _questionPacksName); 
    }

    public List<QuestionPack> LoadAllPacks()
    {
        string fullPath = Path.Combine(_localFolder, _appName);

        if (!Directory.Exists(_fullPath))
        {
            Directory.CreateDirectory(_fullPath);
        }
        if (!File.Exists(_fullPathWithFileName))
        {
            File.Create(_fullPathWithFileName);
            return new List<QuestionPack>() { new QuestionPack("<Default Empty Question Pack>") };
        }
        return JsonSerializer.Deserialize<List<QuestionPack>>(File.ReadAllText(_fullPathWithFileName));

    }

    public void SaveAllPacks(List<QuestionPack> qPacks)
    {
        if (!Directory.Exists(_fullPath))
        {
            Directory.CreateDirectory(_fullPath);
        }
        File.WriteAllText(_fullPathWithFileName ,JsonSerializer.Serialize(qPacks));
    }
}
