const express = require("express");
const app = express();
const port = "3000";

let nowPlaying = undefined;
let authenticated = false;
const successFalse = "<response success='false' reason='Another user has logged in'/>";

app.get("/", (req, res) => res.send("Hello World"));

// set parameters
app.get("/api/setparameter*", (req, res) => {
    const response = authenticated
        ? "<response success='true'/>"
        : successFalse;

    nowPlaying = req.query.value
    authenticated = false;

    res.contentType("application/xml");
    res.send(response);
});

// get parameters
app.get("/api/getparameter*", (req, res) => {
    const response = authenticated
        ? `<parameters><parameter id='rds.dsn[1].psn[0].rt' value='${nowPlaying}'/></parameters>`
        : successFalse;

    authenticated = false;

    res.contentType("application/xml");
    res.send(response);
});

app.get("/api/auth", (req, res) => {
    authenticated = true;

    res.contentType("application/xml");
    res.send("<response login='true' sid='914523398'/>");
});

app.listen(port, () => console.info("App listening on localhost."));
