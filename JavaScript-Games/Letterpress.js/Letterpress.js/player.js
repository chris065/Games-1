function Player(name, lightColor, darkColor) {
    this.name= name;
    this.lightColor = lightColor;
    this.darkColor = darkColor;
    this.points = 0;
    //this.image;
}


var gameOn = true;
//Main players loop
Player.prototype.play = function (context) {
    while (gameOn){}
};



var red = "rgb(247,153,141)";
var blue = "rgb(120,200,245)";
var gray = "rgb(230, 230, 230)";
var darkRed = "rgb(255, 67, 47)";
var darkBlue = "rgb(0, 162, 255)";

Player1 = new Player("Zhivko", 'red', 'darkRed' );
Player2 = new Player("Villy", 'blue', 'darkBlue');


