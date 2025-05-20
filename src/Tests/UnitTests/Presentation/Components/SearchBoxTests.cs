using LaunchQ.TakeHomeProject.Presentation.Blazor.Components.Shared.SearchBox;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Bunit;
using Xunit;
using FluentAssertions;

namespace LaunchQ.TakeHomeProject.UnitTests.Presentation.Components
{
    public class SearchBoxTests : TestContext
    {
        [Fact]
        public void SearchBox_ShouldRenderCorrectly()
        {
            // Arrange & Act
            var cut = RenderComponent<SearchBox>(parameters => parameters
                .Add(p => p.Placeholder, "Test placeholder")
                .Add(p => p.SearchButtonText, "Find")
                .Add(p => p.ClearButtonText, "Reset"));

            // Assert
            cut.Find("input").GetAttribute("placeholder").Should().Be("Test placeholder");
            cut.Find("button").TextContent.Should().Contain("Find");
        }

        [Fact]
        public void SearchBox_ShouldNotShowClearButton_WhenEmpty()
        {
            // Arrange & Act
            var cut = RenderComponent<SearchBox>(parameters => parameters
                .Add(p => p.ShowClearButton, true));

            // Assert
            cut.FindAll("button").Count.Should().Be(1); // Apenas o botão de busca deve estar visível
        }

        [Fact]
        public void SearchBox_ShouldShowClearButton_WhenTextIsEntered()
        {
            // Arrange
            var cut = RenderComponent<SearchBox>(parameters => parameters
                .Add(p => p.ShowClearButton, true));

            // Act
            cut.Find("input").Input("test search");

            // Assert
            cut.FindAll("button").Count.Should().Be(2); // Botão de busca e botão de limpar
        }

        [Fact]
        public void SearchBox_HandleSearch_ShouldInvokeCallback()
        {
            // Arrange
            string? searchedText = null;
            var cut = RenderComponent<SearchBox>(parameters => parameters
                .Add(p => p.OnSearch, (string text) => searchedText = text));

            // Act
            cut.Find("input").Input("test search");
            cut.Find("button").Click();

            // Assert
            searchedText.Should().Be("test search");
        }

        [Fact]
        public void SearchBox_HandleClear_ShouldClearTextAndInvokeCallback()
        {
            // Arrange
            string? searchedText = "initial";
            var cut = RenderComponent<SearchBox>(parameters => parameters
                .Add(p => p.OnSearch, (string text) => searchedText = text)
                .Add(p => p.ShowClearButton, true)
                .Add(p => p.InitialSearchText, "test search"));

            // Act
            cut.FindAll("button")[1].Click();

            // Assert
            var inputElement = cut.Find("input");
            inputElement.GetAttribute("value").Should().BeEmpty();
            searchedText.Should().BeEmpty();
        }

        [Fact]
        public void SearchBox_EnterKey_ShouldTriggerSearch()
        {
            // Arrange
            string? searchedText = null;
            var cut = RenderComponent<SearchBox>(parameters => parameters
                .Add(p => p.OnSearch, (string text) => searchedText = text));

            // Act
            cut.Find("input").Input("test search");
            cut.Find("input").KeyUp(new KeyboardEventArgs { Key = "Enter", Type = "keyup" });

            // Assert
            searchedText.Should().Be("test search");
        }
    }
}
