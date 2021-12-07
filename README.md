# Воркшопк "От клиента к серверу, что имеем под капотом"
Ссылка на воркшоп: https://youtu.be/q-xBjLXOjiQ \
Данный репозиторий содержит проект, показанный воркшопе, с перехваченной зависимостью `IHubProtocol` (`BlazorPackHubProtocol`) и активированным логированием сообщений от клиента к серверу и от сервера к клиенту.

# Краткий список вызовов client -> server
В `HubConnectionHandler.DispatchMessagesAsync` бесконечный цикл, в котором обрабатываются входящие сообщения
Цепочка:
1. `HubConnectionHandler.DispatchMessagesAsync` слушает сообщения
2. `IHubProtocol.TryParseMessage` пытается распарсить сообщение
3. Если все ОК, то тут происходит цепочка вызывов
4. Доходим до `Renderer.DispatchEventAsync`, там вызывается `GetRequiredEventCallback` и получает колбек, соответствующий `eventHandlerId` из `_eventBindings` 
5. в `Renderer.DispatchEventAsync` вызывается полученный колбек 
  6. Затем вызывается `ComponentBase.IHandleEvent.HandleEventAsync`
    7. Затем вызывается `EventCallbackWorkItem.InvokeAsync` выбирается метод с правильной перегрузкой (с `MouseEventArgs` или без) 
			8. И вот мы приходим в `IncrementCount`, к примеру.
  9. После выполнения метода возвращаемся в `IHandleEvent.HandleEventAsync` и дальше вызывается `StateHasChanged()` (поэтому и не нужно его вызывать явно)
