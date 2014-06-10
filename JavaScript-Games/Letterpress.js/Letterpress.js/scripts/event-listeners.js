function mouseDownListener(evt) {
    var mousePos = getMousePos(canvas, evt);

    // The index of the tile being clicked or -1 if no tile was clicked
    var dragIndex = getDragIndex(mousePos.X, mousePos.Y);

    if (dragIndex > -1) {
        tiles[dragIndex].onMouseDown(mousePos, dragIndex);
    } else {
        // check if one of the buttons is clicked (submit, clear etc.)
    }
    canvas.removeEventListener("mousedown", mouseDownListener, false);
    window.addEventListener("mouseup", mouseUpListener, false);

    // Prevents the mouse down from having an effect on the main browser window:
    evt.preventDefault();
}

function mouseMoveListener(evt) {
    var mousePos = getMousePos(canvas, evt);
    if (isDragging) {
        dragTile.updateTargetPosition(mousePos);
    }
}

function mouseUpListener(evt) {
    canvas.addEventListener("mousedown", mouseDownListener, false);
    window.removeEventListener("mouseup", mouseUpListener, false);
    if (isDragging) {
        isDragging = false;
        dragTile.onMouseUp();
        window.removeEventListener("mousemove", mouseMoveListener, false);
    }
}

// Returns the index of the tile being clicked or -1 if no tile was clicked
function getDragIndex(mouseX, mouseY) {
    var dragIndex = -1;
    // the variable will be overwritten to ensure only the topmost tile is dragged
    for (var i = 0; i < tiles.length; i++)
        if (tiles[i].isClicked(mouseX, mouseY))
            dragIndex = i;
    return dragIndex;
}

// Translates the mouse position to canvas coodrinates
function getMousePos(canvas, evt) {
    var bRect = canvas.getBoundingClientRect();
    return {
        X: (evt.clientX - bRect.left) * (canvas.width / bRect.width),
        Y: (evt.clientY - bRect.top) * (canvas.height / bRect.height)
    };
}

// Runs while the timer is ticking
function onTimerTick() {
    var noTilesMoving = true;
    for (var i = 0; i < tiles.length; i++) {
        tiles[i].move();
        if (tiles[i].isMoving)
            noTilesMoving = false;
    }

    if (noTilesMoving) {
        if (timer) {
            clearInterval(timer);
            timer = false;
        }
    }

    drawScreen();

    console.log(timer);
}

Tile.prototype.onMouseDown = function (mousePos, dragIndex) {
    isDragging = true;
    this.wasDragged = false; // not yet
    window.addEventListener("mousemove", mouseMoveListener, false);

    dragTile = this;
    this.isMoving = true;

    // We now place the currently dragged tile on top by placing it last in the array.
    tiles.push(tiles.splice(dragIndex, 1)[0]);

    // We record the point on the dragged tile where the mouse is "holding" it:          
    this.clickOffsetX = mousePos.X - this.X;
    this.clickOffsetY = mousePos.Y - this.Y;

    // remember where the tile starts from, so it can return back to this position
    this.startDragX = this.X;
    this.startDragY = this.Y;

    // The "target" position is where the object should be if it were to move there instantaneously. But we will
    // set up the code so that this target position is approached gradually, producing a smooth motion. 
    this.targetPosX = this.X;
    this.targetPosY = this.Y;

    // Start timer
    if (!timer)
        timer = setInterval(onTimerTick, 1000 / 60);
}

Tile.prototype.updateTargetPosition = function (mousePos) {
    // check isDragging just in case
    if (isDragging) {
        var minX = 0;
        var maxX = canvas.width - this.size;
        var minY = 0;
        var maxY = canvas.height - this.size;

        // Clamp x and y positions to prevent object from dragging outside of canvas
        this.targetPosX = Math.min(Math.max(mousePos.X - this.clickOffsetX, minX), maxX);
        this.targetPosY = Math.min(Math.max(mousePos.Y - this.clickOffsetY, minY), maxY);
    }
}

Tile.prototype.move = function () {
    if (this.isMoving) {
        // The next variable controls the lag in the tile movement (from 0 to 1)
        var easeAmount = 0.15;
        // Update the moving tile position
        this.X += easeAmount * (this.targetPosX - this.X);
        this.Y += easeAmount * (this.targetPosY - this.Y);

        if ((Math.abs(this.X - this.startDragX) > 2) || (Math.abs(this.Y - this.startDragY) > 2)) {
            this.wasDragged = true;
        }

        // Stop the motion when the target position is reached (close enough)
        if ((!isDragging) && (Math.abs(this.X - this.targetPosX) < 0.1) && (Math.abs(this.Y - this.targetPosY) < 0.1)) {
            // Snap the tile to its final position
            this.X = this.targetPosX;
            this.Y = this.targetPosY;

            this.isMoving = false;
            this.wasDragged = false;
        }
    }

    // >>>>>>>>>>>>>>>>> this is the new tile position -> check if is used and update word holder
}

Tile.prototype.onMouseUp = function () {
    if (this.wasDragged) {

        if (!this.isUsedInWord && ((this.Y - this.targetPosY > 5) || this.Y + this.size / 2 < boardY)) {
            // tile is not used in word and is either moving up or released up
            wordHolder.addTile(this);

        } else if (this.isUsedInWord && (this.Y + this.size / 2 >= boardY)) {
            // tile is used in word and released down
            wordHolder.removeTile(this);

        } else {
            // return it to its starting position (where the drag started)
            this.targetPosX = this.startDragX;
            this.targetPosY = this.startDragY;
        }

    } else {
        // tile was clicked
        if (this.isUsedInWord) {
            wordHolder.removeTile(this);
        } else {
            wordHolder.addTile(this);
        }
    }
}