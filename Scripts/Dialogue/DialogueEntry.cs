using Godot;

namespace PlatformerWithNoJump;

public class DialogueEntry {

    public string Level { get; set; }
    public string[] DialogueSteps { get; set; }
    public int CurrentStep { get; private set; }
    public int NumberOfSteps => DialogueSteps.Length;
    public string GetCurrentStep => DialogueSteps[CurrentStep];

    public DialogueEntry(string level, string[] dialogueSteps)
    {
        Level = level;
        DialogueSteps = dialogueSteps;
        CurrentStep = 0;
    }

    public void SetStep(int step) {
        CurrentStep = step;
    }

    public bool IncrementStep() {
        if(CurrentStep < NumberOfSteps-1){
            CurrentStep+=1;
            return true;
        }

        return false;
    }
}