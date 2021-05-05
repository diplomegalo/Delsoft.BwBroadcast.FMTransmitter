const express = require("express");
const app = express();
const port = "3000";

app.get("/", (req, res) => res.send("Hello World"));

// set parameters
app.get("/api/setparameter*", (req, res) => {
    console.log(req.params)
    res.contentType("application/xml");
    res.send("<response success='true' />");
});

app.listen(port, () => console.info("App listening on localhost."));
