# Delsoft.BwBroadcast.FMTransmitter

This Windows service allows to use RDS feature of the FM Transmitter from the BW Broadcast brand. The current version can set radio text value of the BwBroadcast FM Transmitter through the api. This value is extract from a file when it's created or modified.

## Now Playing file.

The _now playing_ file contains the information to display on device. 

The service waits for creation or update event of the the _now playing_ file. The file path can be configured in the appSettings.json by setting the value of the `NowPlaying` configuration:

* `FilePath`: the path to the directory.
* `FileName`: the name of the file. 

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

This is a very simple mock the FM Transmitter web api. This mock api is based on node.js and use express js minimalist web server. 

> The authentication is always set to false before calling `getParameter` or `setParameter`.

## Run mock

To start the server you should run `npm start` command.

> Don't forget to run `npm install` the first time.

