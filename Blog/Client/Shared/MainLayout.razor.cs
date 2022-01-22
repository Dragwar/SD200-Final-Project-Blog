using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace Blog.Client.Shared;
public partial class MainLayout
{
	public enum Theme : int
	{
		Dark = 0,
		Light = 1,
	}

	[Inject]
	private IJSRuntime? JSRuntime { get; set; }
	private ErrorBoundary? ErrorBoundary { get; set; }
	public FluentDesignSystemProvider FluentDesignSystemProvider { get; set; } = new();
	public LocalizationDirection? LocalizationDir { get; set; }
	public Theme SelectedTheme { get; set; } = Theme.Light;

	protected override void OnParametersSet()
	{
		ErrorBoundary?.Recover();
	}

	public async Task SwitchDirectionAsync()
	{
		LocalizationDir = LocalizationDir is LocalizationDirection.rtl
			? LocalizationDirection.ltr
			: LocalizationDirection.rtl;
		await JSRuntime!.InvokeVoidAsync("switchDirection", LocalizationDir.ToString());
	}

	public void SwitchTheme()
	{
		SelectedTheme = SelectedTheme is Theme.Light ? Theme.Dark : Theme.Light;
	}
}
