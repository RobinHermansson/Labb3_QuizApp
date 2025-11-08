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

    public PackHandlerService()
    {
        
    }

    public List<QuestionPack> LoadAllPacks()
    {
      return JsonSerializer.Deserialize<List<QuestionPack>>(File.ReadAllText("localPacks.json"));

    }

    public void SaveAllPacks(List<QuestionPack> qPacks)
    {
        File.WriteAllText("localPacks.json", JsonSerializer.Serialize(qPacks));
    }

}
