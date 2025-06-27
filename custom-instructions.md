# GitHub Copilot Custom Instructions for ExploreGetRssFeed Solution

## Solution Context

- This solution contains 4 projects, including a Blazor Web project targeting .NET 8.
- Use C# as the primary language. For Blazor, prefer .razor and .cs files.
- Use Entity Framework Core with SQLite for data persistence.
- Follow ASP.NET Core and Blazor best practices.
- Use dependency injection for services.
- Use async/await for I/O and data access.
- Use minimal, clear, and idiomatic C# code.

## Coding Style

- Use PascalCase for class, method, and property names.
- Use camelCase for local variables and parameters.
- Use explicit access modifiers.
- Prefer expression-bodied members for simple properties and methods.
- Use string interpolation with format specifiers over string.Format.
- Use var for local variables when the type is obvious.
- Use nullability annotations where appropriate.
- Use XML documentation comments for public APIs.

## Blazor-Specific Guidance

- Use @inject for dependency injection in .razor files.
- Use EditForm and DataAnnotationsValidator for forms and validation.
- Use strongly-typed models for data binding.
- Use partial classes for code-behind when logic is complex.
- Use async event handlers for UI actions.
- Use @Key directive for lists to improve rendering performance.
- Avoid mixing synchronous and asynchronous code in the same method.
- Minimize JavaScript interaop by using state management techniques.
- Avoid unnecessary rendering of component subtrees.
- Components should be reusable and composable.
- Use RenderFragment for simply, commonly reused UI elements within a Component.

## Entity Framework Core

- Use DbContextFactory for context injection.
- Use async methods for database operations.
- Use migrations for schema changes.

## LINQ Queries

- Use LINQ Queries for data access.
- Limit the resultset size by using pagination.
- Load related entities eagerly when possible.
- Only track entities when adding or updating them.

## General Instructions

- Prefer clear, concise, and maintainable code.
- Add comments only where the intent is not obvious.
- Avoid unnecessary abstractions.
- Ensure code is ready to copy-paste and run.
- Do not include placeholder comments like "existing code here...".
- Adhere to the existing coding style in the solution.

## Example Patterns

```csharp
// avoid expensive rerendering
@code {
    private int prevInboundFlightId = 0;
    private int prevOutboundFlightId = 0;
    private bool shouldRender;

    [Parameter]
    public FlightInfo? InboundFlight { get; set; }

    [Parameter]
    public FlightInfo? OutboundFlight { get; set; }

    protected override void OnParametersSet()
    {
        shouldRender = InboundFlight?.FlightId != prevInboundFlightId
            || OutboundFlight?.FlightId != prevOutboundFlightId;

        prevInboundFlightId = InboundFlight?.FlightId ?? 0;
        prevOutboundFlightId = OutboundFlight?.FlightId ?? 0;
    }

    protected override bool ShouldRender() => shouldRender;
}
```

```csharp
// use EditForm for forms, DataAnnotationsValidator, and ValidationSummary for entry and validation
@page "/starship-2"
@using System.ComponentModel.DataAnnotations
@inject ILogger<Starship2> Logger

<EditForm Model="Model" OnValidSubmit="Submit" FormName="Starship2">
    <DataAnnotationsValidator />
    <ValidationSummary />
    <label>
        Identifier: 
        <InputText @bind-Value="Model!.Id" />
    </label>
    <button type="submit">Submit</button>
</EditForm>

@code {
    [SupplyParameterFromForm]
    private Starship? Model { get; set; }

    protected override void OnInitialized() => Model ??= new();

    private void Submit() => Logger.LogInformation("Id = {Id}", Model?.Id);

    public class Starship
    {
        [Required]
        [StringLength(10, ErrorMessage = "Id is too long.")]
        public string? Id { get; set; }
    }
}
```

```csharp
// use context binding whenever possible
<EditForm ... EditContext="editContext" ...>
    ...
</EditForm>

@code {
    private EditContext? editContext;

    [SupplyParameterFromForm]
    private Starship? Model { get; set; }

    protected override void OnInitialized()
    {
        Model ??= new();
        editContext = new(Model);
    }
}
```

```csharp
// use @key directive for lists, for improved rendering performance

@foreach (var person in people)
{
    <Details @key="person" Data="@person.Data" />
}

@code {
    private Timer timer = new Timer(3000);

    public List<Person> people =
        new()
        {
            { new Person { Data = "Person 1" } },
            { new Person { Data = "Person 2" } },
            { new Person { Data = "Person 3" } }
        };

    protected override void OnInitialized()
    {
        timer.Elapsed += (sender, eventArgs) => OnTimerCallback();
        timer.Start();
    }

    private void OnTimerCallback()
    {
        _ = InvokeAsync(() =>
        {
            people.Insert(0,
                new Person
                {
                    Data = $"INSERTED {DateTime.Now.ToString("hh:mm:ss tt")}"
                });
            StateHasChanged();
        });
    }

    public void Dispose() => timer.Dispose();

    public class Person
    {
        public string? Data { get; set; }
    }
}
```

```csharp
// prefer string interpolation with standard format specifiers over string.Format
var largeNumber = $"{number:N0}"; // 1,234,567
```
