using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class StartMenu : MonoBehaviour
{
    private VisualElement root;
    
    private Label titleLable;
    private Button startButton, creditsButton;

    public UnityEvent OnStartButtonClicked, OnCreditsButtonClicked;
    
    [SerializeField] private string startButtonText, creditsButtonText;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        
        OnStartButtonClicked ??= new UnityEvent();
        OnCreditsButtonClicked ??= new UnityEvent();
        
        titleLable = root.Q<Label>("Title");
        startButton = root.Q<Button>("StartButton");
        creditsButton = root.Q<Button>("CreditsButton");
        
        startButton.text = startButtonText;
        creditsButton.text = creditsButtonText;
        
        startButton.clicked += () => OnStartClicked();
        creditsButton.clicked += () => OnCreditsClicked();
    }

    private void OnStartClicked()
    {
        OnStartButtonClicked?.Invoke();
    }

    private void OnCreditsClicked()
    {
        OnCreditsButtonClicked?.Invoke();
    }

    public void Show()
    {
        root.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        root.style.display = DisplayStyle.None;
    }
}
