

 // Returns an array of Tile objects

Player1 = new Player("Zhivko",'red','darkRed' );
Player2 = new Player("Villy", 'blue', 'darkBlue');



console.log(Player2.lightColor);
console.log(Player1.darkColor);





function updateColors(matrix, wordTiles, player, oponent) {
	//use a mask array to recalculate card colors
	//players can have one letter colors
	//when surrounding a card with their colors, 
	//use wordTiles array of Tiles objects , both players and a 
	//color matrix as an input
	console.log(player.lightColor);
	console.log(player.darkColor);
	console.log(oponent.lightColor);
	console.log(oponent.darkColor);
	var defaulColor = 'gray';
    console.log(wordTiles.length + "length");
	//get the correct color variables and start filling new values
	for (var i = 0; i < wordTiles.length; i++) {
		letterCoords = wordTiles[i].coords;
        console.log(i);
        console.log(matrix[letterCoords[0]][letterCoords[1]]);
		if ((matrix[letterCoords[0]][letterCoords[1]]==defaulColor)||(matrix[letterCoords[0]][letterCoords[1]]==oponent.lightColor)) {
			matrix[letterCoords[0]][letterCoords[1]] = player.lightColor;
            console.log("update")
		}
	for (var j = 0; j < matrix.length; j++) {
		for (var k = 0; k < matrix[0].length; k++) {
			if(isSurrounded(matrix, j, k)){
			}
			else if (matrix[j,k]==player.darkColor){
				matrix[j,k] = player.lightColor;
			}
			else if(matrix[j,k]==oponent.darkColor){
				matrix[j,k] = oponent.lightColor;
            }
		}
	}
	return matrix;
}
}

function isSurrounded(arrayIn, xCoord,yCoord) {
	var lenY = arrayIn.length;//rows
	var lenX = arrayIn[0].length;//cols
	var endLine = 0;//get array boundaries, keep indices within the array
	var endX = lenX-1;
	var endY = lenY-1;
	curTileColor= arrayIn[xCoord,yCoord];
	//check if square above is the same
	if ((xCoord==endLine)||(arrayIn[xCoord-1][yCoord][0]==curTileColor)){
		//check one downward
		if((xCoord==endY)||(arrayIn[xCoord+1][yCoord][0]==curTileColor)){
			//check leftwards
			if((yCoord==endLine)||(arrayIn[xCoord][yCoord-1][0]==curTileColor)){
				//check rightwards
				if((yCoord==endX)||(arrayIn[xCoord][yCoord+1][0]==curTileColor)){
					return true;
				}
			}
		}
	}
	return false;
}

//colors are 'r', 'rr','b','bb', 'gray'
var matrix1 =[['gray','gray','gray','gray','gray'],
             ['gray','gray','gray','gray','gray'],
             ['gray','gray','gray','gray','gray'],
             ['gray','gray','gray','gray','gray'],
             ['gray','gray','gray','gray','gray']];
console.log(matrix1 + "arry 1 ");

//var letterCoords = [[0,1],[1,0],[0,0],[4,4],[2,3]];



//console.log(matrix1);
//

word = new Array();

//tiles.push(new Tile(boardLetters[i], tempColor, tempX, tempY, tileSize, posCoords));
// Tile(text, color, posX, posY, size, coords) 
//word = word.push(new Tile('g','gray',30,30,10,[0,1]));
word[0] = new Tile('g','gray',30,30,10,[4,1]);
word[1] = new Tile('g','red',30,30,10,[3,1]);
word[2] = new Tile('g','blue',30,30,10,[0,0]);
word[3] = new Tile('g','blue',30,30,10,[0,1]);
word[4] = new Tile('g','blue',30,30,10,[1,0]);

//console.log(word.length + "  " + typeof(tiles) + "  " +"  " + "  word array"); 
//console.log(m1);


m1 = updateColors(matrix1,word, Player2,Player1);

console.log("m1" + m1);