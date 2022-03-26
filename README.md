# Delsoft.BwBroadcast.FMTransmitter

Windows service use to send _now playing_ track through RDS. This service use specifically the BW Broadcast FMTransmitter API. Track information is read from a file and send through the BW Broadcast FM Transmitter API every time the file is changing.

This service is written in .net 6.0. 

# Install service

Download the `Delsoft.BwBroadcast.FMTransmitter.RDS.exe` file from release and store it to the local machine where the _now playing_ file is located. Be sure the application has an access on the _now playing_ file.

Create and edit the `appSettings.json` file to configure the application. Below, an example of the config content:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Warning"
    },
    "EventLog": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    }
  },
  "NowPlayingFile": {
    "FilePath" : "C:\\Temp",
    "FileName": "NowPlaying.txt"
  },
    "FilePath": "",
  "TransmitterService": {
    "Endpoint": "http://localhost:3000/",
    "Password": "plop"
  },
  "NowPlayingTrack": {
    "MaxLength": 64
  }
}

```

Install the service by executing the following command where 
- _FMTransmitter.RDS_ is the name of the service
- _C:\Path\To\App.exe_ is the path where the executable file is located
- _C:\Other\Path_ is the path where the `appSettings.json` file is located.

```shell
sc.exe create "FMTransmitter.RDS" binpath="C:\Path\To\App.exe --contentRoot C:\Other\Path"
```

> Be sure you have the sufficent rights to execute this command. See [documentation](https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/sc-create) to find more informations.

# Configuration

The `appSettings.json` file contains information about the configuration. This configuration is load when the service is started. You have to restart the service to reload configuration, for example,  after modifications. 

## `NowPlayingFile` section.

This section contains configuration about the _now playing_ file. The service waits for creation or update event of the the _now playing_ file. The file path and name can be configured :

- `FilePath`: the path to the directory.
- `FileName`: the name of the file. 

Once the watcher detects modification, the file is read and data are send to the FM Transmitter through its web api.

> There is a delay before reading the _now playing_ file to avoid concurrent access and lock while the process is still writing.   

## Transmitter options

The transmitter options can be set in the appSettings.json file in the `Transmitter` section:

* `Endpoint`: the uri of the FM Transmitter endpoint
* `Password`: the password to pass when authentication is needed.

## Authentication

The authentication is done when the success value of response of `setParameter` or `getParmeter` call is `false`. 

> If authentication fail, the system will retry once.

# FM Transmitter Mock

This is a very simple mock of the FM Transmitter web api. This mock api is based on node.js and use the minimalist web server express js . 

> The authentication is always set to false before calling `getParameter` or `setParameter`.

## Run mock

To start the server you should run `npm start` command.

> Don't forget to run `npm install` the first time.

