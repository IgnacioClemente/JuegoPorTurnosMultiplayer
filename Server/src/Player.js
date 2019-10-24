var shortid = require('shortid');
var Vector3 = require('./Vector3.js');

module.exports = class Player{
    constructor(){
        this.name = "user_default";
        this.id = shortid.generate();
        this.position = new Vector3();
        this.rotation = new Vector3();
    }
}