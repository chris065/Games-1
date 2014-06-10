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
     this.updateTilePositions(tile);
     tile.isUsedInWord = true;
 };

 // Removes a tile
 WordHolder.prototype.removeTile = function (tile) {
     var index = this.wordLetters.indexOf(tile);
     if (index > -1) {
         this.wordLetters.splice(index, 1);
         this.updateTilePositions(tile);
         tile.isUsedInWord = false;

         tile.targetPosX = tile.boardX;
         tile.targetPosY = tile.boardY;
     }
 };

 // Update tile positions
 WordHolder.prototype.updateTilePositions = function (tile) {
     var leftPadding = (canvas.width - this.wordLetters.length * tile.size) / 2;
     for (var t = 0; t < this.wordLetters.length; t += 1) {
         this.wordLetters[t].targetPosX = leftPadding + t * tile.size;
         this.wordLetters[t].targetPosY = this.Y;
         this.wordLetters[t].isMoving = true;
     }

     // Start timer
     if (!timer)
         timer = setInterval(onTimerTick, 1000 / 60);
 };