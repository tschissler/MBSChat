# How to work with the MBSChat Backend

## The simples option is  to use VSCode with DevContainers
1. Open the Folder MBSChatBackend in VSCode
1. Install the Remote Explotrer extension if not altready present
1. Open the folder in a container by using one of the following options:
    - Click on the green area on the lower left of the VSCode IDE to open the remote window and select "Reopen in container"
    - Use ```<Ctrl>``` + ```<Shitf>``` + ```P``` to open command pallet and type "Dev Containers: Reopen in Container"
1. Use ```python app.py``` to start the app
1. The port of the app inside the container is routed to your host. You can open a browser and go to ```http://localhost:1234/title```