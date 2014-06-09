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
     var leftPadding = ~~((canvas.width - (~~(canvas.width / tile.size) * tile.size)) / 2);
     tile.wordX = this.X + leftPadding + this.wordLetters.length * tile.size;
     tile.wordY = this.Y;

     this.wordLetters.push(tile);
     console.log("word length" + "->" + this.wordLetters.length)
 };

  // Removes a tile
 WordHolder.prototype.removeTile = function (tile) {
	var index = this.wordLetters.indexOf(tile);
	if (index > -1) {
	    this.wordLetters.splice(index, 1);
	}

    console.log("word length" + "->" + this.wordLetters.length)
 };