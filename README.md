
I decide to create simple implementation of UI on WPF and setup basic architecture for cross platform application using MVVM pattern.
New platforms support can be easy added.

Interface IWifiWrapper located in the Core platform independent project.
FirstViewModel located in the Core also. It allow us implement business logic inside Core project on abstraction layer and separately inject dependencies in ViewModel constructor on each platform.

Implementation of current interface for windows platform located in WPF/Services/WifiWrapper.cs file.

This implementation based on SimpleWiFi nuget package that based on Native WIFI API and adapted to current interface.
