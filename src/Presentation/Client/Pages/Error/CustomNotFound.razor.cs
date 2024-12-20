﻿using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Client.Pages.Error
{
    public partial class CustomNotFound
	{
		[Inject]
		public NavigationManager NavigationManager { get; set; } = default!;

		public void NavigateToHome()
		{
			NavigationManager.NavigateTo("/");
		}
	}
}
