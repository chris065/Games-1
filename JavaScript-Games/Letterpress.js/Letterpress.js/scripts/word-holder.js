 // A class to hold the top row of letters that form the word
 function WordHolder(posX, posY) {
     this.X = posX;
     this.Y = posY;
     this.wordLetters = [];
 }

 // Returns the letters as string
 WordHolder.prototype.word = function () {
     return this.wordLetters.join('');
 };

 // Clears the letters
 WordHolder.prototype.clear = function () {
     this.wordLetters = [];
 };

 // Adds a tile
 WordHolder.prototype.addTile = function (tile) {
     this.wordLetters.push(tile);
 };