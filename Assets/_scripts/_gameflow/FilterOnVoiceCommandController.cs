
using UnityEngine;
using Zenject;

class FilterOnVoiceCommandController : MonoBehaviour
{
    [Inject]
    private KeywordToCommandService keywordToCommandService;

    [Inject]
    private ModelStateController modelStateController;

    private void Start()
    {
        this.keywordToCommandService.Register(GetavizKeyword.FILTER, this.OnFilterVoiceCommand);
    }

    private void OnFilterVoiceCommand()
    {
        this.modelStateController.SwitchState(ModelState.FILTER);
    }
}
