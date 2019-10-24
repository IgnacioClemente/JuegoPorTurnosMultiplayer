var shortid = require('shortid');
var Vector3 = require('./Vector3.js');

module.exports = class ServerObjects{
    constructor(){
        this.name = "object_default";
        this.id = shortid.generate();
        this.position = new Vector3();
    }
}