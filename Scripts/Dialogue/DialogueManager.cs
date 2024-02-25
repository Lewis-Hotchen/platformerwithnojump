using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;

namespace PlatformerWithNoJump;

public class DialogueManager
{
    public DialogueEntry Entry { get; set; }

    public DialogueManager(string path, string dialogueEntry)
    {
        if (string.IsNullOrEmpty(dialogueEntry))
        {
            throw new ArgumentException("dialogue wasnt given, stoopid", dialogueEntry);
        }

        var yml = File.ReadAllText(path);
        var deserializer = new DeserializerBuilder().Build();
        var desYaml = deserializer.Deserialize<Dictionary<string, List<string>>>(yml);

        Entry = new DialogueEntry(desYaml.Keys.First(x => x == dialogueEntry), desYaml[dialogueEntry].ToArray());
        var dialogueAsArr = desYaml.ToList();
    }

    public bool NextStep()
    {
        return Entry.IncrementStep();
    }

    public bool PrevStep() {
        return Entry.DecrementStep();
    }

    public string GetStep()
    {
        return Entry.GetCurrentStep;
    }
}