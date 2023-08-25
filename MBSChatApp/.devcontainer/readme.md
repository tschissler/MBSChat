# Build Android App
To build and run the Android app,
1. Download and install Android Studio from https://developer.android.com/studio
1. Run ```dotnet workload install maui-android```
1. Run ```dotnet publish MBSChatApp.csproj -c:Debug -t:InstallAndroidDependencies -f:net7.0-android -p:AndroidSdkDirectory="/opt/android-sdk-linux" -p:AcceptAndroidSDKLicenses=True``` 

This creates an .apk file that can be deployed to the emulator or on a device