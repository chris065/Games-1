window.addEventListener("load", main, false);

var canvas, context;
var tiles;
var isDragging;
var timer;
var dragTile;
var wasDragged;
var wordToSubmit = '';
var words = [];
var wordHolder; // holds the letters

function main() {

    prepareCanvas();
    prepareBoard();
    drawScreen();

    canvas.addEventListener("mousedown", mouseDownListener, false);
}

function prepareCanvas() {
    canvas = document.createElement("canvas");
    canvas.width = 800;
    canvas.height = 600;
    canvas.style.position = 'absolute';
    canvas.style.left = (document.documentElement.clientWidth - canvas.width) / 2 + 'px';

    document.body.appendChild(canvas);
    context = canvas.getContext("2d");
}

function prepareBoard() {
    var boardLetters = generateRandomLeters();
    tiles = makeTiles(boardLetters);
    wordHolder = new WordHolder(0, 0);
}

// Renders the canvas to screen
function drawScreen() {
    context.fillStyle = 'rgb(240, 239, 236)';
    context.fillRect(0, 0, canvas.width, canvas.height);

    for (var i = 0; i < tiles.length; i++)
        tiles[i].draw(context);
}

// Returns an array of Tile objects
function makeTiles(boardLetters) {
    var i;
    var tempX, tempY, tempColor;
    var boardX, boardY;
    var red = "rgb(247,153,141)";
    var blue = "rgb(120,200,245)";
    var gray = "rgb(230, 230, 230)";
    var darkRed = "rgb(255, 67, 47)";
    var darkBlue = "rgb(0, 162, 255)";

    var tileSize = 85; // pixels
    var tileMargin = 1; // pixels

    boardX = (canvas.width - 5 * (tileSize + tileMargin)) / 2;
    boardY = (canvas.height - 5 * (tileSize + tileMargin)) - tileSize / 4;

    tiles = [];
    for (i = 0; i < boardLetters.length; i++) {
        var row = ~~ (i / 5);
        var col = i % 5;
        tempX = boardX + col * (tileSize + tileMargin);
        tempY = boardY + row * (tileSize + tileMargin);

        boardPos = {
            row: row,
            col: col
        };

        var rand = Math.random();
        tempColor = rand < 0.33 ? red : rand < 0.66 ? blue : gray;

        tiles.push(new Tile(boardLetters[i], tempColor, tempX, tempY, tileSize, boardPos));
    }

    return tiles;
}



function addToWord(letter) {
    wordToSubmit += letter;
}

function addWord(wordToSubmit) {
    words.push(wordToSubmit);
}